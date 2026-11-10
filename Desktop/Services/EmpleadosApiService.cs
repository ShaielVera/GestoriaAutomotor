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
    public class EmpleadosApiService
    {
        HttpClient httpClient;
        JsonSerializerOptions options;
        public EmpleadosApiService()
        {
            httpClient = SettingHttpClient();
            options = SettingJsonSerializer();
        }
        public async Task<List<Empleado>?> GetAllAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var empleados = JsonSerializer.Deserialize<List<Empleado>>(json, options);
                    return empleados;
                }
                else
                {
                    MessageBox.Show("Error al obtener los empleados: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los empleados desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<List<Empleado>?> GetAllWithFilterAsync(string filter)
        {
            try
            {
                var response = await httpClient.GetAsync($"?filtro={filter}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var empleados = JsonSerializer.Deserialize<List<Empleado>>(json, options);
                    return empleados;
                }
                else
                {
                    MessageBox.Show("Error al obtener los empleados: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los empleados desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<bool> AddEmpleadoAsync(Empleado empleado)
        {
            try
            {
                var json = JsonSerializer.Serialize(empleado, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("", content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error al crear el empleado: " + response.ReasonPhrase);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el empleado desde la Api: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteEmpleadoAsync(int id)
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
                    MessageBox.Show("Error al eliminar el empleado: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el empleado desde la Api: " + ex.Message);
                return false;
            }
        }

        public async Task<List<Empleado>?> GetDeletedsAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("deleteds");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var empleados = JsonSerializer.Deserialize<List<Empleado>>(json, options);
                    return empleados;
                }
                else
                {
                    MessageBox.Show("Error al obtener los empleados: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los empleados desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<bool> RestoreEmpleadoAsync(int id)
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
                    MessageBox.Show("Error al restaurar el empleado: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al restaurar el empleado desde la Api: " + ex.Message);
                return false;
            }
        }


        public async Task<bool> UpdateEmpleadoAsync(Empleado empleado)
        {
            try
            {
                var json = JsonSerializer.Serialize(empleado, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string idempleado = empleado.Id.ToString();
                var response = await httpClient.PutAsync(idempleado, content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error al actualizar el empleado: " + response.ReasonPhrase);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el empleado desde la Api: " + ex.Message);
                return false;
            }
        }

        private HttpClient SettingHttpClient()
        {
            Env.Load("../../../");
            var urlApi = Environment.GetEnvironmentVariable("URLAPI");
            //instanciamos el httpClient y lo configuramos para poder utilizarlo en cada uno de los métodos
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(urlApi + "Empleados/");
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