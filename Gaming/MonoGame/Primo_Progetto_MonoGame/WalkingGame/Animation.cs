using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WalkingGame
{
    public class Animation
    {
        // i fotogrammi di questa animazione
        List<AnimationFrame> frames = new List<AnimationFrame>();
        // la quantità di tempo nell'animazione
        TimeSpan timeIntoAnimation;
        // la lunghezza dell'intera animazione
        TimeSpan Duration
        {
            get
            {
                double totalSeconds = 0;
                foreach (var frame in frames)
                    totalSeconds += frame.Duration.TotalSeconds;
                return TimeSpan.FromSeconds(totalSeconds);
            }
        }

        public Rectangle CurrentRectangle
        {
            get
            {
                AnimationFrame currentFrame = null;
                // frame
                TimeSpan accumulatedTime = new TimeSpan(0);
                foreach (var frame in frames)
                {
                    if (accumulatedTime + frame.Duration >= timeIntoAnimation)
                    {
                        currentFrame = frame;
                        break;
                    }
                    else
                        accumulatedTime += frame.Duration;
                }
                /* se non è stato trovato alcun frame, prova l'ultimo fotogramma, nel caso
                 * in cui timeIntoAnimation superi in qualche modo Durata */
                if (currentFrame == null)
                    currentFrame = frames.LastOrDefault();
                /* se abbiamo trovato un frame, restituire il suo rettangolo, altrimenti
                 * restituire un rettangolo vuoto (uno senza larghezza o altezza)  */
                if (currentFrame != null)
                    return currentFrame.SourceRectangle;
                else
                    return Rectangle.Empty;
            }
        }

        // aggiunge un singolo frame a questa animazione
        public void AddFrame(Rectangle rectangle, TimeSpan duration)
        {
            AnimationFrame newFrame = new AnimationFrame()
            {
                SourceRectangle = rectangle,
                Duration = duration
            };

            frames.Add(newFrame);
        }

        // aumenta il valore timeIntoAnimation in base al tempo del frame ottenuto da gameTime
        public void Update(GameTime gameTime)
        {
            double secondsIntoAnimation = timeIntoAnimation.TotalSeconds + gameTime.ElapsedGameTime.TotalSeconds;
            double remainder = secondsIntoAnimation % Duration.TotalSeconds;
            timeIntoAnimation = TimeSpan.FromSeconds(remainder);
        }
    }
}