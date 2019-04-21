using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Mail;    //要寄信要引用這個

namespace Hong.MailHelper
{
    public class MailHelper
    {
        /// <summary>
        /// 寄信人Mail位址
        /// </summary>
        public string SenderAddress;

        /// <summary>
        /// 密碼
        /// </summary>
        public string Password;


        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="sender">寄信人mail位址</param>
        /// <param name="passwd">密碼</param>
        public MailHelper(string sender, string passwd)
        {
            SenderAddress = sender;
            Password = passwd;
        }

        /// <summary>
        /// 寄信（多個一次寄）
        /// </summary>
        /// <param name="data">Mail內容列表（List）</param>
        /// <param name="senderName">寄信人名字（可隨便填，預設為Hong）</param>
        public string SendMails(List<MailContent> data, string senderName)
        {
            //成功寄出的數量
            int successCount = 0;

            //寄出失敗的數量
            int errorCount = 0;

            //跑過所有資料
            foreach(var d in data)
            {
                //呼叫下方的一次寄信
                bool result = Send(d, senderName);

                //更新成功或失敗
                if (result) { successCount++; } else { errorCount++; }
            }

            return "成功：" + successCount.ToString() + "筆； 失敗：" + errorCount.ToString() + "筆";
        }


        /// <summary>
        /// 寄信（一次）
        /// </summary>
        /// <param name="d">Mail內容</param>
        /// <param name="senderName">寄信人名字（可隨便填，預設為Hong）</param>
        /// <returns></returns>
        public bool Send(MailContent d, string senderName = "Hong")
        {
            //如果收信人一個都沒有，傳false
            if (d.Recipient.Count() <= 0) { return false; }

            try
            {
                //宣告MalMessage類型
                MailMessage msg = new MailMessage();

                //跑過所有收件人
                foreach (var r in d.Recipient)
                {
                    //新增收件人
                    msg.To.Add(r);
                }

                //跑過所有副本
                foreach (var c in d.Copy)
                {
                    //新增副本
                    msg.CC.Add(c);
                }

                //3個參數分別是發件人地址（可以隨便寫），發件人姓名，編碼
                msg.From = new MailAddress(SenderAddress, senderName, Encoding.UTF8);

                //標題
                msg.Subject = d.Title;

                //標題編碼（都用utf-8）
                msg.SubjectEncoding = Encoding.UTF8;

                //內文
                msg.Body = d.Content;

                //內文編碼（都用utf-8）
                msg.BodyEncoding = Encoding.UTF8;

                //跑過所有的附件
                foreach (var a in d.Attachment)
                {
                    //新增附件
                    msg.Attachments.Add(new Attachment(@"" + a));
                }

                //是否html郵件
                msg.IsBodyHtml = d.IsBodyHtml;

                //優先等級
                msg.Priority = d.Priority == 2 ? MailPriority.High : d.Priority == 1 ? MailPriority.Normal : MailPriority.Low;


                #region 設定完參數要開始寄信

                //宣告SmtpClient類型
                SmtpClient client = new SmtpClient();

                //設定寄件人的資料
                client.Credentials = new System.Net.NetworkCredential(SenderAddress, Password);

                //SMTP的Server
                client.Host = d.Host;

                client.Port = 25; //設定Port
                client.EnableSsl = true; //gmail預設開啟驗證
                client.Send(msg); //寄出信件
                client.Dispose();
                msg.Dispose();

                #endregion

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }

    /// <summary>
    /// 寄信所要用到的屬性（變成一個類別，讓別人好使用）
    /// </summary>
    public class MailContent
    {
        /// <summary>
        /// 收件人（可以多個，但至少一個）
        /// </summary>
        public List<string> Recipient;

        /// <summary>
        /// 副本（可以多個）
        /// </summary>
        public List<string> Copy;

        /// <summary>
        /// 標題
        /// </summary>
        public string Title;

        /// <summary>
        /// 內容文字
        /// </summary>
        public string Content;

        /// <summary>
        /// 附件（請輸入完整路徑，可以多個）
        /// </summary>
        public List<string> Attachment;

        /// <summary>
        /// 是否是Html郵件（預設是true）
        /// </summary>
        public bool IsBodyHtml;

        /// <summary>
        /// 優先等級（0=低；1=中等；2=高） => 若輸入不是這幾個數值預設為0
        /// </summary>
        public int Priority;

        /// <summary>
        /// 設定SMTP Server（預設為Gmail）
        /// </summary>
        public string Host;


        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="title">標題</param>
        /// <param name="content">內容</param>
        public MailContent(string title, string content)
        {
            Recipient = new List<string>();
            Copy = new List<string>();
            Title = title;
            Content = content;
            Attachment = new List<string>();
            IsBodyHtml = true;
            Priority = 0;
            Host = "smtp.gmail.com";
        }
    }
}
