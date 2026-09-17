using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.Playwright;

/*
    前置作業：
    (1) 先去 NuGet 裝上 Microsoft.Playwright
    (2) AI說有 需要去 裝 Playwright Browser，但我直接啟動還是可以，所以後續要再問了
    (3) 若要錄影的話要裝 ffmpeg (同上，先建置完後 Debug底下就會有 ps1 檔，再執行 (在 Debug的目錄裡，用 PowerShell程式來執行)
        pwsh playwright.ps1 install ffmpeg
        若要用 CMD 執行的話 改成下面的
        powershell -ExecutionPolicy Bypass -File "playwright.ps1" install ffmpeg
 */

namespace Playwright_自動化測試_
{
    class Program
    {
        private static int _screenshotNumber = 0;

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            // ========================================================
            // 1. 建立 Output 目錄
            // ========================================================

            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string outputDirectory = Path.Combine(exeDirectory, "output", timestamp);

            Directory.CreateDirectory(outputDirectory);

            Console.WriteLine("======================================");
            Console.WriteLine("Playwright Test");
            Console.WriteLine("======================================");
            Console.WriteLine($"Output Directory: {outputDirectory}");

            // ========================================================
            // 影片暫存目錄
            // ========================================================
            string videoDirectory = Path.Combine(exeDirectory, "output", timestamp, "video");
            Directory.CreateDirectory(videoDirectory);

            try
            {
                // ====================================================
                // 2. 建立 Playwright
                // ====================================================

                using (IPlaywright playwright = await Playwright.CreateAsync())
                {
                    // =================================================
                    // 3. 啟動 Google Chrome
                    //
                    // Channel = "chrome"
                    //
                    // 使用電腦已安裝的 Google Chrome
                    // =================================================

                    BrowserTypeLaunchOptions options =
                        new BrowserTypeLaunchOptions
                        {
                            Channel = "chrome",
                            // 顯示 Chrome
                            Headless = false,
                            // 每個操作稍微停一下
                            SlowMo = 400
                        };

                    IBrowser browser = await playwright.Chromium.LaunchAsync(options);

                    // =============================================
                    // 4. 建立 Browser Context
                    //
                    // 注意：
                    // 錄影是設定在 Context，不是 Page
                    // =============================================

                    IBrowserContext context =
                        await browser.NewContextAsync(
                            new BrowserNewContextOptions
                            {
                                // 錄影目錄
                                RecordVideoDir = videoDirectory,

                                // 影片尺寸
                                RecordVideoSize =
                                    new RecordVideoSize
                                    {
                                        Width = 1280,
                                        Height = 720
                                    }
                            });

                    try
                    {
                        // =============================================
                        // 4. 建立 Page
                        // =============================================

                        IPage page = await context.NewPageAsync();

                        // =============================================
                        // Step 1
                        // =============================================

                        Console.WriteLine("[Step 1] 開啟 Google");

                        await page.GotoAsync(
                            "https://www.google.com.tw",
                            new PageGotoOptions
                            {
                                WaitUntil =
                                    WaitUntilState.DOMContentLoaded
                            });

                        // ---------------------------------------------
                        // 驗證 Google 是否成功開啟
                        // ---------------------------------------------

                        if (!page.Url.Contains("google.com"))
                        {
                            throw new Exception(
                                "Google 開啟失敗。" +
                                Environment.NewLine +
                                "目前 URL: " +
                                page.Url);
                        }

                        // ---------------------------------------------
                        // Screenshot #1
                        // ---------------------------------------------

                        await TakeScreenshotAsync(page, outputDirectory, "Google首頁");

                        Console.WriteLine("Step 1 PASS");
                        Console.WriteLine();

                        // =============================================
                        // Step 2
                        // 找搜尋框
                        // =============================================

                        Console.WriteLine("[Step 2] 輸入 Playwright");

                        ILocator searchBox =
                            page.Locator(
                                "textarea[name='q'], " +
                                "input[name='q']")
                            .First;

                        await searchBox.WaitForAsync(
                            new LocatorWaitForOptions
                            {
                                State =
                                    WaitForSelectorState.Visible,

                                Timeout = 10000
                            });

                        // ---------------------------------------------
                        // 輸入 Playwright
                        // ---------------------------------------------

                        await searchBox.FillAsync("Playwright");

                        // ---------------------------------------------
                        // Screenshot #2
                        // ---------------------------------------------

                        await TakeScreenshotAsync(
                            page,
                            outputDirectory,
                            "輸入Playwright");

                        Console.WriteLine("Step 2 PASS");
                        Console.WriteLine();

                        // =============================================
                        // Step 3
                        // 按 Enter
                        // =============================================

                        Console.WriteLine("[Step 3] 按下 Enter");

                        await searchBox.PressAsync("Enter");

                        // ---------------------------------------------
                        // 等待 Google Search URL
                        // ---------------------------------------------

                        await page.WaitForURLAsync(
                            new Regex(
                                @"https://www\.google\.com" +
                                @"(\.[a-z.]+)?/search.*"),
                            new PageWaitForURLOptions
                            {
                                Timeout = 15000
                            });

                        // ---------------------------------------------
                        // 驗證 URL
                        // ---------------------------------------------

                        if (!page.Url.Contains("/search"))
                        {
                            throw new Exception(
                                "沒有進入 Google 搜尋結果頁。" +
                                Environment.NewLine +
                                "目前 URL: " +
                                page.Url);
                        }

                        // ---------------------------------------------
                        // 驗證搜尋結果
                        // ---------------------------------------------

                        ILocator result =
                            page.GetByText(
                                "Playwright",
                                new PageGetByTextOptions
                                {
                                    Exact = false
                                })
                            .First;

                        await result.WaitForAsync(
                            new LocatorWaitForOptions
                            {
                                State =
                                    WaitForSelectorState.Visible,

                                Timeout = 10000
                            });

                        // ---------------------------------------------
                        // Screenshot #3
                        // ---------------------------------------------

                        await TakeScreenshotAsync(page, outputDirectory, "搜尋結果");

                        Console.WriteLine("Step 3 PASS");
                        Console.WriteLine();

                        // =============================================
                        // TEST PASS
                        // =============================================

                        Console.WriteLine("======================================");
                        Console.WriteLine("TEST PASS");
                        Console.WriteLine("======================================");
                        Console.WriteLine($"目前 URL: {page.Url}");
                        Console.WriteLine($"Screenshots: {outputDirectory}");

                        // 保留 Chrome 2 秒
                        await Task.Delay(2000);
                    }
                    finally
                    {
                        // =================================================
                        // 非常重要
                        //
                        // 關閉 Context 後 Playwright 才會完成影片檔案
                        // =================================================

                        await context.CloseAsync();
                        await browser.CloseAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // ====================================================
                // TEST FAIL
                // ====================================================

                Console.WriteLine();
                Console.WriteLine("======================================");
                Console.WriteLine("TEST FAIL");
                Console.WriteLine("======================================");
                Console.WriteLine($"Exception ： {ex.Message}");
                Console.WriteLine($"Screenshots: {outputDirectory}");
            }

            Console.WriteLine();
            Console.WriteLine("Press ENTER to exit.");

            Console.ReadLine();
        }

        // ============================================================
        // Screenshot
        // ============================================================

        private static async Task TakeScreenshotAsync(
            IPage page,
            string outputDirectory,
            string description)
        {
            _screenshotNumber++;

            string fileName = _screenshotNumber + ".jpg";

            string filePath =Path.Combine(outputDirectory, fileName);

            await page.ScreenshotAsync(
                new PageScreenshotOptions
                {
                    Path = filePath,

                    // JPEG
                    Type = ScreenshotType.Jpeg,

                    // JPG 品質
                    Quality = 90,

                    // 整個頁面
                    FullPage = true
                });

            Console.WriteLine("Screenshot #" + _screenshotNumber);
            Console.WriteLine("Description: " + description);
            Console.WriteLine("File: " + filePath);

            Console.WriteLine();
        }
    }
}
