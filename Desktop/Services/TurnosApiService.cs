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
    public class TurnosApiService
    {
        HttpClient httpClient;
        JsonSerializerOptions options;
        public TurnosApiService()
        {
            httpClient = SettingHttpClient();
            options = SettingJsonSerializer();
        }
        public async Task<List<Turno>?> GetAllAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var turnos = JsonSerializer.Deserialize<List<Turno>>(json, options);
                    return turnos;
                }
                else
                {
                    MessageBox.Show("Error al obtener los turnos: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los turnos desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<List<Turno>?> GetAllWithFilterAsync(string filter)
        {
            try
            {
                var response = await httpClient.GetAsync($"?filtro={filter}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var turnos = JsonSerializer.Deserialize<List<Turno>>(json, options);
                    return turnos;
                }
                else
                {
                    MessageBox.Show("Error al obtener los turnos: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los turnos desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<bool> AddTurnoAsync(Turno turno)
        {
            try
            {
                var json = JsonSerializer.Serialize(turno, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("", content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error al crear el turno: " + response.ReasonPhrase);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el turno desde la Api: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteTurnoAsync(int id)
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
                    MessageBox.Show("Error al eliminar el turno: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el turno desde la Api: " + ex.Message);
                return false;
            }
        }

        public async Task<List<Turno>?> GetDeletedsAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("deleteds");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var turnos = JsonSerializer.Deserialize<List<Turno>>(json, options);
                    return turnos;
                }
                else
                {
                    MessageBox.Show("Error al obtener los turnos: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los turnos desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<bool> RestoreTurnoAsync(int id)
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
                    MessageBox.Show("Error al restaurar el turno: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al restaurar el turno desde la Api: " + ex.Message);
                return false;
            }
        }


        public async Task<bool> UpdateTurnoAsync(Turno turno)
        {
            try
            {
                var json = JsonSerializer.Serialize(turno, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string idturno = turno.Id.ToString();
                var response = await httpClient.PutAsync(idturno, content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error al actualizar el turno: " + response.ReasonPhrase);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el turno desde la Api: " + ex.Message);
                return false;
            }
        }

        private HttpClient SettingHttpClient()
        {
            Env.Load("../../../");
            var urlApi = Environment.GetEnvironmentVariable("URLAPI");
            //instanciamos el httpClient y lo configuramos para poder utilizarlo en cada uno de los métodos
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(urlApi + "Turnos/");
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