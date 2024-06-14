using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 建立可以using的類別
{
    internal class Program
    {
        static void Main(string[] args)
        {

            using (CanUsingClass u = new CanUsingClass())
            {
                for (int i = 1; i <= 10; i++)
                {
                    u.Write(i.ToString());
                }
            }

            Console.Read();
        }
    }

    /// <summary>
    /// 可以使用 using 的 Class
    /// </summary>
    public class CanUsingClass : IDisposable
    {
        private bool disposedValue;

        public CanUsingClass()
        {
            // 在這邊放在 開始要做的動作
            Console.WriteLine("====================開始執行=====================");
        }

        public void Write(string text) 
        {
            Console.WriteLine(text);
        }

        // 固定放在這個
        public void Dispose()
        {
            this.Dispose(disposing: true);

            // 釋放記憶體
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // 在這邊放在結尾要做的動作
                    Console.WriteLine("====================結束執行=====================");
                }

                this.disposedValue = true;
            }
        }
    }
}
