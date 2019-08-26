using System;
using SkiaSharp;

namespace SkiaSharpFormsDemos
{
    internal class MatrixDisplay
    {
        public SKPaint MatrixPaint { set; get; } = new SKPaint { Color = SKColors.Black, TextSize = 48, StrokeWidth = 2 };

        public string PerspectiveFormat { set; get; } = "F0";

        public SKSize Measure(SKMatrix matrix)
        {
            return MeasureAndPaint(null, matrix, new SKPoint(), false);
        }

        public void Paint(SKCanvas canvas, SKMatrix matrix, SKPoint location)
        {
            MeasureAndPaint(canvas, matrix, location, true);
        }

        private SKSize MeasureAndPaint(SKCanvas canvas, SKMatrix matrix, SKPoint location, bool doPaint)
        {
            float[] values = matrix.Values;
            string[] texts = new string[9];
            SKRect[] bounds = new SKRect[9];
            float[] widths = new float[3];
            for (int i = 0; i < 9; i++)
            {
                int row = i % 3;
                int col = i / 3;
                // formattare la stringa in modo diverso in base alla riga
                texts[i] = values[i].ToString(row == 2 ? "F0" : (col == 2 ? PerspectiveFormat : "F2"));
                // misurare la stringa con un '-' anche se non è presente
                bool isNegative = texts[i][0] == '-';
                string text = (isNegative ? "" : "-") + texts[i];
                MatrixPaint.MeasureText(text, ref bounds[i]);
                // ottenere la larghezza massima per ogni colonna
                widths[col] = Math.Max(widths[col], bounds[i].Width);
                // misurare di nuovo il testo senza il '-' davanti
                MatrixPaint.MeasureText(texts[i], ref bounds[i]);
            }
            // alcune costanti di formattazione
            float horzGap = MatrixPaint.TextSize;
            float horzMarg = MatrixPaint.TextSize;
            float vertMarg = MatrixPaint.FontSpacing / 4;
            // calcolare la larghezza e l'altezza totali del display a matrice
            float totalWidth = widths[0] + widths[1] + widths[2] + 2 * horzGap + 2 * horzMarg;
            float totalHeight = 3 * MatrixPaint.FontSpacing + 2 * vertMarg;
            if (doPaint)
            {
                SKPaintStyle saveStyle = MatrixPaint.Style;
                for (int i = 0; i < 9; i++)
                {
                    int row = i % 3;
                    int col = i / 3;
                    // trova x, y dell'angolo superiore sinistro del testo
                    float x = location.X + horzMarg;
                    for (int c = 0; c < col; c++)
                        x += widths[c] + horzGap;
                    float y = location.Y + vertMarg + row * MatrixPaint.FontSpacing;
                    // regola per il testo giustificato a destra
                    x += widths[col] - bounds[i].Width;
                    y += (MatrixPaint.FontSpacing - bounds[i].Height) / 2 - bounds[i].Top;
                    // disegnare il testo
                    MatrixPaint.Style = SKPaintStyle.Fill;
                    canvas.DrawText(texts[i], x, y, MatrixPaint);
                }
                // visualizzare le linee verticali ai lati della matrice
                MatrixPaint.Style = SKPaintStyle.Stroke;
                canvas.DrawLine(location.X + horzMarg / 2, location.Y + vertMarg, location.X + horzMarg / 2, location.Y + totalHeight - vertMarg, MatrixPaint);
                canvas.DrawLine(location.X + totalWidth - horzMarg / 2, location.Y + vertMarg, location.X + totalWidth - horzMarg / 2, location.Y + totalHeight - vertMarg, MatrixPaint);
                MatrixPaint.Style = saveStyle;
            }
            return new SKSize(totalWidth, totalHeight);
        }
    }
}