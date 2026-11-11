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
    public class ClientesTramitesApiService
    {
        HttpClient httpClient;
        JsonSerializerOptions options;
        public ClientesTramitesApiService()
        {
            httpClient = SettingHttpClient();
            options = SettingJsonSerializer();
        }
        public async Task<List<ClienteTramite>?> GetAllAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var clientesTramites = JsonSerializer.Deserialize<List<ClienteTramite>>(json, options);
                    return clientesTramites;
                }
                else
                {
                    MessageBox.Show("Error al obtener los clientes de los trámites: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los clientes de los trámites desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<List<ClienteTramite>?> GetAllWithFilterAsync(string filter)
        {
            try
            {
                var response = await httpClient.GetAsync($"?filtro={filter}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var clientesTramites = JsonSerializer.Deserialize<List<ClienteTramite>>(json, options);
                    return clientesTramites;
                }
                else
                {
                    MessageBox.Show("Error al obtener los clientes de los trámites: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los clientes de los trámites desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<bool> AddClienteTramiteAsync(ClienteTramite clienteTramite)
        {
            try
            {
                var json = JsonSerializer.Serialize(clienteTramite, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("", content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error al crear el cliente del trámite: " + response.ReasonPhrase);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el cliente del trámite desde la Api: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteClienteTramiteAsync(int id)
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
                    MessageBox.Show("Error al eliminar el cliente del trámite: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el cliente del trámite desde la Api: " + ex.Message);
                return false;
            }
        }

        public async Task<List<ClienteTramite>?> GetDeletedsAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("deleteds");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var clientesTramites = JsonSerializer.Deserialize<List<ClienteTramite>>(json, options);
                    return clientesTramites;
                }
                else
                {
                    MessageBox.Show("Error al obtener los clientes de los trámites: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los clientes de los trámites desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<bool> RestoreClienteTramiteAsync(int id)
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
                    MessageBox.Show("Error al restaurar el cliente del trámite: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al restaurar el cliente del trámite desde la Api: " + ex.Message);
                return false;
            }
        }


        public async Task<bool> UpdateClienteTramiteAsync(ClienteTramite clienteTramite)
        {
            try
            {
                var json = JsonSerializer.Serialize(clienteTramite, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string idclienteTramite = clienteTramite.Id.ToString();
                var response = await httpClient.PutAsync(idclienteTramite, content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error al actualizar el cliente del trámite: " + response.ReasonPhrase);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el cliente del trámite desde la Api: " + ex.Message);
                return false;
            }
        }

        private HttpClient SettingHttpClient()
        {
            Env.Load("../../../");
            var urlApi = Environment.GetEnvironmentVariable("URLAPI");
            //instanciamos el httpClient y lo configuramos para poder utilizarlo en cada uno de los métodos
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(urlApi + "ClientesTramites/");
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