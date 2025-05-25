using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace Raktárkészlet_nyilvántartó_rendszer
{
    internal class ServerConnection
    {
        public string BaseUrl = "http://localhost:3000";
        HttpClient client = new HttpClient();
        public ServerConnection() 
        {

        }
        public async Task<bool> CreateProduct(string name, string type, int price)
        {
            string url = BaseUrl + "/productcreate";

            try
            {
                var jsonInfo = new
                {
                    createProductName = name,
                    createProductType = type,
                    createProductPrice = price
                };

                string jsonString = JsonConvert.SerializeObject(jsonInfo);
                HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();
                JsonData responseData = JsonConvert.DeserializeObject<JsonData>(result);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Hiba a termék létrehozásakor: " + e.Message);
                return false;
            }
        }

        public async Task<List<JsonData>> GetAllProducts()
        {
            string url = BaseUrl + "/products";
            List<JsonData> products = new List<JsonData>();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<List<JsonData>>(result);
            }
            catch (Exception e)
            {
                Console.WriteLine("Hiba a termékek lekérdezésekor: " + e.Message);
            }

            return products;
        }

        public async Task<JsonData> GetProductById(string id)
        {
            string url = BaseUrl + "/productsid";

            try
            {
                var jsonInfo = new { id = id };
                string jsonString = JsonConvert.SerializeObject(jsonInfo);
                HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();
                JsonData product = JsonConvert.DeserializeObject<JsonData>(result);

                return product;
            }
            catch (Exception e)
            {
                Console.WriteLine("Hiba a termék lekérdezésekor: " + e.Message);
                return null;
            }
        }

        //-----------------------------------------------------

        public async Task<bool> EditProduct(int id, string name, string type, int price)
        {
            string url = BaseUrl + "/update";

            try
            {
                var jsonInfo = new
                {
                    id = id,
                    name = name,
                    type = type,
                    price = price
                };
                string jsonString = JsonConvert.SerializeObject(jsonInfo);
                HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(url, content);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Hiba a termék módosításakor: " + e.Message);
                return false;
            }
        }

        public async Task<bool> DeleteProduct(int id)
        {
            string url = BaseUrl + "/deletProduct";

            try
            {
                var jsonInfo = new { id = id };
                string jsonString = JsonConvert.SerializeObject(jsonInfo);
                var request = new HttpRequestMessage(HttpMethod.Delete, url)
                {
                    Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
                };

                HttpResponseMessage response = await client.SendAsync(request);
                Console.WriteLine("Sikeres törlés");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Hiba a termék törlésekor: " + e.Message);
                return false;
            }
        }

        public async Task<JsonData> GetProductByName(string name)
        {
            string url = BaseUrl + "/productsearchname";

            try
            {
                var jsonInfo = new { name = name };
                string jsonString = JsonConvert.SerializeObject(jsonInfo);
                HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();
                JsonData product = JsonConvert.DeserializeObject<JsonData>(result);

                return product;
            }
            catch (Exception e)
            {
                Console.WriteLine("Hiba a termék lekérdezésekor: " + e.Message);
                return null;
            }
        }

        //-----------------------------------------------------

        public async Task<bool> CreateWarehouse(string name, string location, string capacity, string managerName, string notes)
        {
            string url = BaseUrl + "/warehouses";

            try
            {
                var warehouseInfo = new
                {
                    name = name,
                    location = location,
                    capacity = capacity,
                    manager_name = managerName,
                    notes = notes
                };

                string jsonString = JsonConvert.SerializeObject(warehouseInfo);
                HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Raktár sikeresen létrehozva: " + result);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Hiba a raktár létrehozásakor: " + e.Message);
                return false;
            }
        }

        public async Task<List<Warehouse>> GetAllWarehouses()
        {
            string url = BaseUrl + "/warehouses";

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();
                var warehouses = JsonConvert.DeserializeObject<List<Warehouse>>(result);

                return warehouses;
            }
            catch (Exception e)
            {
                Console.WriteLine("Hiba a raktárak lekérdezésekor: " + e.Message);
                return new List<Warehouse>();
            }
        }

        public async Task<Warehouse> GetWarehouseById(int id)
        {
            string url = $"{BaseUrl}/warehouses/{id}";

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();
                var warehouse = JsonConvert.DeserializeObject<Warehouse>(result);

                return warehouse;
            }
            catch (Exception e)
            {
                Console.WriteLine("Hiba a raktár lekérdezésekor: " + e.Message);
                return null;
            }
        }

    }

    public class Warehouse
    {
        public int id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public int capacity { get; set; }
        public string manager_name { get; set; }
        public string notes { get; set; }
    }
}

