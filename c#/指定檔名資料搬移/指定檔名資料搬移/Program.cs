using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 指定檔名資料搬移
{
    public class HandleData
    {
        /// <summary>
        /// 指定目錄
        /// </summary>
        public string dir { get; set; }

        /// <summary>
        /// 篩選字串
        /// </summary>
        public string filterStr { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // 查詢 資料夾目錄 (搭配 dir 使用，也可以都組在 dir 裡面)
            string queryRootDir = "D:\\";

            string outputDir = "output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
            else
            {
                // 先刪除，再建立
                Directory.Delete(outputDir, true);
                Directory.CreateDirectory(outputDir);
            }
            DirectoryInfo o = new DirectoryInfo(outputDir);

            DirectoryInfo d = new DirectoryInfo("input");

            // 取出第1個 csv檔， 裡面有全部的清單
            FileInfo firstFile = d.GetFiles().Where(x => x.Extension.ToLower() == ".csv").FirstOrDefault();

            if (firstFile != null)
            {
                string inputFileName = firstFile.Name;
                Console.WriteLine($"執行檔案： {inputFileName}");
                Console.WriteLine("-------------------------------------------------------");

                List<HandleData> lines = new List<HandleData>();
                using (StreamReader sr = new StreamReader(firstFile.FullName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        // 有包含 , 
                        if (line.Contains(","))
                        {
                            List<string> data = line.Split(',').ToList();
                            if (data.Count() == 2)
                            {
                                // 格式： 日期,單號
                                // 2024/04/01,E40101184145816
                                string dir = data[0];
                                string filterStr = data[1];

                                // 重新加入回去
                                lines.Add(new HandleData
                                {
                                    dir = dir,
                                    filterStr = filterStr
                                });
                            }
                        }
                    }
                }

                Console.WriteLine("-----------------------執行資料--------------------------------");

                /*
                // 先取出相同的資料夾，一起處理
                List<string> dirs = lines.Select(x => x.dir).Distinct().ToList();

                foreach (string dir in dirs)
                {
                    Console.WriteLine("*********");
                    Console.WriteLine($"處理資料夾： {dir}");

                    try
                    {
                        string filterDir = Path.Combine(queryRootDir, dir);
                        DirectoryInfo pdir = new DirectoryInfo(filterDir);

                        // 符合這個資料夾下的 指定資料
                        List<string> procStrList = lines.Where(x => x.dir == dir).Select(x => x.filterStr).ToList();
                        // GetFiles() 的速度小於 EnumerateFiles() 很多，所以改成用 EnumerateFiles()
                        // 參考網址： https://www.cnblogs.com/liweis/p/17946887
                        //List<FileInfo> procFileList = pdir.GetFiles().Where(x => procTicketNoList.Count(p => x.Name.Contains(p)) > 0).ToList();
                        List<FileInfo> procFileList = pdir.EnumerateFiles().AsParallel().Where(x => procTicketNoList.Count(p => x.Name.Contains(p)) > 0).ToList();
                        Console.WriteLine($"符合的檔案數： {procFileList.Count()}");
                        foreach (FileInfo f in procFileList)
                        {
                            Console.WriteLine($"搬移 {f.Name} 檔案");
                            File.Copy(f.FullName, o.Name + "\\" + f.Name, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"處理資料夾： {dir} 發生錯誤： {ex.Message}");
                    }
                    Console.WriteLine("*********");
                }

                */

                // 改用 Cmd (xcopy) 來執行
                foreach (HandleData line in lines)
                {
                    Console.WriteLine("*********");
                    string cmd = $"xcopy /d /y /s \"{Path.Combine(queryRootDir, line.dir)}\\*{line.filterStr}*.*\" \"{o.FullName}\\\"";
                    Console.WriteLine($"執行的執令(可看筆數)： {cmd}");
                    string runCmdResult = ExecuteCommand(cmd);
                    if (!string.IsNullOrEmpty(runCmdResult))
                    {
                        Console.WriteLine($"發生錯誤： {runCmdResult}");
                    }
                    else
                    {
                        Console.WriteLine($"成功！！");
                    }

                    Console.WriteLine("*********");
                }
            }
            else
            {
                Console.WriteLine("查無輸入的 csv 檔案，結束程式！");
            }

            Console.WriteLine("####################程式完成########################");
            Console.Read();
        }

        static string ExecuteCommand(string command)
        {
            string result = string.Empty;
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo("cmd.exe")
                {
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process process = new Process { StartInfo = psi };
                process.Start();

                process.StandardInput.WriteLine(command);
                process.StandardInput.Close();

                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                return result;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
