using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAnalyzer.Models
{
    internal class Product
    {
        public float Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        
        public Product(float price, string name, string description, string link)
        {
            Price = price;
            Name = name;
            Description = description;
            Link = link;
        }
    }
}
