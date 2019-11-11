namespace GestionObraWPF.Servicios
{
    using Newtonsoft.Json;
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public static class WebServices
    {
        public static HttpClient HttpClient { get; set; }

        public static void InitializeCliente()
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            
        }
        //public async Task<T> GetData<T>(string url)
        //{
        //    _client.BaseAddress = new Uri(string.Format(url));
        //    var respose = await _client.GetAsync(_client.BaseAddress);
        //    respose.EnsureSuccessStatusCode();
        //    var jsonResult = await respose.Content.ReadAsStringAsync();
        //    var result = JsonConvert.DeserializeObject<T>(jsonResult);
        //    return result;
        //}
        //public async Task<bool> SaveTodoItemAsync<T>(T item, string url)
        //{
        //    var uri = new Uri(url);
        //    var json = JsonConvert.SerializeObject(item);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    var request = await _client.PostAsync(uri, content);
        //    request.EnsureSuccessStatusCode();

        //    var jsonResult = await request.Content.ReadAsStringAsync();
        //    var result = await request.Content.ReadAsStringAsync();
        //    return bool.Parse(result);

        //}

        //public async Task SaveTodoItemAsync<T>(T item, string url, bool isNewItem = false)
        //{
        //    var uri = new Uri(url);


        //    var json = JsonConvert.SerializeObject(item);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    HttpResponseMessage response = null;
        //    if (isNewItem)
        //    {
        //        response = await _client.PostAsync(uri, content);
        //        var result = await response.Content.ReadAsStringAsync();
        //    }


        //    if (response.IsSuccessStatusCode)
        //    {
        //        Debug.WriteLine(@"\tSe guardo exitosamente.");
        //    }
        //}
    }

}
