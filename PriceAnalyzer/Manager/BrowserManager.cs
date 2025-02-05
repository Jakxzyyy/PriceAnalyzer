using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PuppeteerSharp;

namespace PriceAnalyzer.Manager
{
    public class BrowserManager
    {
        public Browser browser;
        public Page page;
        LaunchOptions launchOptions;
        private List<string> sites;

        public BrowserManager()
        {
            string browserPath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            launchOptions.ExecutablePath = browserPath;
            string sitesTxtPath = @"\PriceAnalyzer\Resources\Sites.txt";
            sites = File.ReadAllLines(sitesTxtPath).ToList();
        }

        public async Task<string> LoadHtmlPage(string url)
        {
            if (browser == null)
            {
                await InitBrowser();
                page = await GetPage();
            }
            var response = await page.GoToAsync(url);
            var jsonString = await response.TextAsync();

            await browser.DisposeAsync();

            return jsonString;
        }
        public async Task InitBrowser()
        {
            browser = (Browser)await Puppeteer.LaunchAsync(launchOptions);
        }
        public async Task<Page> GetPage()
        {
            return (Page)await browser.NewPageAsync();
        }
        public async Task<List<string>> GetUrls()
        {
            List<string> urls = null; //чота
            return urls;
        }
    }
}
