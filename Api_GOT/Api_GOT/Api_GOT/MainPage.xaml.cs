using Api_GOT.Modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;

namespace Api_GOT
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<object> Items { get; set; } = new ObservableCollection<object> { 1, "2", true, false };

        private Personajes _selectedPersonaje { get; set; }
        public Personajes SelectedPersonaje {
            get { return _selectedPersonaje; }
            set
            {
                if (_selectedPersonaje != value)
                {
                    _selectedPersonaje = value;
                    HandleSelectedItem();
                }
            }
        }

        private void HandleSelectedItem()
        {
            //DisplayAlert("SelectedItem", "Name: " + SelectedPersonaje.name, "Ok");
            Navigation.PushAsync(new DetailPage(SelectedPersonaje.name));
        }

        public MainPage()
		{
			InitializeComponent();
            //var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            //player.Load("got_song.mp3");
            //player.Play();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await CargarItems();
        }

        private async Task CargarItems()
        {
            if (!Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Advertencia", "No hay internet", "Cerrar");
                return;
            }
            IsBusy = true;

            //await Task.Delay(2500);

            //Items.Add($"Elemento #{Items.Count}");

            Items.Clear();

            var personajes = await CargarPersonajes();

            foreach (var item in personajes)
            {
                Items.Add(item);
            }

            IsBusy = false;

            //await DisplayAlert("Titulo", "Hola!", "Cerrar");
        }

        private async Task<Personajes[]> CargarPersonajes()
        {
            try
            {
                var cliente = new HttpClient();

                cliente.BaseAddress = new Uri(App.WebServiceUrl);
                var json = await cliente.GetStringAsync("characters/");

                //Console.WriteLine(json);

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
