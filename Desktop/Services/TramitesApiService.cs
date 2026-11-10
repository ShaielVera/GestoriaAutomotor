using DotNetEnv;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;


namespace Desktop.Services
{
    public class TramitesApiService
    {
        HttpClient httpClient;
        JsonSerializerOptions options;
        public TramitesApiService()
        {
            httpClient = SettingHttpClient();
            options = SettingJsonSerializer();
        }
        public async Task<List<Tramite>?> GetAllAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var tramites = JsonSerializer.Deserialize<List<Tramite>>(json, options);
                    return tramites;
                }
                else
                {
                    MessageBox.Show("Error al obtener los trámites: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los trámites desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<List<Tramite>?> GetAllWithFilterAsync(string filter)
        {
            try
            {
                var response = await httpClient.GetAsync($"?filtro={filter}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var tramites = JsonSerializer.Deserialize<List<Tramite>>(json, options);
                    return tramites;
                }
                else
                {
                    MessageBox.Show("Error al obtener los trámites: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los trámites desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<bool> AddTramiteAsync(Tramite tramite)
        {
            try
            {
                var json = JsonSerializer.Serialize(tramite, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("", content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error al crear el trámite: " + response.ReasonPhrase);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el trámite desde la Api: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteTramiteAsync(int id)
        {
            try
            {
                var response = await httpClient.DeleteAsync(id.ToString());
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Error al eliminar el trámite: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el trámite desde la Api: " + ex.Message);
                return false;
            }
        }

        public async Task<List<Tramite>?> GetDeletedsAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("deleteds");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var tramites = JsonSerializer.Deserialize<List<Tramite>>(json, options);
                    return tramites;
                }
                else
                {
                    MessageBox.Show("Error al obtener los trámites: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los trámites desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<bool> RestoreTramiteAsync(int id)
        {
            try
            {
                var response = await httpClient.PutAsync($"restore/{id}", null);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Error al restaurar el trámite: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al restaurar el trámite desde la Api: " + ex.Message);
                return false;
            }
        }


        public async Task<bool> UpdateTramiteAsync(Tramite tramite)
        {
            try
            {
                var json = JsonSerializer.Serialize(tramite, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string idtramite = tramite.Id.ToString();
                var response = await httpClient.PutAsync(idtramite, content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error al actualizar el trámite: " + response.ReasonPhrase);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el trámite desde la Api: " + ex.Message);
                return false;
            }
        }

        private HttpClient SettingHttpClient()
        {
            Env.Load("../../../");
            var urlApi = Environment.GetEnvironmentVariable("URLAPI");
            //instanciamos el httpClient y lo configuramos para poder utilizarlo en cada uno de los métodos
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(urlApi + "Tramites/");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            return httpClient;
        }

        private JsonSerializerOptions SettingJsonSerializer()
        {
            return new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true,
            };
        }

    }
}