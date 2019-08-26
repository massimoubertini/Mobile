using Xamarin.Forms;

namespace Primo_Progetto_Xamarin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // gestire l'avvio dell'app
        }

        protected override void OnSleep()
        {
            // gestire la sospensione dell'app
        }

        protected override void OnResume()
        {
            // gestire la ripresa dell'app
        }
    }
}