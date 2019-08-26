using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace SkiaSharpFormsDemos.Basics
{
    public class OutlinedTextPage : ContentPage
    {
        public OutlinedTextPage()
        {
            Title = "Testo Strutturato";
            SKCanvasView canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            Content = canvasView;
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();
            string text = "OUTLINE";
            // crea un SKPaint oggetto per visualizzare il testo
            SKPaint textPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1,
                FakeBoldText = true,
                Color = SKColors.Blue
            };
            // regola la proprietà TextSize in modo che il testo sia pari al 95% della larghezza dello schermo
            float textWidth = textPaint.MeasureText(text);
            textPaint.TextSize = 0.95f * info.Width * textPaint.TextSize / textWidth;
            // trovare i limiti del testo
            SKRect textBounds = new SKRect();
            textPaint.MeasureText(text, ref textBounds);
            // calcola gli offset per centrare il testo sullo schermo
            float xText = info.Width / 2 - textBounds.MidX;
            float yText = info.Height / 2 - textBounds.MidY;
            // e disegnare il testo
            canvas.DrawText(text, xText, yText, textPaint);
        }
    }
}