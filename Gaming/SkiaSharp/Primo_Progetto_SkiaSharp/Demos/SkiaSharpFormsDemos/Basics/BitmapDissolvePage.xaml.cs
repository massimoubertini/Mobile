using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace SkiaSharpFormsDemos.Basics
{
    public partial class BitmapDissolvePage : ContentPage
    {
        private SKBitmap bitmap1;
        private SKBitmap bitmap2;

        public BitmapDissolvePage()
        {
            InitializeComponent();
            // carica due bitmap
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("SkiaSharpFormsDemos.Media.SeatedMonkey.jpg"))
            {
                bitmap1 = SKBitmap.Decode(stream);
            }
            using (Stream stream = assembly.GetManifestResourceStream("SkiaSharpFormsDemos.Media.FacePalm.jpg"))
            {
                bitmap2 = SKBitmap.Decode(stream);
            }
        }

        private void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            canvasView.InvalidateSurface();
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();
            // trova rettangolo per adattarlo alla bitmap
            float scale = Math.Min((float)info.Width / bitmap1.Width, (float)info.Height / bitmap1.Height);
            SKRect rect = SKRect.Create(scale * bitmap1.Width, scale * bitmap1.Height);
            float x = (info.Width - rect.Width) / 2;
            float y = (info.Height - rect.Height) / 2;
            rect.Offset(x, y);
            // ottenere il valore di avanzamento da Slider
            float progress = (float)progressSlider.Value;
            // visualizza due bitmap con trasparenza
            using (SKPaint paint = new SKPaint())
            {
                paint.Color = paint.Color.WithAlpha((byte)(0xFF * (1 - progress)));
                canvas.DrawBitmap(bitmap1, rect, paint);
                paint.Color = paint.Color.WithAlpha((byte)(0xFF * progress));
                canvas.DrawBitmap(bitmap2, rect, paint);
            }
        }
    }
}