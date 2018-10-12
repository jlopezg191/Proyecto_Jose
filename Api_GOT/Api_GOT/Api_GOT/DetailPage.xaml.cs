using Api_GOT.Modelos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Api_GOT
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetailPage : ContentPage
	{
        public ObservableCollection<object> ItemsDetail { get; set; } = new ObservableCollection<object> { 1, "2", true, false };
        private string NameGot;
        private string image = "https://api.got.show";

        public DetailPage (string name)
		{
			InitializeComponent ();
            NameGot = name;

            B_Imagen.Command = new Command(() => {
                Navigation.PushAsync(new ImagePage(image));
            });
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await CargarItems(NameGot);
        }

        private async Task CargarItems(string name)
        {
            if (!Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Advertencia", "No hay internet", "Cerrar");
                return;
            }
            IsBusy = true;

            //await Task.Delay(2500);

            //Items.Add($"Elemento #{Items.Count}");

            ItemsDetail.Clear();

            var personajes = await CargarPersonajes(name);

            foreach (var item in personajes)
            {
                NameG.Text = item.name;
                HouseG.Text = item.house;
                DateBirthG.Text = item.dateOfBirth.ToString();
                MaleG.Text = item.male.ToString();

                if (item.imageLink == null)
                {
                    image = "https://vignette.wikia.nocookie.net/max-steel-reboot/images/7/72/No_Image_Available.gif/revision/latest?cb=20130902173013";
                }
                else
                {
                    image = image + item.imageLink.ToString();
                }
            }
            

            //await DisplayAlert("Titulo", "Hola!", "Cerrar");
        }

        private async Task<Personajes[]> CargarPersonajes(string name)
        {
            try
            {
                var cliente = new HttpClient();

                cliente.BaseAddress = new Uri(App.WebServiceUrlDetail);
                var json = await cliente.GetStringAsync(name);
                var replace = json.Substring(0, 28);
                json = json.Replace(replace, string.Empty);
                json = "[" + json.Substring(0, json.Length - 1).ToString() + "]";
                

                var resultado = JsonConvert.DeserializeObject<Personajes[]>(json);

                return resultado;

            }
            catch (Exception ex)
            {
                // Log 
                return new Personajes[0];
            }
        }

    }
}