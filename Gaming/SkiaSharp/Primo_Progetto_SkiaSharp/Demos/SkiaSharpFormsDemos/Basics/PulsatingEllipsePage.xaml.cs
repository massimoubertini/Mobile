// elimina l'avviso di non utilizzare await su AnimationLoop
#pragma warning disable 4014

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace SkiaSharpFormsDemos.Basics
{
    public partial class PulsatingEllipsePage : ContentPage
    {
        private Stopwatch stopwatch = new Stopwatch();
        private bool pageIsActive;

        // spazia da 0 a 1 a 0
        private float scale;

        public PulsatingEllipsePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            pageIsActive = true;
            AnimationLoop();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            pageIsActive = false;
        }

        private async Task AnimationLoop()
        {
            stopwatch.Start();

            while (pageIsActive)
            {
                double cycleTime = slider.Value;
                double t = stopwatch.Elapsed.TotalSeconds % cycleTime / cycleTime;
                scale = (1 + (float)Math.Sin(2 * Math.PI * t)) / 2;
                canvasView.InvalidateSurface();
                await Task.Delay(TimeSpan.FromSeconds(1.0 / 30));
            }
            stopwatch.Stop();
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();
            float maxRadius = 0.75f * Math.Min(info.Width, info.Height) / 2;
            float minRadius = 0.25f * maxRadius;
            float xRadius = minRadius * scale + maxRadius * (1 - scale);
            float yRadius = maxRadius * scale + minRadius * (1 - scale);
            using (SKPaint paint = new SKPaint())
            {
                paint.Style = SKPaintStyle.Stroke;
                paint.Color = SKColors.Blue;
                paint.StrokeWidth = 50;
                canvas.DrawOval(info.Width / 2, info.Height / 2, xRadius, yRadius, paint);
                paint.Style = SKPaintStyle.Fill;
                paint.Color = SKColors.SkyBlue;
                canvas.DrawOval(info.Width / 2, info.Height / 2, xRadius, yRadius, paint);
            }
        }
    }
}