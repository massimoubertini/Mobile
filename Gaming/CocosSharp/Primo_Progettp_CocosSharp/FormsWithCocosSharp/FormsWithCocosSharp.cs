using Xamarin.Forms;

namespace FormsWithCocosSharp
{
    public class App : Application
    {
        public App()
        {
            // pagina radice dell'app
            MainPage = new FormsWithCocosSharp.MainPage();
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
            // gestire la ripresa dell'app
        }
    }
}