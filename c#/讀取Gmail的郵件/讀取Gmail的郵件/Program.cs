using ActiveUp.Net.Mail;
using LumiSoft.Net.Mail;
using LumiSoft.Net.MIME;
using LumiSoft.Net.POP3.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 *     方法一(只有Gmail)：
       參考網圵： https://blog.darkthread.net/blog/gmail-imap-sample/
       1. 去加入參考 / 瀏覽 / 我放在最外層 的 DLL 資料夾 內的 ActiveUp.Net.Common.dll 和 ActiveUp.Net.ActiveUp.Net.Imap4.dll

        // ----------------------------------------------------------------------------

        方法二：
        參考網圵： https://www.cnblogs.com/kenjiang/p/11726070.html
        1. 去 NuGet 裝上 LumiSoft.Net
 */

namespace 讀取Gmail的郵件
{
    class Program
    {
        /*
            **2個方法都沒測成功，因為一直登入不進去，不知道是哪邊出問題了 = =
            *
            *後來 把 gmail 開啟 允許低安全應用程式就可以進去了
            *註： 聽說 POP3 的比 IMAP 的讀取還快
            *證： 真的 POP3 比 IMAP 還快 (即 方法二 比 方法一 還快)
         */
        static void Main(string[] args)
        {
            string rootPath = Directory.GetCurrentDirectory() + "\\";
            string goalPath = rootPath + "Output\\";

            #region 方法一 
            string userName = "username";
            string password = "password";

            Imap4Client clnt = new Imap4Client();

            //使用ConnectSsl進行加密連線
            // 第一個是 Host ， 第二個是 Port (這是 Gmail 的)
            // gmail host   ： imap.gmail.com, port : 993
            // outlook host :  outlook.office365.com, port : 993
            var hmm = clnt.ConnectSsl("imap.gmail.com", 993);

            //登入
            try
            {
                clnt.Login(userName, password);

                //取得收件匣
                Mailbox inbox = clnt.SelectMailbox("inbox");

                int messageCount = inbox.MessageCount;

                Console.WriteLine($"總共有 {messageCount} 封 郵件");

                //因讀完信就會移至垃圾桶，故由後讀到前，以免序號變動
                for (int n = messageCount; n > 0; n--)
                {
                    //取回第n封信
                    ActiveUp.Net.Mail.Message m = inbox.Fetch.MessageObject(n);

                    // 是否 已讀
                    bool isRead = inbox.Fetch.Flags(n).Merged == "()";

                    if (m != null)
                    {
                        //為每封郵件建立專屬資料夾(要換掉主旨不能當資料夾名稱的字元)
                        string subject = m.Subject;

                        Console.WriteLine($"處理 Subject = {subject}");

                        // 得到時間
                        // 差了8小時，應是時區問題。ReceiveDate裡用的是UTC標準時間，要變成台灣本地時間需經過轉換，我想改成
                        DateTime receiveDateTime = m.ReceivedDate.ToLocalTime();

                        string goalDir = goalPath + ReplaceInvalidCharsForPathOrFileName(subject + "_" + receiveDateTime.ToString("yyyyMMddHHmmss"));
                        if (Directory.Exists(goalDir))
                        {
                            Directory.Delete(goalDir, true);
                            Directory.CreateDirectory(goalDir);
                        }
                        else
                        {
                            Directory.CreateDirectory(goalDir);
                        }

                        //將信件內文(HTML)寫入MailBody檔案
                        string fileName = "Mail.html";
                        string filePath = goalDir + "\\" + fileName;
                        string content = m.BodyHtml.Text;
                        File.WriteAllText(filePath, content);

                        //逐一寫入附件檔案
                        foreach (MimePart att in m.Attachments)
                        {
                            string attFilePath = goalDir + "\\" + ReplaceInvalidCharsForPathOrFileName(att.Filename);
                            byte[] attContent = att.BinaryContent;
                            File.WriteAllBytes(attFilePath, attContent);
                        }

                        //將信件移至垃圾桶(CopyMessage即可產生移動資料夾的效果)
                        //inbox.CopyMessage(n, "[Gmail]/Trash");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"發生錯誤： {ex.Message}");
            }

            #endregion

            Console.WriteLine("******************************************************************************");

            #region 方法二

            // Create a POP3 client and connect.
            // 上面方法一是 IMAP 的， 這邊是 POP3 的
            POP3_Client client = new POP3_Client();

            try
            {
                // 連接
                // outlook host :outlook.office365.com , port : 995
                // gmail host : pop.gmail.com, port : 995
                client.Connect("pop.gmail.com", 995, true);  // (Host, Port, 是否使用ssl加密)
                client.Login("username(gmail)", "password");        // (使用者名稱, 密碼)

                // 得到 郵箱
                POP3_ClientMessageCollection inbox = client.Messages;
                int messageCount = inbox.Count;

                Console.WriteLine($"總共有 {messageCount} 封 郵件");

                // 跑每個郵件
                for (var n = messageCount - 1; n >= 0; n--)
                {
                    // 得到當前郵件
                    POP3_ClientMessage m = inbox[n];
                    byte[] messageBytes = m.MessageToByte();
                    Mail_Message message = Mail_Message.ParseFromByte(messageBytes);

                    if (message != null)
                    {
                        //為每封郵件建立專屬資料夾(要換掉主旨不能當資料夾名稱的字元)
                        string subject = message.Subject;

                        Console.WriteLine($"處理 Subject = {subject}");

                        // 得到日期
                        DateTime receiveDateTime = message.Date.ToLocalTime();

                        string goalDir = goalPath + ReplaceInvalidCharsForPathOrFileName(subject + "_" + receiveDateTime.ToString("yyyyMMddHHmmss"));
                        if (Directory.Exists(goalDir))
                        {
                            Directory.Delete(goalDir, true);
                            Directory.CreateDirectory(goalDir);
                        }
                        else
                        {
                            Directory.CreateDirectory(goalDir);
                        }

                        //將信件內文(HTML)寫入MailBody檔案
                        string fileName = "Mail.html";
                        string filePath = goalDir + "\\" + fileName;
                        string content = message.BodyHtmlText;
                        //string data = message.BodyText;
                        File.WriteAllText(filePath, content);

                        //下載附件
                        foreach (var attach in message.Attachments)
                        {
                            var disposition = attach.ContentDisposition;
                            if (disposition != null)
                            {
                                string attFilePath = goalDir + "\\" + ReplaceInvalidCharsForPathOrFileName(disposition.Param_FileName);
                                MIME_b_SinglepartBase byteObj = (MIME_b_SinglepartBase)attach.Body;
                                Stream attContent = byteObj.GetDataStream();
                                CopyStream(attFilePath, attContent);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"發生錯誤： {ex.Message}");
            }

            #endregion

            Console.WriteLine("=================================================================");
            Console.WriteLine("執行完成");
            Console.Read();
        }

        //將不可做為路徑名稱的字元換成_
        static string ReplaceInvalidCharsForPathOrFileName(string raw, char replaceChar = '_')
        {
            List<char> invalid = new List<char>();
            invalid.AddRange(Path.GetInvalidPathChars());
            invalid.AddRange(Path.GetInvalidFileNameChars());

            var invalidList = invalid.Distinct();

            foreach (char c in invalidList)
            {
                raw = raw.Replace(c, replaceChar);
            }

            return raw;
        }

        /// <summary>
        ///  寫入 Stream
        /// </summary>
        /// <param name="input"></param>
        /// <param name="filePath"></param>
        static void CopyStream(string filePath, Stream input)
        {
            FileStream fs = null;
            try
            {
                fs = File.Create(filePath);
                byte[] buffer = new byte[8 * 1024];
                int len;
                while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fs.Write(buffer, 0, len);
                }
                fs.Close();
            }
            catch (Exception)
            {
                if (fs != null)
                {
                    fs.Close();
                }
                throw;
            }
        }
    }
}
