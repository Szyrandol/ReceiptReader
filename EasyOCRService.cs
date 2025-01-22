using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace EasyOCRFlaskAPITest
{
    internal class EasyOCRService
    {
        private readonly HttpClient _httpClient;

        public EasyOCRService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<List<Item>> GetItemListFromImage(string imagePath)
        {
            using var formData = new MultipartFormDataContent();
            formData.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(imagePath)), "image", "image.jpg");

            var response = await _httpClient.PostAsync("http://localhost:5000/ocr", formData);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return DecodeJsonToItemList(result);
                //string result = await response.Content.ReadFromJsonAsync<string>();
                //return result;
                //return await response.Content.ReadFromJsonAsync();
            }
            else
            {
                throw new Exception($"Error: {response.ReasonPhrase}");
            }
        }
        private List<Item> DecodeJsonToItemList (string jsonText)
        {
            Console.WriteLine("jsonText");
            List<Item> result = new();
            using (JsonDocument doc = JsonDocument.Parse(jsonText))
            {
                JsonElement root = doc.RootElement;
                foreach (JsonElement jsonElement in root.EnumerateArray())
                {
                    string jsonValue = jsonElement.GetProperty("newline").GetString();
                    Item item = new(jsonValue);
                    result.Add(item);
                }
            }
            result[result.Count - 1].Value *= 10;
            return result;
        }
        /* 
        public async Task<string> GetStringFromImage(string imagePath)
        {
            using var formData = new MultipartFormDataContent();
            formData.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(imagePath)), "image", "image.jpg");

            var response = await _httpClient.PostAsync("http://localhost:5000/ocr", formData);
            if (response.IsSuccessStatusCode)
            {
                var result =  await response.Content.ReadAsStringAsync();
                return DecodeJsonToString(result);
                //string result = await response.Content.ReadFromJsonAsync<string>();
                //return result;
                //return await response.Content.ReadFromJsonAsync();
            }
            else
            {
                throw new Exception($"Error: {response.ReasonPhrase}");
            }
        }
        private string DecodeJsonToString(string jsonText)
        {
            Console.WriteLine("DecodeJsonToString");
            StringBuilder sb = new();
            using (JsonDocument doc = JsonDocument.Parse(jsonText))
            {
                JsonElement root = doc.RootElement;
                foreach (JsonElement item in root.EnumerateArray())
                {
                    sb.Append(item.GetProperty("newline").GetString());
                    sb.Append('\n');
                }
            }
            return sb.ToString();
        }
         */
    }
}
