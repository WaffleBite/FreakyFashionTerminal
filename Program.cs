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
            int productId = int.Parse(Console.ReadLine());

            Console.SetCursorPosition("Category ID: ".Length + 2, 2);
            int categoryId = int.Parse(Console.ReadLine());

            Console.WriteLine("\nIs this correct? [Y]es  [N]o");

            ConsoleKeyInfo keyPressed = Console.ReadKey(true);

            if (keyPressed.Key == ConsoleKey.Y)
            {             
                var response = httpClient.PostAsync($"category/{categoryId}/product/{productId}", new StringContent("")).Result;
                
                if (response.IsSuccessStatusCode) 
                {
                    Console.WriteLine("It did work!");
                }
                else
                {
                    Console.WriteLine("Did not work");
                }
                
                Thread.Sleep(2000);
            }
            else if (keyPressed.Key == ConsoleKey.N)
            {
                Console.Clear();
                AddProductToCategory();
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

            Console.WriteLine("\nIs this correct? [Y]es  [N]o");

            ConsoleKeyInfo keyPressed = Console.ReadKey(true);

            if (keyPressed.Key == ConsoleKey.Y)
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

            Console.SetCursorPosition(2, 5);
            Console.WriteLine("Art. Number: ");

            Console.SetCursorPosition(2, 6);
            Console.WriteLine("Price: ");

            Console.SetCursorPosition(2, 7);
            Console.WriteLine("Image URL: ");

            Console.SetCursorPosition("Name: ".Length + 2, 1);
            string name = Console.ReadLine();

            Console.SetCursorPosition("Description: ".Length + 2, 2);
            string description = Console.ReadLine();

            Console.SetCursorPosition("Art. Number: ".Length + 2, 5);
            string artNumber = Console.ReadLine();

            Console.SetCursorPosition("Price: ".Length + 2, 6);
            decimal price = decimal.Parse(Console.ReadLine());

            Console.SetCursorPosition("Image URL: ".Length + 2, 7);
            var imgUrl = new Uri(Console.ReadLine());

            Console.WriteLine("\nIs this correct? [Y]es  [N]o");

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
            else if (keyPressed.Key == ConsoleKey.N)
            {
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
            Console.WriteLine("<Press [e] to edit a product>");
            Console.WriteLine("<Press [d] to edit a product>");

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

            if (keyPressed.Key == ConsoleKey.E)
            {
                Console.Write("Edit (ID): ");
                var chosenId = Console.ReadLine();
                Console.Clear();

                response = httpClient.GetAsync($"/api/category/{chosenId}").Result;

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Category not found");
                    Thread.Sleep(2000);
                }
                var json = response.Content.ReadAsStringAsync().Result;

                var desCategory = JsonConvert.DeserializeObject<Category>(json);

                Console.WriteLine($"ID: {desCategory.Id}");
                Console.WriteLine($"Name: {desCategory.Name}");
                Console.WriteLine($"Image URl: {desCategory.Image}");

                Console.WriteLine(sb);

                Console.WriteLine($"ID: {desCategory.Id}");

                Console.SetCursorPosition(0, 4);
                Console.WriteLine("Name: ");

                Console.SetCursorPosition(0, 5);
                Console.WriteLine("Image URL: ");

                Console.SetCursorPosition("Name: ".Length + 0, 4);
                string name = Console.ReadLine();

                Console.SetCursorPosition("Image URL: ".Length + 0, 5);
                var imageUrl = new Uri(Console.ReadLine());

                Console.WriteLine("\nIs this correct? [Y]es  [N]o");
                keyPressed = Console.ReadKey(true);

                if (keyPressed.Key == ConsoleKey.Y)
                {
                    var updatedCategory = new Category(desCategory.Id, name, imageUrl);

                    var serializedUpdatedCategory = JsonConvert.SerializeObject(updatedCategory);

                    var content = new StringContent(serializedUpdatedCategory, Encoding.UTF8, "application/json");
                    response = httpClient.PutAsync($"/api/category/{desCategory.Id}", content).Result;

                    Console.Clear();

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Category updated.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to update category");
                    }

                    Thread.Sleep(2000);
                }
                else if (keyPressed.Key == ConsoleKey.N)
                {
                    Console.Clear();
                    ListCategories();
                }
            }
            
            if (keyPressed.Key == ConsoleKey.D)
            {
                Console.Write("Delete (ID): ");
                var chosenId = Console.ReadLine();
                Console.Clear();

                response = httpClient.GetAsync($"/api/category/{chosenId}").Result;

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Category not found");
                    Thread.Sleep(2000);
                }
                var json = response.Content.ReadAsStringAsync().Result;

                var desCategory = JsonConvert.DeserializeObject<Category>(json);

                Console.WriteLine($"ID: {desCategory.Id}");
                Console.WriteLine($"Name: {desCategory.Name}");
                Console.WriteLine($"Image URl: {desCategory.Image}");

                Console.WriteLine(sb);

                Console.WriteLine("\nDelete category? [Y]es  [N]o");
                keyPressed = Console.ReadKey(true);

                if (keyPressed.Key == ConsoleKey.Y)
                {
                    response = httpClient.DeleteAsync($"category/{chosenId}")
                        .GetAwaiter()
                        .GetResult();

                    Console.Clear();

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Category deleted");
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
                    ListCategories();
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

            Console.WriteLine("\n<Press [v] to view a product>");
            Console.WriteLine("<Press [e] to edit a product>");
            Console.WriteLine("<Press [d] to edit a product>");

            ConsoleKeyInfo keyPressed = Console.ReadKey(true);

            if (keyPressed.Key == ConsoleKey.V)  // view
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

            if (keyPressed.Key == ConsoleKey.E)  // edit
            {
                Console.Write("Edit (ID): ");
                var chosenId = Console.ReadLine();
                Console.Clear();

                response = httpClient.GetAsync($"/api/product/{chosenId}").Result;

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Product not found");
                    Thread.Sleep(2000);
                }
                var json = response.Content.ReadAsStringAsync().Result;

                var desProduct = JsonConvert.DeserializeObject<Product>(json);

                Console.WriteLine($"ID: {desProduct.Id}");
                Console.WriteLine($"Name: {desProduct.Name}");
                Console.WriteLine($"Description: {desProduct.Description}");
                Console.WriteLine($"Price: {desProduct.Price}");
                Console.WriteLine($"Image URl: {desProduct.ImageUrl}");

                Console.WriteLine(sb);

                Console.WriteLine($"ID: {desProduct.Id}");

                Console.SetCursorPosition(0, 7);
                Console.WriteLine("Name: ");

                Console.SetCursorPosition(0, 8);
                Console.WriteLine("Description: ");

                Console.SetCursorPosition(0, 9);
                Console.WriteLine("Price: ");

                Console.SetCursorPosition(0, 10);
                Console.WriteLine("Image URL: ");

                Console.SetCursorPosition("Name: ".Length + 0, 7);
                string name = Console.ReadLine();

                Console.SetCursorPosition("Description: ".Length + 0, 8);
                string description = Console.ReadLine();

                Console.SetCursorPosition("Price: ".Length + 0, 9);
                decimal price = decimal.Parse(Console.ReadLine());

                Console.SetCursorPosition("Image URL: ".Length + 0, 10);
                var imageUrl = new Uri(Console.ReadLine());

                Console.WriteLine("\nIs this correct? [Y]es  [N]o");
                keyPressed = Console.ReadKey(true);

                if (keyPressed.Key == ConsoleKey.Y)
                {
                    var updatedProduct = new Product(desProduct.Id, name, description, price, imageUrl);

                    var serializedUpdatedProduct = JsonConvert.SerializeObject(updatedProduct);

                    var content = new StringContent(serializedUpdatedProduct, Encoding.UTF8, "application/json");
                    response = httpClient.PutAsync($"/api/product/{desProduct.Id}", content).Result;

                    Console.Clear();

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Product updated.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to update product");
                    }

                    Thread.Sleep(2000);
                }
                else if(keyPressed.Key == ConsoleKey.N)
                {
                    Console.Clear();
                    ListProducts();
                }               
            } 

            if (keyPressed.Key == ConsoleKey.D) // delete
            {
                Console.Write("Delete (ID): ");
                var chosenId = Console.ReadLine();
                Console.Clear();

                response = httpClient.GetAsync($"/api/product/{chosenId}").Result;

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Product not found");
                    Thread.Sleep(2000);
                }
                var json = response.Content.ReadAsStringAsync().Result;

                var desProduct = JsonConvert.DeserializeObject<Product>(json);

                Console.WriteLine($"ID: {desProduct.Id}");
                Console.WriteLine($"Name: {desProduct.Name}");
                Console.WriteLine($"Description: {desProduct.Description}");
                Console.WriteLine($"Price: {desProduct.Price}");
                Console.WriteLine($"Image URl: {desProduct.ImageUrl}");

                Console.WriteLine(sb);

                Console.WriteLine("\nDelete product? [Y]es  [N]o");
                keyPressed = Console.ReadKey(true);

                if (keyPressed.Key == ConsoleKey.Y)
                {
                    // TODO: Make HTTP DELETE request to delete resource...
                    response = httpClient.DeleteAsync($"product/{chosenId}")
                        .GetAwaiter()
                        .GetResult();

                    Console.Clear();

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Product deleted");
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
                    ListProducts();
                }
            }
        }
    }
}