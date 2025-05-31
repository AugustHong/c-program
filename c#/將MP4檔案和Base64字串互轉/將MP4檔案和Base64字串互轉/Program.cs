using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 將MP4檔案和Base64字串互轉
{
	class Program
	{
		static void Main(string[] args)
		{
                  string inputFilePath = "input.mp4"; // MP4 檔案路徑
                  string outputFilePath = "output.mp4"; // 生成的新 MP4 檔案路徑

                  // 讀取 MP4 檔案並轉換為 Base64 字串
                  string base64String = Convert.ToBase64String(File.ReadAllBytes(inputFilePath));
                  Console.WriteLine("Base64 String:\n" + base64String); // 只顯示前 100 個字元


                  // 將 Base64 字串轉換回二進位格式並寫入新檔案
                  byte[] fileBytes = Convert.FromBase64String(base64String);
                  File.WriteAllBytes(outputFilePath, fileBytes);

                  Console.WriteLine("新的 MP4 檔案已成功生成: " + outputFilePath);

                  //--------------------------------------------------------------------
                  /*但是因為 MP4檔案天生較大，建議用下面的方法來取*/
                  byte[] fileBytesA;
                  using (FileStream fs = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
                  using (MemoryStream ms = new MemoryStream())
                  {
                        fs.CopyTo(ms);  // 正確累積所有 bytes
                        fileBytesA = ms.ToArray();
                  }
                  string bbb = Convert.ToBase64String(fileBytesA);
                  File.WriteAllText($"Big.txt", bbb);
                  byte[] fileBytesB = Convert.FromBase64String(bbb);
                  File.WriteAllBytes("Big.mp4", fileBytes);


                  Console.Read();
            }
	}
}
