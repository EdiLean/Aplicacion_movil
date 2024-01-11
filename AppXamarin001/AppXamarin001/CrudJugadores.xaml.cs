using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppXamarin001
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrudJugadores : ContentPage
    {
        string apiUrl = "https://apifutboledisson.azurewebsites.net/api/Jugadores";
        public CrudJugadores()
        {
            InitializeComponent();
        }
        private void cmdUpdate_Clicked(object sender, EventArgs e)
        {
            using (var client = new HttpClient())
            {
                var url = $"{apiUrl}/{txtCedula.Text}";
                client.BaseAddress = new Uri(url);
                client
                    .DefaultRequestHeaders
                    .Accept
                    .Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
                var json = JsonConvert.SerializeObject(new Jugador
                {
                    Id = int.Parse(txtCedula.Text),
                    nombreJugador = txtNombre.Text,
                    domicilio = txtDomicilio.Text,
                    telefono = txtTelefono.Text,
                    edad = txtEdad.Text
                });

                var rqst = new HttpRequestMessage(HttpMethod.Put, url);
                rqst.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var resp = client.SendAsync(rqst);
                resp.Wait();
            }
        }

        private void cmdDelete_Clicked(object sender, EventArgs e)
        {
            using (var client = new HttpClient())
            {
                var url = $"{apiUrl}/{txtCedula.Text}";
                client.BaseAddress = new Uri(url);
                client
                    .DefaultRequestHeaders
                    .Accept
                    .Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var resp = client.DeleteAsync(url);
                resp.Wait();

                txtCedula.Text = "";
                txtNombre.Text = "";
                txtDomicilio.Text = string.Empty;
                txtTelefono.Text = string.Empty;
                txtEdad.Text = string.Empty;
            }
        }

        private void cmdReadOne_Clicked(object sender, EventArgs e)
        {
            using (var webClient = new HttpClient())
            {
                var resp = webClient.GetStringAsync(apiUrl + "/" + txtCedula.Text);
                resp.Wait();

                var json = resp.Result;
                var prod = Newtonsoft.Json.JsonConvert.DeserializeObject<Jugador>(json);

                txtCedula.Text = prod.Id.ToString();
                txtNombre.Text = prod.nombreJugador;
                txtDomicilio.Text = prod.domicilio.ToString();
                txtTelefono.Text = prod.telefono.ToString();
                txtEdad.Text = prod.edad.ToString();
            }
        }

        private void cmdInsert_Clicked(object sender, EventArgs e)
        {
            using (var webClient = new HttpClient())
            {
                webClient.BaseAddress = new Uri(apiUrl);
                webClient
                    .DefaultRequestHeaders
                    .Accept
                    .Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(new Jugador
                {
                    Id = int.Parse(txtCedula.Text),
                    nombreJugador = txtNombre.Text,
                    domicilio = txtDomicilio.Text,
                    telefono = txtTelefono.Text,
                    edad = txtEdad.Text
                });

                var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var resp = webClient.SendAsync(request);
                resp.Wait();

                json = resp.Result.Content.ReadAsStringAsync().Result;
                var prod = JsonConvert.DeserializeObject<Jugador>(json);

                txtCedula.Text = prod.Id.ToString();
            }
        }

        private async void cmdRegesar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}