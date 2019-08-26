using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace WalkingGame
{
    public class CharacterEntity
    {
        static Texture2D characterSheetTexture;
        Animation walkDown;
        Animation walkUp;
        Animation walkLeft;
        Animation walkRight;
        Animation standDown;
        Animation standUp;
        Animation standLeft;
        Animation standRight;
        Animation currentAnimation;

        public float X
        {
            get;
            set;
        }

        public float Y
        {
            get;
            set;
        }

        public CharacterEntity(GraphicsDevice graphicsDevice)
        {
            if (characterSheetTexture == null)
            {
                using (var stream = TitleContainer.OpenStream("Content/charactersheet.png"))
                {
                    characterSheetTexture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }

            walkDown = new Animation();
            walkDown.AddFrame(new Rectangle(0, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkDown.AddFrame(new Rectangle(16, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkDown.AddFrame(new Rectangle(0, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkDown.AddFrame(new Rectangle(32, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkUp = new Animation();
            walkUp.AddFrame(new Rectangle(144, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkUp.AddFrame(new Rectangle(160, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkUp.AddFrame(new Rectangle(144, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkUp.AddFrame(new Rectangle(176, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkLeft = new Animation();
            walkLeft.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkLeft.AddFrame(new Rectangle(64, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkLeft.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkLeft.AddFrame(new Rectangle(80, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkRight = new Animation();
            walkRight.AddFrame(new Rectangle(96, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkRight.AddFrame(new Rectangle(112, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkRight.AddFrame(new Rectangle(96, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkRight.AddFrame(new Rectangle(128, 0, 16, 16), TimeSpan.FromSeconds(.25));
            // le animazioni permanenti hanno un solo frame di animazione
            standDown = new Animation();
            standDown.AddFrame(new Rectangle(0, 0, 16, 16), TimeSpan.FromSeconds(.25));
            standUp = new Animation();
            standUp.AddFrame(new Rectangle(144, 0, 16, 16), TimeSpan.FromSeconds(.25));
            standLeft = new Animation();
            standLeft.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(.25));
            standRight = new Animation();
            standRight.AddFrame(new Rectangle(96, 0, 16, 16), TimeSpan.FromSeconds(.25));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 topLeftOfSprite = new Vector2(this.X, this.Y);
            Color tintColor = Color.White;
            var sourceRectangle = currentAnimation.CurrentRectangle;
            spriteBatch.Draw(characterSheetTexture, topLeftOfSprite, sourceRectangle, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            var velocity = GetDesiredVelocityFromInput();
            this.X += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.Y += velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
            // la variabile velocità determina se il carattere si muove o si sta fermando
            bool isMoving = velocity != Vector2.Zero;
            if (isMoving)
            {
                /* se il valore assoluto del componente X è maggiore del valore assoluto
                 * della componente Y, allora questo significa che il carattere si sta
                 * muovendo orizzontalmente */
                bool isMovingHorizontally = Math.Abs(velocity.X) > Math.Abs(velocity.Y);
                if (isMovingHorizontally)
                {
                    /* non  sappiamo se il carattere si muove orizzontalmente
                     * possiamo verificare se la velocità è positiva (muovendosi a destra)
                     * o negativa (spostamento a sinistra) */
                    if (velocity.X > 0)
                        currentAnimation = walkRight;
                    else
                        currentAnimation = walkLeft;
                }
                else
                {
                    /* se il carattere non si muove orizzontalmente allora deve muoversi
                     * verticalmente, la classe SpriteBatch tratta positiva Y come giù, quindi
                     * questo definisce il sistema di coordinate per il gioco, pertanto, se Y
                     * è positivo, quindi il carattere si muove verso il basso, in caso
                     * contrario, il carattere si sta spostando verso l'alto  */
                    if (velocity.Y > 0)
                        currentAnimation = walkDown;
                    else
                        currentAnimation = walkUp;
                }
            }
            else
            {
                /* questa istruzione else contiene la logica per il carattere, in primo luogo
                 * ci accingiamo a controllare se il carattere sta attualmente
                 * riproducendo tutte le animazioni a piedi, se è così, allora vogliamo
                 * passare ad un'animazione permanente, vogliamo preservare la
                 * direzione in cui il carattere è rivolto verso di noi in modo da
                 * impostare la posizione corrispondente l'animazione in base
                 * all'animazione a piedi riprodotta */
                if (currentAnimation == walkRight)
                    currentAnimation = standRight;
                else if (currentAnimation == walkLeft)
                    currentAnimation = standLeft;
                else if (currentAnimation == walkUp)
                    currentAnimation = standUp;
                else if (currentAnimation == walkDown)
                    currentAnimation = standDown;
                // se il carattere è fermo ma non mostra alcuna animazione, allora avremo di default la fronte verso il basso
                else if (currentAnimation == null)
                    currentAnimation = standDown;
            }

            currentAnimation.Update(gameTime);
        }

        Vector2 GetDesiredVelocityFromInput()
        {
            Vector2 desiredVelocity = new Vector2();
            TouchCollection touchCollection = TouchPanel.GetState();
            if (touchCollection.Count > 0)
            {
                desiredVelocity.X = touchCollection[0].Position.X - this.X;
                desiredVelocity.Y = touchCollection[0].Position.Y - this.Y;
                if (desiredVelocity.X != 0 || desiredVelocity.Y != 0)
                {
                    desiredVelocity.Normalize();
                    const float desiredSpeed = 200;
                    desiredVelocity *= desiredSpeed;
                }
            }
            return desiredVelocity;
        }
    }
}