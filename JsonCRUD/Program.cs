using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace JsonCRUD
{
    class Program
    {
        static void Main(string[] args)
        {

            Program objProgram = new Program();

            Console.WriteLine("Choose option : 1 - Select, 2 - Insert, 3 - Update, 4 - Delete \n");
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    objProgram.GetProducts();
                    break;
                case "2":
                    objProgram.AddProduct();
                    break;
                case "3":
                    objProgram.UpdateProduct();
                    break;
                case "4":
                    objProgram.DeleteProduct();
                    break;
                default:
                    Main(null);
                    break;
            }
            Console.ReadLine();

        }


        // put the path to the json file as it is on your computer
        private string jsonFile = @"C:\Users\Milan\GitProjects\JsonCRUD\product.json";


        private void GetProducts()
        {
            var json = File.ReadAllText(jsonFile);
            try
            {
                var jObject = JObject.Parse(json);

                if (jObject != null)
                {

                    JArray proizvodiArrary = (JArray)jObject["products"];
                    if (proizvodiArrary != null)
                    {
                        foreach (var item in proizvodiArrary)
                        {
                            Console.WriteLine("Id: " + item["productId"]);
                            Console.WriteLine("Name: " + item["name"]);
                            Console.WriteLine("Description: " + item["description"]);
                            Console.WriteLine("Category: " + item["category"]);
                            Console.WriteLine("Producer: " + item["producer"]);
                            Console.WriteLine("Supplier: " + item["supplier"]);
                            Console.WriteLine("Price: " + item["price"]);
                            Console.WriteLine();
                        }

                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }






        private void AddProduct()
        {
            Console.WriteLine("Enter product Id: ");
            var productId = Console.ReadLine();
            Console.WriteLine("\nEnter product name: ");
            var productName = Console.ReadLine();
            Console.WriteLine("\nEnter product description: ");
            var productDescription = Console.ReadLine();
            Console.WriteLine("\nEnter product category: ");
            var category = Console.ReadLine();
            Console.WriteLine("\nEnter producer name: ");
            var producerName = Console.ReadLine();
            Console.WriteLine("\nEnter suppler name: ");
            var supplierName = Console.ReadLine();
            Console.WriteLine("\nEnter price: ");
            var price = Console.ReadLine();

            var newProductMember = "{ 'productId': " + productId + ", 'name': '" + productName + "', 'description': '" + productDescription + "', 'category': '"
                                   + category + "', 'producer': '" + producerName + "', 'supplier': '" + supplierName + "', 'price': " + price + "  }";

            try
            {
                var json = File.ReadAllText(jsonFile);
                var jsonObj = JObject.Parse(json);
                var productsArrary = jsonObj.GetValue("products") as JArray;
                var newProduct = JObject.Parse(newProductMember);
                productsArrary.Add(newProduct);

                jsonObj["products"] = productsArrary;
                string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(jsonFile, newJsonResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add Error: " + ex.Message);
            }
        }





        private void UpdateProduct()
        {
            string json = File.ReadAllText(jsonFile);

            try
            {
                var jObject = JObject.Parse(json);
                JArray productsArrary = (JArray)jObject["products"];
                Console.Write("Enter product Id for update: ");
                var productId = Convert.ToInt32(Console.ReadLine());

                if (productId > 0)
                {
                    int price;
                    Console.Write("Enter product name: ");
                    var productName = Convert.ToString(Console.ReadLine());
                    Console.WriteLine("\nEnter product description: ");
                    var productDescription = Console.ReadLine();
                    Console.WriteLine("\nEnter category: ");
                    var category = Console.ReadLine();
                    Console.WriteLine("\nEnter producer name: ");
                    var producerName = Console.ReadLine();
                    Console.WriteLine("\nEnter supplier name: ");
                    var supplierName = Console.ReadLine();
                    Console.WriteLine("\nEnter price: ");
                    while (!int.TryParse(Console.ReadLine(), out price))
                    {
                        Console.WriteLine("You must enter a number!");
                    }

                    foreach (var product in productsArrary.Where(obj => obj["productId"].Value<int>() == productId))
                    {
                        product["name"] = !string.IsNullOrEmpty(productName) ? productName : "";
                        product["description"] = !string.IsNullOrEmpty(productDescription) ? productDescription : "";
                        product["category"] = !string.IsNullOrEmpty(category) ? category : "";
                        product["producer"] = !string.IsNullOrEmpty(producerName) ? producerName : "";
                        product["supplier"] = !string.IsNullOrEmpty(supplierName) ? supplierName : "";
                        product["price"] = price;
                    }

                    jObject["products"] = productsArrary;
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(jsonFile, output);
                }
                else
                {
                    Console.Write("Invalid Product Id, Try Again!");
                    UpdateProduct();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update Error: " + ex.Message);
            }

        }



        private void DeleteProduct()
        {

            var json = File.ReadAllText(jsonFile);
            try
            {
                var jObject = JObject.Parse(json);
                JArray productsArrary = (JArray)jObject["products"];
                Console.Write("Enter product Id to delete product : ");
                var productId = Convert.ToInt32(Console.ReadLine());

                if (productId > 0)
                {

                    var productToDeleted = productsArrary.FirstOrDefault(obj => obj["productId"].Value<int>() == productId);
                    productsArrary.Remove(productToDeleted);

                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(jsonFile, output);
                }
                else
                {
                    Console.Write("Invalid Product Id, Try Again!");
                    UpdateProduct();
                }
            }
            catch (Exception)
            {
                throw;
            }


        }



    }
}
