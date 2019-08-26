using System.ComponentModel;
using Xamarin.Forms;

namespace Primo_Progetto_Xamarin
{
    /* ulteriori informazioni su come rendere visibile il codice personalizzato nel
     * visualizzatore anteprima Xamarin.Forms, visitare  https://aka.ms/xamarinforms-previewer */

    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private int count = 0;

        private void BtnPremi_Clicked(object sender, System.EventArgs e)
        {
            count++;
            ((Button)sender).Text = $"Hai fatto clic {count}.";
        }
    }
}