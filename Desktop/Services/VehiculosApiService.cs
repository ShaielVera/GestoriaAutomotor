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
    public class VehiculosApiService
    {
        HttpClient httpClient;
        JsonSerializerOptions options;
        public VehiculosApiService()
        {
            httpClient = SettingHttpClient();
            options = SettingJsonSerializer();
        }
        public async Task<List<Vehiculo>?> GetAllAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var vehiculos = JsonSerializer.Deserialize<List<Vehiculo>>(json, options);
                    return vehiculos;
                }
                else
                {
                    MessageBox.Show("Error al obtener los vehículos: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los vehículos desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<List<Vehiculo>?> GetAllWithFilterAsync(string filter)
        {
            try
            {
                var response = await httpClient.GetAsync($"?filtro={filter}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var vehiculos = JsonSerializer.Deserialize<List<Vehiculo>>(json, options);
                    return vehiculos;
                }
                else
                {
                    MessageBox.Show("Error al obtener los vehículos: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los vehículos desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<bool> AddVehiculoAsync(Vehiculo vehiculo)
        {
            try
            {
                var json = JsonSerializer.Serialize(vehiculo, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("", content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error al crear el vehículo: " + response.ReasonPhrase);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el vehículo desde la Api: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteVehiculoAsync(int id)
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
                    MessageBox.Show("Error al eliminar el vehículo: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el vehículo desde la Api: " + ex.Message);
                return false;
            }
        }

        public async Task<List<Vehiculo>?> GetDeletedsAsync()
        {
            try
            {
                var response = await httpClient.GetAsync("deleteds");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var vehiculos = JsonSerializer.Deserialize<List<Vehiculo>>(json, options);
                    return vehiculos;
                }
                else
                {
                    MessageBox.Show("Error al obtener los vehículos: " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error obtener los vehículos desde la Api: " + ex.Message);
                return null;

            }
        }

        public async Task<bool> RestoreVehiculoAsync(int id)
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
                    MessageBox.Show("Error al restaurar el vehículo: " + response.ReasonPhrase);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al restaurar el vehículo desde la Api: " + ex.Message);
                return false;
            }
        }


        public async Task<bool> UpdateVehiculoAsync(Vehiculo vehiculo)
        {
            try
            {
                var json = JsonSerializer.Serialize(vehiculo, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                string idvehiculo = vehiculo.Id.ToString();
                var response = await httpClient.PutAsync(idvehiculo, content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error al actualizar el vehículo: " + response.ReasonPhrase);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el vehículo desde la Api: " + ex.Message);
                return false;
            }
        }

        private HttpClient SettingHttpClient()
        {
            Env.Load("../../../");
            var urlApi = Environment.GetEnvironmentVariable("URLAPI");
            //instanciamos el httpClient y lo configuramos para poder utilizarlo en cada uno de los métodos
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(urlApi + "Vehiculos/");
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