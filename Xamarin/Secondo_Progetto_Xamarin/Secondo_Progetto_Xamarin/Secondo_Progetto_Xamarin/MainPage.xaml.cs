using System;
using System.ComponentModel;
using System.IO;
using Xamarin.Forms;

namespace Secondo_Progetto_Xamarin
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "note.txt");

        public MainPage()
        {
            InitializeComponent();
            if (File.Exists(_fileName))
                editor.Text = File.ReadAllText(_fileName);
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            File.WriteAllText(_fileName, editor.Text);
        }

        private void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            if (File.Exists(_fileName))
                File.Delete(_fileName);
            editor.Text = string.Empty;
        }
    }
}