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

        public int ProductId { get; protected set; }
        public Product Product { get; protected set; }
        public int CategoryId { get; protected set; }
        public Category Category { get; protected set; }
    }
}