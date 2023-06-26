using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace fishing_store_app.Model
{
    public class Server
    {
        public static async Task<List<Product>> GetProducts(HttpClient httpClient)
        {
            var data = await httpClient.GetFromJsonAsync<List<Product>>("products", default);

            if (data is List<Product> products)
            {
                return products;
            }

            return null;
        }
    }
}
