using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V130.Page;
using PriceAnalyzer.Models;
using static System.Net.Mime.MediaTypeNames;

namespace PriceAnalyzer.Manager
{
    public class BrowserManager
    {
        private readonly IWebDriver driver;

        public BrowserManager()
        {
            string pathToChrome = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            driver = new ChromeDriver(pathToChrome);
        }
        
        public async Task<List<Product>> GetFromDNS()
        {
            List<Product> allProducts = new List<Product>();
            this.driver.Navigate().GoToUrl("https://www.dns-shop.ru/?ysclid=m6dl93sj62382412681&utm_medium=organic&utm_source=yandex&utm_referrer=https%3A%2F%2Fyandex.ru%2F");
            List<IWebElement> categories = new List<IWebElement>();
            categories.AddRange(driver.FindElements(By.ClassName("catalog-menu__root-item")));
            foreach (var category in categories)
            {
                category.Click();
                List<Product> categoryProducts = await GetProductDataFromDNS();
                allProducts.AddRange(categoryProducts);
            }
            return allProducts;
        }

        private async Task<List<Product>> GetProductDataFromDNS()
        {
            List<Product> products = new List<Product>();
            List<IWebElement> subcategories = new List<IWebElement>();
            subcategories.AddRange(driver.FindElements(By.ClassName("subcategory__item ui-link ui-link_blue")));
            foreach (var subcategory in subcategories)
            {
                subcategory.Click();
                if (driver.FindElements(By.ClassName("subcategory__item ui-link ui-link_blue")) == null)
                {
                    List<IWebElement> units = new List<IWebElement>();
                    units.AddRange(driver.FindElements(By.ClassName("catalog-product ui-button-widget  ")));
                    foreach (var unit in units)
                    {
                        unit.Click();
                        var name = driver.FindElement(By.ClassName("product-card-top__title")).Text;
                        var price = float.Parse(driver.FindElement(By.ClassName("product-buy__price")).Text); // убрать знак рубля
                        var description = driver.FindElement(By.ClassName("product-card-description-text")).Text;
                        var link = driver.PageSource.ToString();
                        products.Add(new Product(price,name,description,link));
                    }
                }
                else
                {
                    await GetProductDataFromDNS();
                }
            }
            return products;
        }
    }
}
