using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Models
{

    class Category
    {
        


        public Category(string name, string description, string urlSlug)
        {
            Name = name;
            Description = description; 
            UrlSlug = urlSlug;
        }



        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string UrlSlug { get; set; }



        public Uri ImageUrl { get; set; }
        public List<ProductCategory> Products { get; set; } = new List<ProductCategory>();
    }
}