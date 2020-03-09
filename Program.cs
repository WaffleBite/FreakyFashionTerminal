using FreakyFashionTerminal.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace FreakyFashionTerminal
{
    class Program
    {
        static readonly HttpClient httpClient = new HttpClient();

        static StringBuilder sb = new StringBuilder();
        static void Main(string[] args)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            //vi ska även skicka med en header som vi helst vill ha, vilken data vi helst vill ha
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("User-Agent", "HighscoreTerminal"); // för att servern ska veta vem vi är
            httpClient.BaseAddress = new Uri("https://localhost:5001/api/"); // för att slippa skriva hela url-en

            bool isRunning = true;

            sb.Append('.', Console.WindowWidth);

            while (isRunning)
            {
                Console.Clear();

                Console.WriteLine("1. Products");
                Console.WriteLine("2. Categories");
                Console.WriteLine("3. Exit");

                ConsoleKeyInfo keyPressed = Console.ReadKey(true);

                Console.Clear();

                switch (keyPressed.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:  // Products

                        Console.WriteLine("1. List products");
                        Console.WriteLine("2. Add products");
                        keyPressed = Console.ReadKey(true);

                        Console.Clear();

                        switch (keyPressed.Key)
                        {
                            case ConsoleKey.D1:
                            case ConsoleKey.NumPad1:    // list products

                                ListProducts();
                                break;

                            case ConsoleKey.D2:
                            case ConsoleKey.NumPad2: // add products

                                AddProducts();
                                break;
                        }

                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2: // Categories

                        Console.WriteLine("1. List categories");
                        Console.WriteLine("2. Add categories");
                        Console.WriteLine("3. Add product to category");
                        keyPressed = Console.ReadKey(true);

                        Console.Clear();

                        switch (keyPressed.Key)
                        {
                            case ConsoleKey.D1:
                            case ConsoleKey.NumPad1: // list categories

                                ListCategories();

                                break;

                            case ConsoleKey.D2:
                            case ConsoleKey.NumPad2: // add categories

                                AddCategories();

                                break;

                            case ConsoleKey.D3:
                            case ConsoleKey.NumPad3: // add product to category

                                AddProductToCategory();

                                break;
                        }

                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:

                        isRunning = false;

                        break;
                }
            }

        }

        private static void AddProductToCategory()
        {
            Console.SetCursorPosition(2, 1);
            Console.WriteLine("Product ID: ");

            Console.SetCursorPosition(2, 2);
            Console.WriteLine("Category ID: ");

            Console.SetCursorPosition("Product ID: ".Length + 2, 1);
            int productID = int.Parse(Console.ReadLine());

            Console.SetCursorPosition("Category ID: ".Length + 2, 2);
            int categoryId = int.Parse(Console.ReadLine());

            Console.WriteLine("Is this correct? [Y]es  [N]o");

            ConsoleKeyInfo keyPressed = Console.ReadKey(true);

            if (keyPressed.Key == ConsoleKey.Y)
            {

            }
        }

        private static void AddCategories()
        {
            Console.SetCursorPosition(2, 1);
            Console.WriteLine("Name: ");

            Console.SetCursorPosition(2, 2);
            Console.WriteLine("Image URL: ");

            Console.SetCursorPosition("Name: ".Length + 2, 1);
            string name = Console.ReadLine();

            Console.SetCursorPosition("Image URL: ".Length + 2, 2);
            var imgUrl = new Uri(Console.ReadLine());

            Console.WriteLine("Is this correct? [Y]es  [N]o");

            ConsoleKeyInfo keyPressed = Console.ReadKey(true);

            if(keyPressed.Key == ConsoleKey.Y)
            {
                var category = new Category(name, imgUrl);
                var serializedCategory = JsonConvert.SerializeObject(category);

                var data = new StringContent(
                serializedCategory,
                Encoding.UTF8,
                "application/json");

                var response = httpClient.PostAsync("category", data).Result;

                Console.Clear();

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Category added");
                }
                else
                {
                    Console.WriteLine("Failed!");
                }

                Thread.Sleep(2000);

            }
            else if (keyPressed.Key == ConsoleKey.N)
            {
                Console.Clear();
                AddCategories();
            }

        }

        private static void AddProducts()
        {
            Console.SetCursorPosition(2, 1);
            Console.WriteLine("Name: ");

            Console.SetCursorPosition(2, 2);
            Console.WriteLine("Description: ");

            Console.SetCursorPosition(2, 3);
            Console.WriteLine("Art. Number: ");

            Console.SetCursorPosition(2, 4);
            Console.WriteLine("Price: ");

            Console.SetCursorPosition(2, 5);
            Console.WriteLine("Image URL: ");

            Console.SetCursorPosition("Name: ".Length + 2, 1);
            string name = Console.ReadLine();

            Console.SetCursorPosition("Description: ".Length + 2, 2);
            string description = Console.ReadLine();

            Console.SetCursorPosition("Art. Number: ".Length + 2, 3);
            string artNumber = Console.ReadLine();

            Console.SetCursorPosition("Price: ".Length + 2, 4);
            decimal price = decimal.Parse(Console.ReadLine());

            Console.SetCursorPosition("Image URL: ".Length + 2, 5);
            var imgUrl = new Uri(Console.ReadLine());

            Console.WriteLine("Is this correct? [Y]es  [N]o");

            ConsoleKeyInfo keyPressed = Console.ReadKey(true);

            if (keyPressed.Key == ConsoleKey.Y)
            {
                var product = new Product(name, description, artNumber, price, imgUrl);
                var serializedProduct = JsonConvert.SerializeObject(product);

                var data = new StringContent(
               serializedProduct,
               Encoding.UTF8,
               "application/json");

                var response = httpClient.PostAsync("product", data).Result;

                Console.Clear();

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Product added");
                }
                else
                {
                    Console.WriteLine("Failed!");
                }

                Thread.Sleep(2000);
            }
            else if (keyPressed.Key == ConsoleKey.N){
                Console.Clear();
                AddProducts();
            }
        }

        private static void ListCategories()
        {
            // make a HTTP Get /category request
            var response = httpClient.GetAsync("category")
                .GetAwaiter()
                .GetResult();

            //skapar en tom samling
            var category = Enumerable.Empty<Category>();

            if (response.IsSuccessStatusCode)
            {
                var stringContent = response.Content.ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult(); // dessa är som promises i javascript
                                  // få den här koden att bete sig synkront

                // deserializerar till en samling av produkter
                category = JsonConvert.DeserializeObject<IEnumerable<Category>>(stringContent);
            }

            Console.WriteLine("Id" + "|".PadLeft(5, ' ') + "Name".PadLeft(10, ' '));
            Console.WriteLine(sb); //stringbuilder

            foreach (var categories in category)
            {
                int categoryId = categories.Id;
                int x = categoryId >= 10 ? 5 : 6;

                string categoryName = categories.Name.PadLeft(5, ' ');

                Console.Write(categoryId + "|".PadLeft(x, ' '));
                Console.WriteLine(categoryName);
            }

            Console.WriteLine("<Press [v] to view a category>");

            ConsoleKeyInfo keyPressed = Console.ReadKey(true);

            if (keyPressed.Key == ConsoleKey.V)
            {
                Console.Write("View (ID): ");
                var chosenId = Console.ReadLine();
                Console.Clear();

                response = httpClient.GetAsync($"/api/category/{chosenId}").Result;

                Category desCategory = JsonConvert.DeserializeObject<Category>(response.Content.ReadAsStringAsync().Result);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Category not found");
                    Thread.Sleep(2000);
                    return;
                }
                else
                {
                    Console.WriteLine($"ID: {desCategory.Id}\n");
                    Console.WriteLine($"Name: {desCategory.Name}\n");
                    Console.WriteLine($"ImageUrl: {desCategory.Image}");

                    Console.ReadLine();
                }
            }
        }

        private static void ListProducts()
        {
            // make a HTTP Get /product request
            var response = httpClient.GetAsync("product")
                .GetAwaiter()
                .GetResult(); //vi skriver så för vi har async

            //skapar en tom samling
            var product = Enumerable.Empty<Product>();

            if (response.IsSuccessStatusCode)
            {
                var stringContent = response.Content.ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult(); // dessa är som promises i javascript
                                  // få den här koden att bete sig synkront

                // deserializerar till en samling av produkter
                product = JsonConvert.DeserializeObject<IEnumerable<Product>>(stringContent);
            }

            Console.WriteLine("Id" + "|".PadLeft(5, ' ') + "Name".PadLeft(10, ' '));
            Console.WriteLine(sb); //stringbuilder

            foreach (var products in product)
            {
                int productId = products.Id;
                int x = productId >= 10 ? 5 : 6;
                string productName = products.Name.PadLeft(5, ' ');

                Console.Write(productId + "|".PadLeft(x, ' '));
                Console.WriteLine(productName);
            }

            Console.WriteLine("<Press [v] to view a product>");

            ConsoleKeyInfo keyPressed = Console.ReadKey(true);

            if (keyPressed.Key == ConsoleKey.V)
            {

                Console.Write("View (ID): ");
                var chosenId = Console.ReadLine();
                Console.Clear();

                response = httpClient.GetAsync($"/api/product/{chosenId}").Result;

                Product desProduct = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Product not found");
                    Thread.Sleep(2000);

                }
                else
                {
                    Console.WriteLine($"ID: {desProduct.Id}");
                    Console.WriteLine($"Name: {desProduct.Name}");
                    Console.WriteLine($"Description:\n{desProduct.Description}");
                    Console.WriteLine($"Price: {desProduct.Price}");
                    Console.WriteLine("Categories:\n");
                    foreach (var cat in desProduct.ProductCategories)
                    {
                        Console.WriteLine($"\t{cat.Category.Name}");
                    }


                    Console.ReadLine();
                }
            }

        }
    }
}
