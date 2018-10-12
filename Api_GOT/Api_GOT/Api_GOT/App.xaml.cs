using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Api_GOT
{
	public partial class App : Application
	{
        public const string WebServiceUrl = "https://api.got.show/api/";
        public const string WebServiceUrlDetail = "https://api.got.show/api/characters/";

        public App ()
		{
			InitializeComponent();

            var mainPage = new MainPage() { Title = "Game of Thrones" };
            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load("got_song.mp3");
            player.Play();

            Device.StartTimer(TimeSpan.FromSeconds(89), () =>
            {
                player.Play();

                return true; // True = Repeat again, False = Stop the timer
            });

            MainPage = new NavigationPage(mainPage)
            {
                BarTextColor = Color.White,
                BarBackgroundColor = Color.Black
            };
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
