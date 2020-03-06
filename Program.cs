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
                        keyPressed = Console.ReadKey(true);

                        Console.Clear();

                        switch (keyPressed.Key)
                        {
                            case ConsoleKey.D1:
                            case ConsoleKey.NumPad1:    // list products

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
                                    string productName = products.Name.PadLeft( 5, ' ');

                                    Console.Write(productId + "|".PadLeft(x, ' '));
                                    Console.WriteLine(productName);
                                }

                                Console.WriteLine("<Press [v] to view a product>");

                                keyPressed = Console.ReadKey(true);

                                if(keyPressed.Key == ConsoleKey.V) {

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
                                        Console.WriteLine($"ID: {desProduct.Id}" );
                                        Console.WriteLine($"Name: {desProduct.Name}" );
                                        Console.WriteLine($"Description:\n{desProduct.Description}");
                                        Console.WriteLine($"Price: {desProduct.Price}");
                                        Console.WriteLine("Categories:\n\n");
                                        foreach (var cat in desProduct.ProductCategories)
                                        {
                                            Console.WriteLine($"\t\t{cat.Category.Name}");
                                        }
                                       

                                        Console.ReadLine();
                                    }
                                }

                                break;
                        }

                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2: // Categories

                        Console.WriteLine("1. List categories");
                        keyPressed = Console.ReadKey(true);

                        Console.Clear();

                        switch (keyPressed.Key)
                        {
                            case ConsoleKey.D1:
                            case ConsoleKey.NumPad1: // list categories

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

                                keyPressed = Console.ReadKey(true);

                                if (keyPressed.Key == ConsoleKey.V)
                                {
                                    Console.WriteLine("View (ID): ");
                                    var chosenId = Console.ReadLine();
                                    Console.Clear();

                                    response = httpClient.GetAsync($"/api/category/{chosenId}").Result;

                                    Category desCategory = JsonConvert.DeserializeObject<Category>(response.ToString());

                                    if (!response.IsSuccessStatusCode)
                                    {
                                        Console.WriteLine("Category not found");
                                        Thread.Sleep(2000);
                                        return;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"ID: {desCategory.Id}");
                                        Console.WriteLine($"Name: {desCategory.Name}");
                                        Console.WriteLine($"ImageUrl: {desCategory.Image}");
                                    }
                                }

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
    }
}
