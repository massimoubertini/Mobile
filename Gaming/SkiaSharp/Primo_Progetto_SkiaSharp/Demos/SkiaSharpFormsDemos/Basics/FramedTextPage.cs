using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace SkiaSharpFormsDemos.Basics
{
    public class FramedTextPage : ContentPage
    {
        public FramedTextPage()
        {
            Title = "Testo incorniciato";
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
            string str = "Hello SkiaSharp!";
            // crea un SKPaint oggetto per visualizzare il testo
            SKPaint textPaint = new SKPaint
            {
                Color = SKColors.Chocolate
            };
            // regola la proprietà TextSize in modo che il testo sia 90% della larghezza dello schermo
            float textWidth = textPaint.MeasureText(str);
            textPaint.TextSize = 0.9f * info.Width * textPaint.TextSize / textWidth;
            // trovare i limiti del testo
            SKRect textBounds = new SKRect();
            textPaint.MeasureText(str, ref textBounds);
            // calcola gli offset per centrare il testo sullo schermo
            float xText = info.Width / 2 - textBounds.MidX;
            float yText = info.Height / 2 - textBounds.MidY;
            // e disegna il testo
            canvas.DrawText(str, xText, yText, textPaint);
            // crea un nuovo skRect oggetto per la cornice intorno al testo
            SKRect frameRect = textBounds;
            frameRect.Offset(xText, yText);
            frameRect.Inflate(10, 10);
            // crea un SKPaint oggetto per visualizzare il frame
            SKPaint framePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 5,
                Color = SKColors.Blue
            };
            // disegna un frame
            canvas.DrawRoundRect(frameRect, 20, 20, framePaint);
            // gonfiare il frameRect e disegnarne un altro
            frameRect.Inflate(10, 10);
            framePaint.Color = SKColors.DarkBlue;
            canvas.DrawRoundRect(frameRect, 30, 30, framePaint);
        }
    }
}