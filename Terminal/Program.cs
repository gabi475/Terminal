﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using Terminal.Models;
using static System.Console;

namespace Terminal
{
    class Program
    {


        static readonly HttpClient httpClient = new HttpClient();

        static void Main(string[] args)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("User-Agent", "FrakyFashionTerminal");
            httpClient.BaseAddress = new Uri("https://localhost:44343/api/");


            bool shouldRun = true;

            while (shouldRun)
            {
                Clear();

                WriteLine("1. Products");
                WriteLine("2. Categories");
                WriteLine("3. Exit");

                ConsoleKeyInfo keyPressed = ReadKey(true);

                Clear();

                switch (keyPressed.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:

                        AddProduct();

                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:

                        AddCategory();

                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:

                        shouldRun = false;

                        break;

                    default:
                        break;
                }
            }
        }

        private static void AddCategory()
        {
            bool shouldRun = true;

            while (shouldRun)
            {
                Clear();

                WriteLine("1. List categories");
                WriteLine("2. Add category");
                WriteLine("3. Add product to category");
                WriteLine();
                WriteLine("Press Esc to return to main menu");

                ConsoleKeyInfo keyPressed = ReadKey(true);

                switch (keyPressed.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:

                        ListAllCategories();

                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:

                        LäggCategory();  

                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:

                        AddProductToCategory();

                        break;

                    case ConsoleKey.Escape:

                        shouldRun = false;

                        break;

                    default:
                        break;
                }
            }
        }

        private static void AddProductToCategory()
        {

            bool shouldNotExit = false;

            Clear();

            while (!shouldNotExit)
            {
                Write("Product ID: ");
                int.TryParse(ReadLine(), out int productId);

                Write("Category ID: ");
                int.TryParse(ReadLine(), out int categoryId);

                WriteLine();
                WriteLine("Is this correct? (Y)es or (N)o ");


                ConsoleKeyInfo keyPressed = ReadKey(true);

                Clear();
                switch (keyPressed.Key)
                {
                    case ConsoleKey.Y:
                        var productCategory = new ProductCategory(productId, categoryId);




                        var serializedProductCategory = JsonConvert.SerializeObject(productCategory); 

                        var data = new StringContent( 
                            serializedProductCategory,
                            Encoding.UTF8,
                            "application/json");

                        var response = httpClient.PostAsync("AllProductCategories", data).Result; 

                        Clear();

                        if (response.IsSuccessStatusCode)
                        {
                            WriteLine("Product added to category");
                        }
                        else
                        {
                            WriteLine("Failed!");
                        }

                        Thread.Sleep(2000);

                        shouldNotExit = true;

                        break;

                    case ConsoleKey.N:
                        break;

                    case ConsoleKey.L:
                        shouldNotExit = true;
                        break;

                    default:
                        break;
                }
            }


        }


        private static void LäggCategory()
        {
            bool shouldExit = false;

            Clear();

            while (!shouldExit)
            {
                Write("Name: ");
                var name = ReadLine();
                Write("Description: ");
                var description = ReadLine();

                var urlSlug = name.Replace(' ', '-').ToLower();

                WriteLine();
                WriteLine("Is this correct? (Y)es or (N)o  ");
                ConsoleKeyInfo keyPressed = ReadKey(true);
                
                Clear();

                switch (keyPressed.Key)
                {
                    case ConsoleKey.Y:

                        var category = new Category(name,description,urlSlug);


                        var serializedCategory = JsonConvert.SerializeObject(category); 

                        var data = new StringContent( 
                            serializedCategory,
                            Encoding.UTF8,
                            "application/json");

                        var response = httpClient.PostAsync("category", data).Result; 

                        Clear();

                        if (response.IsSuccessStatusCode)
                        {
                            WriteLine("Category added");
                        }
                        else
                        {
                            WriteLine("Failed!");
                        }

                        Thread.Sleep(2000);

                        shouldExit = true;

                        break;

                    case ConsoleKey.N:
                        break;

                    

                    default:
                        break;
                }
            }
        }




        private static void ListAllCategories()
        {
            Clear();

            IEnumerable<Category> categories = FetchCategories();

            Write("ID".PadRight(5, ' '));
            Write("|".PadRight(4, ' '));
            WriteLine("Name");
            WriteLine("--------------------------------------------------");

            foreach (var category in categories)
            {
                Write($"{category.Id}".PadRight(5, ' '));
                Write("|".PadRight(4, ' '));
                WriteLine(category.Name);
            }

            WriteLine();
            Write("(V)iew");

            bool shouldRun = true;

            while (shouldRun)
            {
                ConsoleKeyInfo keyPressed = ReadKey(true);

                switch (keyPressed.Key)
                {
                    case ConsoleKey.V:
                        Write("\r" + new string(' ', WindowWidth) + "\r");
                        Write("View (ID): ");

                        int.TryParse(ReadLine(), out int id);

                        var category = categories.FirstOrDefault(x => x.Id == id);

                        if (category != null)
                        {
                            ViewCategoryDetails(category);
                        }

                        break;

                    case ConsoleKey.Escape:

                        shouldRun = false;

                        break;

                    default:
                        break;
                }
            }
        }

        private static void ViewCategoryDetails(Category category)
        {
            Clear();

            WriteLine($"ID: {category.Id}");
            WriteLine($"Name: {category.Name}");
            WriteLine($"URL slug: {category.UrlSlug}");

            WriteLine();
            WriteLine("Press Esc to return to Category");

            bool validKeyPress = false;

            while (validKeyPress == false)
            {
                ConsoleKeyInfo keyPressed = ReadKey(true);

                if (keyPressed.Key == ConsoleKey.Escape) validKeyPress = true;
            }


        }

        private static IEnumerable<Category> FetchCategories()
        {
            var response = httpClient.GetAsync("category")
               .GetAwaiter()
                .GetResult();


            var categories = Enumerable.Empty<Category>();

            if (response.IsSuccessStatusCode)
            {
                var stringContent = response.Content.ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult();

                categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(stringContent);
            }

            return categories;
        }

        private static void AddProduct()
        {
            bool shouldRun = true;

            while (shouldRun)
            {
                Clear();

                WriteLine("1. List products");
                WriteLine("2. Add product");
                WriteLine();
                WriteLine("Press Esc to return to main menu");

                ConsoleKeyInfo keyPressed = ReadKey(true);

                switch (keyPressed.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:

                        ListAllProducts();

                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:

                       LäggProduct ();

                        break;

                    case ConsoleKey.Escape:

                        shouldRun = false;

                        break;

                    default:
                        break;
                }
            }
        }

        private static void LäggProduct()
        {

            bool Exit = false;

            Clear();

            while (!Exit)
            {
                Write("Name: ");
                var name = ReadLine();

                Write("Description: ");
                var description = ReadLine();

      

                 Write("Price: ");
                int.TryParse(ReadLine(), out int price);

                Write("Image URL: ");
                var imageUrl = new Uri(ReadLine());

                var urlSlug = name.Replace(' ', '-').ToLower();

                WriteLine();
                WriteLine("Is this correct? (Y)es or (N)o ");

                ConsoleKeyInfo keyPressed = ReadKey(true);

                Clear();

                switch (keyPressed.Key)
                {
                    case ConsoleKey.Y:
                        var product = new Product(
                            name,
                           description,
                           price,
                           imageUrl,
                           urlSlug);


                        var serializedProduct = JsonConvert.SerializeObject(product); 

                        var data = new StringContent( 
                            serializedProduct,
                            Encoding.UTF8,
                            "application/json");

                        var response = httpClient.PostAsync("productcategory", data).Result; 

                        Clear();

                        if (response.IsSuccessStatusCode)
                        {
                            WriteLine("Product added");
                        }
                        else
                        {
                            WriteLine("Failed!");
                        }

                        Thread.Sleep(2000);

                        Exit = true;

                        break;

                    case ConsoleKey.N:
                        break;


                    default:
                        break;
                }
            }
        }



        private static void ListAllProducts()
        {
            Clear();
            var endpoint = "https://localhost:44343/api/productcategory";
            var data = httpClient.GetStringAsync(endpoint).Result;
            var products = JsonConvert.DeserializeObject<List<Product>>(data);

            Write("ID".PadRight(5, ' '));
            Write("|".PadRight(4, ' '));
            WriteLine("Name");
            WriteLine("--------------------------------------------------");

            foreach (var product in products)
            {
                Write($"{product.Id}".PadRight(5, ' '));
                Write("|".PadRight(4, ' '));
                WriteLine(product.Name);
            }

            WriteLine();
            Write("(V)iew");

            bool shouldRun = true;

            while (shouldRun)
            {
                ConsoleKeyInfo keyPressed = ReadKey(true);

                switch (keyPressed.Key)
                {
                    case ConsoleKey.V:
                        Write("\r" + new string(' ', WindowWidth) + "\r");
                        Write("View (ID): ");

                        int.TryParse(ReadLine(), out int id);

                        var product = products.FirstOrDefault(x => x.Id == id);

                        if (product != null)
                        {
                            ViewProductDetails(product);
                        }

                        break;

                    case ConsoleKey.Escape:

                        shouldRun = false;

                        break;

                    default:
                        break;
                }
            }
        }

        private static IEnumerable<Product> FetchProducts()
        {

            var response = httpClient.GetAsync("products")
                .GetAwaiter()
                .GetResult();

            var products = Enumerable.Empty<Product>();

            if (response.IsSuccessStatusCode)
            {
                var stringContent = response.Content.ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult();

                products = JsonConvert.DeserializeObject<IEnumerable<Product>>(stringContent);
            }

            return products;
        }

        private static void ViewProductDetails(Product product)
        {
            Clear();

            WriteLine($"ID: {product.Id}");
            WriteLine($"Name: {product.Name}");
            WriteLine($"Description: {product.Description}");
            WriteLine($"Price: {product.Price} SEK");
            Write($"Categories:{product.Name }");


            foreach (var category in product.Products)
            {
                Write($"{category.Category.Name} ");
            }



            WriteLine();
            WriteLine();
            WriteLine("Press Esc to return to Products");

            bool validKeyPress = false;

            do
            {
                ConsoleKeyInfo keyPressed = ReadKey(true);

                validKeyPress = keyPressed.Key == ConsoleKey.Escape;

                if (keyPressed.Key == ConsoleKey.Escape) validKeyPress = true;

            } while (validKeyPress == false);

            ListAllProducts();

        }
    }
}

