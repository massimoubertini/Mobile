using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WalkingGame
{
    // questo è il main per il  gioco
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        CharacterEntity character;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = true;
        }

        /* consente al gioco di eseguire qualsiasi inizializzazione che dev eeseguire
         * prima d'iniziare l'esecuzione, questo è dove può interrogare per tutti i servizi
         * richiesti e caricare qualsiasi non-grafico contenuti correlati, chiamata base:
         * Initialize inizializza tutti i componenti  */
        protected override void Initialize()
        {
            character = new CharacterEntity(this.GraphicsDevice);
            base.Initialize();
        }

        // LoadContent sarà chiamato una volta per game ed è il luogo in cui caricare i contenuti
        protected override void LoadContent()
        {
            // crea un nuovo SpriteBatch che può essere utilizzato per disegnare le textures
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /* consente al gioco di eseguire la logica come l'aggiornamento del mondo,
         * controllare le collisioni, la raccolta dell'input e la riproduzione audio
         * gameTime fornisce uno snapshot dei valori di temporizzazione */
        protected override void Update(GameTime gameTime)
        {
            character.Update(gameTime);
            base.Update(gameTime);
        }

        /* questo è chiamato quando il gioco dovrebbe disegnare da solo
         * gameTime fornisce uno snapshot dei valori di temporizzazione */
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // inizieremo tutto il nostro disegno qui
            spriteBatch.Begin();
            // ora possiamo fare qualsiasi rendering di entità
            character.Draw(spriteBatch);
            // fine, esegue il rendering di tutti gli sprites sullo schermo
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}