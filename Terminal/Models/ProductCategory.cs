using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Models
{


    class ProductCategory
    {
        public ProductCategory(int productId, int categoryId)
        {
            ProductId = productId;
            CategoryId = categoryId;
        }
        public ProductCategory()
        {

        }

        public int ProductId { get; set; }
        public Product Product { get;  set; }
        public int CategoryId { get;  set; }
        public Category Category { get;  set; }
    }
}