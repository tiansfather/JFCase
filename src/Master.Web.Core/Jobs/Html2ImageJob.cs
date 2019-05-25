using Abp.BackgroundJobs;
using Abp.Dependency;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Master.Web.Jobs
{
    /// <summary>
    /// 将html转图片任务
    /// </summary>
    public class Html2ImageJob : BackgroundJob<Html2ImageJobArgs>, ITransientDependency
    {
        public override void Execute(Html2ImageJobArgs args)
        {
            ChromeOptions op = new ChromeOptions();
            op.BinaryLocation = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            op.AddArguments("--headless");//开启无gui模式
            op.AddArguments("--no-sandbox");//停用沙箱以在Linux中正常运行
            //op.AddArguments("--start-maximized");
            op.AddArguments($"--window-size={args.Width},{args.Height}");//设置窗口大小
            ChromeDriver driver = new ChromeDriver(Environment.CurrentDirectory, op, TimeSpan.FromSeconds(180));
            driver.Navigate().GoToUrl(args.Url);

            if (!string.IsNullOrEmpty(args.WaitForId))
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(o => o.FindElement(By.Id(args.WaitForId)).Displayed);
            }
            //截图保存
            Screenshot screenshot = driver.GetScreenshot();
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(args.FilePath));


            screenshot.SaveAsFile(args.FilePath, ScreenshotImageFormat.Png);
            //退出
            driver.Quit();
        }
    }

    public class Html2ImageJobArgs
    {
        public string Url { get; set; }
        public string FilePath { get; set; }
        public int Width { get; set; } = 1024;
        public int Height { get; set; } = 768;
        /// <summary>
        /// 要等待显示的domId
        /// </summary>
        public string WaitForId { get; set; }
    }
}
