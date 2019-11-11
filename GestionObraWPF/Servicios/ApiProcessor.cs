using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GestionObraWPF.Servicios
{
    public static class ApiProcessor
    {
        public static async Task<T> GetApi<T>(string urlString = "")
        {
            try
            {
                string url = "";
                if (urlString == "")
                {
                    url = Constantes.Constantes.CONEXION;
                }
                else
                {
                    url = Constantes.Constantes.CONEXION + urlString;
                }
                using (HttpResponseMessage response = await WebServices.HttpClient.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResult = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<T>(jsonResult);
                        return result;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Hubo un error con la consulta al servidor");
                return T;
            }

        }

        public static async Task<string> PostApi<T>(T item, string urlString = "")
        {
            try
            {
                string url = "";
                if (urlString == "")
                {
                    url = Constantes.Constantes.CONEXION;
                }
                else
                {
                    url = Constantes.Constantes.CONEXION + urlString;
                }
                var uri = new Uri(url);

                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (HttpResponseMessage request = await WebServices.HttpClient.PostAsync(uri, content))
                {
                    if (request.IsSuccessStatusCode)
                    {
                        var result = await request.Content.ReadAsStringAsync();
                        return result;
                    }
                    else
                    {
                        throw new Exception(request.ReasonPhrase);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Hubo un error con la consulta al servidor");
                return "";
            }

        }
        public static async Task<string> PutApi<T>(T item, string urlString = "")
        {
            try
            {
                string url = "";
                if (urlString == "")
                {
                    url = Constantes.Constantes.CONEXION;
                }
                else
                {
                    url = Constantes.Constantes.CONEXION + urlString;
                }
                var uri = new Uri(url);

                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (HttpResponseMessage request = await WebServices.HttpClient.PutAsync(uri, content))
                {
                    if (request.IsSuccessStatusCode)
                    {
                        var result = await request.Content.ReadAsStringAsync();
                        return result;
                    }
                    else
                    {
                        throw new Exception(request.ReasonPhrase);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Hubo un error con la consulta al servidor");
                return "";
            }

        }
        public static async Task<string> DeleteApi(string urlString = "")
        {
            try
            {
                string url = "";
                if (urlString == "")
                {
                    url = Constantes.Constantes.CONEXION;
                }
                else
                {
                    url = Constantes.Constantes.CONEXION + urlString;
                }
                var uri = new Uri(url);

                using (HttpResponseMessage request = await WebServices.HttpClient.DeleteAsync(uri))
                {
                    if (request.IsSuccessStatusCode)
                    {
                        var result = await request.Content.ReadAsStringAsync();
                        return result;
                    }
                    else
                    {
                        throw new Exception(request.ReasonPhrase);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Hubo un error con la consulta al servidor");
                return "";
            }
        }
    }
}
