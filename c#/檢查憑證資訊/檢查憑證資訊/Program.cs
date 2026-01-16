using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;


namespace 檢查憑證資訊
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("請輸入網址 (要包含 https 。 例如 https://www.google.com 或 https://example.com:8443): ");
            string url = Console.ReadLine();

            ShowCertificateData(url);
            Console.Read();
        }

        static void ShowCertificateData(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                string host = uri.Host;

                // 如果網址有指定 port 就用指定的，否則用預設值
                int port = uri.Port;
                if (port == -1) // 沒指定 port
                {
                    port = uri.Scheme == Uri.UriSchemeHttps ? 443 : 80;
                }

                if (uri.Scheme != Uri.UriSchemeHttps)
                {
                    // Http 沒有憑證
                    Console.WriteLine("此網址不是 HTTPS，沒有憑證可檢查。");
                    return;
                }

                using (TcpClient client = new TcpClient(host, port))
                using (SslStream sslStream = new SslStream(client.GetStream(), false,
                    new RemoteCertificateValidationCallback((sender, certificate, chain, errors) => true)))
                {
                    sslStream.AuthenticateAsClient(host);

                    X509Certificate2 cert = new X509Certificate2(sslStream.RemoteCertificate);

                    DateTime notBefore = cert.NotBefore;
                    DateTime notAfter = cert.NotAfter;
                    DateTime now = DateTime.UtcNow;

                    Console.WriteLine($"憑證主題 (Subject): {cert.Subject}");
                    Console.WriteLine($"憑證發行者 (Issuer): {cert.Issuer}");
                    Console.WriteLine($"憑證生效時間: {notBefore}");
                    Console.WriteLine($"憑證到期時間: {notAfter}");

                    if (now > notAfter)
                    {
                        Console.WriteLine("憑證已過期！");
                    }
                    else
                    {
                        TimeSpan remaining = notAfter - now;
                        Console.WriteLine($"憑證尚未過期，剩餘 {remaining.Days} 天");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"發生錯誤: {ex.Message}");
            }
        }
    }
}
