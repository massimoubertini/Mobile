using System;
using Xamarin.Forms;
using CocosSharp;

namespace FormsWithCocosSharp
{
    public class MainPage : ContentPage
    {
        // mantenere il GameScene nell'ambito della classe in modo che gli eventi clic del pulsante possano accedervi
        private GameScene gameScene;

        public MainPage()
        {
            // questa è la griglia di primo livello che dividerà la nostra pagina a metà
            var grid = new Grid();
            grid.RowSpacing = 0;
            this.Content = grid;
            grid.RowDefinitions = new RowDefinitionCollection {
				// ogni metà sarà della stessa dimensione
				new RowDefinition{ Height = new GridLength(1, GridUnitType.Star)},
                new RowDefinition{ Height = new GridLength(1, GridUnitType.Star)},
            };
            CreateTopHalf(grid);
            CreateBottomHalf(grid);
        }

        private void CreateTopHalf(Grid grid)
        {
            // questo ospita la nostra vista di gioco
            var gameView = new CocosSharpView()
            {
                /* ha le stesse proprietà di altre visualizzazioni XamarinForms
                 * questo è chiamato dopo CocosSharp avviato */
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                ViewCreated = HandleViewCreated
            };
            // lo aggiungeremo alla metà superiore (riga 0)
            grid.Children.Add(gameView, 0, 0);
        }

        private void CreateBottomHalf(Grid grid)
        {
            // useremo un StackLayout per organizzare i nostri pulsanti
            var stackLayout = new StackLayout();
            // i primo pulsante sposterà il cerchio a sinistra quando si fa clic
            var moveLeftButton = new Button
            {
                Text = "Move Circle Left"
            };
            moveLeftButton.Clicked += (sender, e) => gameScene.MoveCircleLeft();
            stackLayout.Children.Add(moveLeftButton);
            // il secondo pulsante sposterà il cerchio a destra quando si fa clic su
            var moveCircleRight = new Button
            {
                Text = "Sposta il cerchio a destra"
            };
            moveCircleRight.Clicked += (sender, e) => gameScene.MoveCircleRight();
            stackLayout.Children.Add(moveCircleRight);
            // il layout dello stack sarà nella metà inferiore (riga 1)
            grid.Children.Add(stackLayout, 0, 1);
        }

        /* LoadGame è chiamato quando CocosSharp è inizializzato,
         * possiamo iniziare a creare i nostri oggetti CocosSharp qui */

        private void HandleViewCreated(object sender, EventArgs e)
        {
            var gameView = sender as CCGameView;
            if (gameView != null)
            {
                // imposta la risoluzione "mondo" del gioco su 100x100
                gameView.DesignResolution = new CCSizeI(100, 100);
                // GameScene è la radice della gerarchia di rendering CocosSharp
                gameScene = new GameScene(gameView);
                // start CocosSharp
                gameView.RunWithScene(gameScene);
            }
        }
    }
}