using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace SkiaSharpFormsDemos
{
    public class App : Application
    {
        public App()
        {
            // la pagina radice dell'applicazione
            MainPage = new NavigationPage(new HomePage());
        }

        protected override void OnStart()
        {
            // gestire all'avvio dell'app
        }

        protected override void OnSleep()
        {
            // gestire quando l'app è in sospensione
        }

        protected override void OnResume()
        {
            // Gestire la ripresa dell'app
        }
    }
}