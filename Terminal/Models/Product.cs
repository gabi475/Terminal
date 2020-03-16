using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Models
{

    class Product
    {
        public Product(string name, string description, int price, Uri imageUrl, string urlSlug)
        {
            Name = name;
            Description = description;
            Price = price;
            ImageUrl = imageUrl;
            UrlSlug = urlSlug;
        }
    

    public Product()
    {

    }

        public Product(int id, string name, string description,int price,Uri imageUrl,string urlSlug)
           : this(name, description,price,imageUrl,urlSlug)
        {
            Id = id;
        }

       


        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public Uri ImageUrl { get; set; }
        public string UrlSlug { get; set; }

        public List<ProductCategory> Products { get; set; } = new List<ProductCategory>();
    }
}
