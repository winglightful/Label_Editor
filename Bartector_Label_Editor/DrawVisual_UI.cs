using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Bartector_Label_Editor
{
      partial class PrintObject_Action
    {
        private System.Windows.Media.DrawingVisual CreateDrawingVisualRectangle()
        {
            System.Windows.Media.DrawingVisual drawingVisual = new System.Windows.Media.DrawingVisual();

            // Retrieve the DrawingContext in order to create new drawing content.
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            // Create a rectangle and draw it in the DrawingContext.
            Rect rect = new Rect(new System.Windows.Point(160, 100), new System.Windows.Size(320, 80));
            drawingContext.DrawRectangle(System.Windows.Media.Brushes.LightBlue, null, rect);

            // Persist the drawing content.
            drawingContext.Close();

            return drawingVisual;
        }

        private DrawingVisual CreateObjectDrawingVisual(ObservableCollection<PrintObject_Base> Barcode_Base_List)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            // Retrieve the DrawingContext in order to create new drawing content.
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            foreach (PrintObject_Base b in Barcode_Base_List)
            {
                // Create a rectangle and draw it in the DrawingContext.
                BitmapImage imageSource = ConvertBitmap(b.Paint());
                drawingContext.DrawImage(imageSource, new Rect(b.Location, new System.Windows.Size(imageSource.Width, imageSource.Height)));
            }
            // Persist the drawing content.
            drawingContext.Close();

            return drawingVisual;
        }

        private DrawingVisual CreateObjectDrawingVisual(PrintObject_Base Barcode_Base)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            // Retrieve the DrawingContext in order to create new drawing content.
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            // Create a rectangle and draw it in the DrawingContext.
            BitmapImage imageSource = ConvertBitmap(Barcode_Base.Paint());
            drawingContext.DrawImage(imageSource, new Rect(Barcode_Base.Location, new System.Windows.Size(imageSource.Width, imageSource.Height)));
            // Persist the drawing content.
            drawingContext.Close();

            return drawingVisual;
        }

        private DrawingVisual CreateDashRectDrawingVisual(Rect rectangle)
        {
            var Pen =new System.Windows.Media.Pen(System.Windows.Media.Brushes.Black, 2)
            {
                DashStyle = DashStyles.Dash
            };
            DrawingVisual drawingVisual = new DrawingVisual();
            // Retrieve the DrawingContext in order to create new drawing content.
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawRectangle(null, Pen, rectangle);
            drawingContext.Close();
            return drawingVisual;
        }
        // Provide a required override for the VisualChildrenCount property.
        protected override int VisualChildrenCount => _children.Count;

        // Provide a required override for the GetVisualChild method.
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= _children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return _children[index];
        }

        private static BitmapImage ConvertBitmap(Bitmap source)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                source.Save(stream, System.Drawing.Imaging.ImageFormat.Png); // 坑点：格式选Bmp时，不带透明度

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }

        // Capture the mouse event and hit test the coordinate point value against
        // the child visual objects.
        private void BarcodeAction_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Retreive the coordinates of the mouse button event.
            System.Windows.Point pt = e.GetPosition((UIElement)sender);
            foreach (PrintObject_Base b in PrintObject_List)
            {
                Rect rectangle = new Rect(b.Location, b.ImagepixelSize);
                if (rectangle.Contains(pt))
                {
                    Rect rectangleDash = new Rect(new System.Windows.Point(b.Location.X - 2, b.Location.Y - 2), new System.Windows.Size(b.ImagepixelSize.Width + 4, b.ImagepixelSize.Height + 4));
                    if (_children.Contains(DashRect))
                    {
                        _children.Remove(DashRect);
                    }
                    DashRect = CreateDashRectDrawingVisual(rectangleDash);
                    _children.Add(DashRect);
                    BarcodeMouseHitEvent?.Invoke(b);
                    break;
                }
            }
        }
    }
}
