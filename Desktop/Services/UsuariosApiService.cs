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
    public class UsuariosApiService
    {
        HttpClient httpClient;
        JsonSerializerOptions options;
        public UsuariosApiService()
        {
            httpClient = SettingHttpClient();
            options = SettingJsonSerializer();
        }
        public async Task<List<Usuario>?> GetAllAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var usuarios = JsonSerializer.Deserialize<List<Usuario>>(json, options);
                    return usuarios;
                }
                else
                {
                    MessageBox.Show("Error al obtener los usuarios: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los usuarios desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<List<Usuario>?> GetAllWithFilterAsync(string filter)
        {
            try
            {
                var response = await httpClient.GetAsync($"?filtro={filter}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var usuarios = JsonSerializer.Deserialize<List<Usuario>>(json, options);
                    return usuarios;
                }
                else
                {
                    MessageBox.Show("Error al obtener los usuarios: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los usuarios desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<bool> AddUsuarioAsync(Usuario usuario)
        {
            try
            {
                var json = JsonSerializer.Serialize(usuario, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("", content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error al crear el usuario: " + response.ReasonPhrase);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el usuario desde la Api: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteUsuarioAsync(int id)
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
                    MessageBox.Show("Error al eliminar el usuario: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el usuario desde la Api: " + ex.Message);
                return false;
            }
        }

        public async Task<List<Usuario>?> GetDeletedsAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("deleteds");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var usuarios = JsonSerializer.Deserialize<List<Usuario>>(json, options);
                    return usuarios;
                }
                else
                {
                    MessageBox.Show("Error al obtener los usuarios: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los usuarios desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<bool> RestoreUsuarioAsync(int id)
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
                    MessageBox.Show("Error al restaurar el usuario: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al restaurar el usuario desde la Api: " + ex.Message);
                return false;
            }
        }


        public async Task<bool> UpdateUsuarioAsync(Usuario usuario)
        {
            try
            {
                var json = JsonSerializer.Serialize(usuario, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string idusuario = usuario.Id.ToString();
                var response = await httpClient.PutAsync(idusuario, content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error al actualizar el usuario: " + response.ReasonPhrase);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el usuario desde la Api: " + ex.Message);
                return false;
            }
        }

        private HttpClient SettingHttpClient()
        {
            Env.Load("../../../");
            var urlApi = Environment.GetEnvironmentVariable("URLAPI");
            //instanciamos el httpClient y lo configuramos para poder utilizarlo en cada uno de los métodos
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(urlApi + "Usuarios/");
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