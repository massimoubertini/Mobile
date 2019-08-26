using System;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace SkiaSharpFormsDemos.Basics
{
    public class SurfaceSizePage : ContentPage
    {
        private SKCanvasView canvasView;

        public SurfaceSizePage()
        {
            Title = "Dimensioni Superficie";
            canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            Content = canvasView;
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();
            SKPaint paint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 40
            };
            float fontSpacing = paint.FontSpacing;
            // margine sinistro
            float x = 20;
            // prima linea di base
            float y = fontSpacing;
            float indent = 100;
            canvas.DrawText("SKCanvasView Height e Width:", x, y, paint);
            y += fontSpacing;
            canvas.DrawText(String.Format("{0:F2} x {1:F2}", canvasView.Width, canvasView.Height), x + indent, y, paint);
            y += fontSpacing * 2;
            canvas.DrawText("SKCanvasView CanvasSize:", x, y, paint);
            y += fontSpacing;
            canvas.DrawText(canvasView.CanvasSize.ToString(), x + indent, y, paint);
            y += fontSpacing * 2;
            canvas.DrawText("SKImageInfo Size:", x, y, paint);
            y += fontSpacing;
            canvas.DrawText(info.Size.ToString(), x + indent, y, paint);
        }
    }
}