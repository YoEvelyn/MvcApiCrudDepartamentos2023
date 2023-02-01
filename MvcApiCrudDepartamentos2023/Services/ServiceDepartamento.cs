using MvcApiCrudDepartamentos2023.Models;
using MvcApiCrudDepartamentos2023.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MvcApiCrudDepartamentos2023.Services
{
    public class ServiceDepartamentos
    {
        private string UrlApi;
        private MediaTypeWithQualityHeaderValue header;

        public ServiceDepartamentos(string url)
        {
            this.UrlApi = url;
            this.header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        //METODO GENERICO PARA DEVOLVER CUALQUIER CLASE CON GET
        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<List<Departamento>> GetDepartamentosAsync()
        {
            string request = "/api/departamentos";
            List<Departamento> departamentos =
                await this.CallApiAsync<List<Departamento>>(request);
            return departamentos;
        }

        public async Task<Departamento> FindDepartamentoAsync(int iddepartamento)
        {
            string request = "/api/departamentos/" + iddepartamento;
            Departamento departamento = await this.CallApiAsync<Departamento>(request);
            return departamento;
        }

        public async Task<List<Departamento>> GetDepartamentosLocalidadAsync(string localidad)
        {
            string request = "/api/departamentos/departametoslocalidad/" + localidad;
            List<Departamento> departamentos =
                await this.CallApiAsync<List<Departamento>>(request);
            return departamentos;
        }

        public async Task DeleteDepartamentoAsync(int iddepartamento)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/departamentos/" + iddepartamento;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                await client.DeleteAsync(request);
            }
        }

        public async Task InsertDepartamentoAsync(int id, string nombre, string localidad)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/departamentos";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();

                //TENEMOS QUE ENVIAR UN OBJETO DEPARTAMENTO, CREAMOS UNA CLASE DEL MODEL DEPARTAMENTO

                Departamento departamento = new Departamento();
                departamento.Nombre = nombre;
                departamento.IdDepartamento = id;
                departamento.Localidad = localidad;

                //CONVERTIMOS EL OBJETO DEPARTAMENTO EN UN JSON

                string departamentoJson = 
                    JsonConvert.SerializeObject(departamento);

                //PARA ENVIAR EL OBJETO JSON EN EL BOY SE HACE MEDIANTE StringContent INDICANDO TIPO DE CONTENIDO

                StringContent content = new StringContent(departamentoJson, Encoding.UTF8, "application/json");

                //REALIZAMOS LA LLAMADA AL SERVICIO ENVIANCO EL OBJETO JSON

                await client.PostAsync(request, content);

            }
        }

        public async Task UpdateDepartamentoAsync(int id, string nombre, string localidad)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/departamentos";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                Departamento departamento = new Departamento();
                departamento.Nombre = nombre;
                departamento.IdDepartamento = id;
                departamento.Localidad = localidad;
                string json = JsonConvert.SerializeObject(departamento);
                StringContent content= new StringContent(json, Encoding.UTF8, "application/json");
                await client.PutAsync(request, content);

            }
        }
    }
}