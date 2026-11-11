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
    public class UsuariosTramitesApiService
    {
        HttpClient httpClient;
        JsonSerializerOptions options;
        public UsuariosTramitesApiService()
        {
            httpClient = SettingHttpClient();
            options = SettingJsonSerializer();
        }
        public async Task<List<UsuarioTramite>?> GetAllAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var usuariosTramites = JsonSerializer.Deserialize<List<UsuarioTramite>>(json, options);
                    return usuariosTramites;
                }
                else
                {
                    MessageBox.Show("Error al obtener los usuarios de los trámites: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los usuarios de los trámites desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<List<UsuarioTramite>?> GetAllWithFilterAsync(string filter)
        {
            try
            {
                var response = await httpClient.GetAsync($"?filtro={filter}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var usuariosTramites = JsonSerializer.Deserialize<List<UsuarioTramite>>(json, options);
                    return usuariosTramites;
                }
                else
                {
                    MessageBox.Show("Error al obtener los usuarios de los trámites: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los usuarios de los trámites desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<bool> AddUsuarioTramiteAsync(UsuarioTramite usuarioTramite)
        {
            try
            {
                var json = JsonSerializer.Serialize(usuarioTramite, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("", content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error al crear el usuario del trámite: " + response.ReasonPhrase);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el usuario del trámite desde la Api: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteUsuarioTramiteAsync(int id)
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
                    MessageBox.Show("Error al eliminar el usuario del trámite: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el usuario del trámite desde la Api: " + ex.Message);
                return false;
            }
        }

        public async Task<List<UsuarioTramite>?> GetDeletedsAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("deleteds");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var usuariosTramites = JsonSerializer.Deserialize<List<UsuarioTramite>>(json, options);
                    return usuariosTramites;
                }
                else
                {
                    MessageBox.Show("Error al obtener los usuarios de los trámites: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los usuarios de los trámites desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<bool> RestoreUsuarioTramiteAsync(int id)
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
                    MessageBox.Show("Error al restaurar el usuario del trámite: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al restaurar el usuario del trámite desde la Api: " + ex.Message);
                return false;
            }
        }


        public async Task<bool> UpdateUsuarioTramiteAsync(UsuarioTramite usuarioTramite)
        {
            try
            {
                var json = JsonSerializer.Serialize(usuarioTramite, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string idusuarioTramite = usuarioTramite.Id.ToString();
                var response = await httpClient.PutAsync(idusuarioTramite, content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error al actualizar el usuario del trámite: " + response.ReasonPhrase);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el usuario del trámite desde la Api: " + ex.Message);
                return false;
            }
        }

        private HttpClient SettingHttpClient()
        {
            Env.Load("../../../");
            var urlApi = Environment.GetEnvironmentVariable("URLAPI");
            //instanciamos el httpClient y lo configuramos para poder utilizarlo en cada uno de los métodos
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(urlApi + "UsuariosTramites/");
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