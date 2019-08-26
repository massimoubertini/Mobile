using System;
using System.Diagnostics;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace SkiaSharpFormsDemos.Basics
{
    public class CodeMoreCodePage : ContentPage
    {
        private SKCanvasView canvasView;
        private bool isAnimating;
        private Stopwatch stopwatch = new Stopwatch();
        private double transparency;

        public CodeMoreCodePage()
        {
            Title = "Codice Altro Codice";
            canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            Content = canvasView;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            isAnimating = true;
            stopwatch.Start();
            Device.StartTimer(TimeSpan.FromMilliseconds(16), OnTimerTick);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            stopwatch.Stop();
            isAnimating = false;
        }

        private bool OnTimerTick()
        {
            // secondi
            const int duration = 5;
            double progress = stopwatch.Elapsed.TotalSeconds % duration / duration;
            transparency = 0.5 * (1 + Math.Sin(progress * 2 * Math.PI));
            canvasView.InvalidateSurface();
            return isAnimating;
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();
            const string TEXT1 = "CODE";
            const string TEXT2 = "MORE";
            using (SKPaint paint = new SKPaint())
            {
                // imposta la larghezza del testo per adattarla alla larghezza dell'area di disegno
                paint.TextSize = 100;
                float textWidth = paint.MeasureText(TEXT1);
                paint.TextSize *= 0.9f * info.Width / textWidth;
                // centra la prima stringa di testo
                SKRect textBounds = new SKRect();
                paint.MeasureText(TEXT1, ref textBounds);
                float xText = info.Width / 2 - textBounds.MidX;
                float yText = info.Height / 2 - textBounds.MidY;
                paint.Color = SKColors.Blue.WithAlpha((byte)(0xFF * (1 - transparency)));
                canvas.DrawText(TEXT1, xText, yText, paint);
                // centra la seconda stringa di testo
                textBounds = new SKRect();
                paint.MeasureText(TEXT2, ref textBounds);
                xText = info.Width / 2 - textBounds.MidX;
                yText = info.Height / 2 - textBounds.MidY;
                paint.Color = SKColors.Blue.WithAlpha((byte)(0xFF * transparency));
                canvas.DrawText(TEXT2, xText, yText, paint);
            }
        }
    }
}