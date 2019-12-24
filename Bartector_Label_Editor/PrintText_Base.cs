using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bartector_Label_Editor
{
    [Serializable]
    internal class PrintText_Base : PrintObject_Base
    {
    
        [Category("Location")]
        [Description(" LocationX")]
        public double X
        {
            get
            {
                return _location.X;
            }
            set
            {
                Location = new System.Windows.Point(value, _location.Y);
            }
        }
        [Category("Location")]
        [Description(" LocationY")]
        public double Y
        {
            get
            {
                return _location.Y;
            }
            set
            {
                Location = new System.Windows.Point(_location.X, value);
            }
        }
        [NonSerialized]
        private System.Windows.Media.FontFamily _fontFamily = new System.Windows.Media.FontFamily("Arial");
        [Category("Font")]
        [Description("FontFamily")]
        public System.Windows.Media.FontFamily FontFamily
        {
            get
            {
                return _fontFamily;
            }
            set
            {
                _fontFamily = value;
                fontfamily = _fontFamily.ToString();
                OnpropertyChanged("FontFamily");
            }
        }

        private string fontfamily = "Arial";

        private EnumFontStyle _fontStyle;
        [Category("Font")]
        [Description("FontStyle")]
        public EnumFontStyle FontStyle
        {
            get
            {
                return _fontStyle;
            }
            set
            {
                _fontStyle = value;
                OnpropertyChanged("FontStyle");
            }
        }

        private int _fontsize = 10;
        [Category("Font")]
        [Description("Fontsize")]
        public int FontSize
        {
            get
            {
                return _fontsize;
            }
            set
            {
                _fontsize = value;
                OnpropertyChanged("FontSize");
            }
        }

        //private EnumRotate _rotate;
        //public EnumRotate Rotate
        //{
        //    get
        //    {
        //        return _rotate;
        //    }
        //    set
        //    {
        //        _rotate = value;
        //        OnpropertyChanged("Rotate");
        //    }
        //}

        public override Bitmap Paint()
        {
            using (Bitmap bmText = new Bitmap(500, 500))
            {
                using (Graphics gr = Graphics.FromImage(bmText))
                {
                    Font FontText= new Font(fontfamily, (float)FontSize, (System.Drawing.FontStyle)Enum.ToObject(typeof(System.Drawing.FontStyle), (int)FontStyle));
                    if (_fontFamily?.ToString() != fontfamily)
                    {
                        _fontFamily = new System.Windows.Media.FontFamily(fontfamily);
                    }
                    SizeF stringsize = gr.MeasureString(Data, FontText);                               
                    switch (_rotate)
                    {
                        case EnumRotate._0:
                        default:
                            _imagepixelSize = new System.Windows.Size(stringsize.Width, stringsize.Height);
                            gr.DrawString(Data, FontText, Brushes.Black, new PointF(0, 0));
                                using (Bitmap bmOutput = new Bitmap((int)stringsize.Width+10, (int)stringsize.Height+10))
                                {
                                    using (Graphics grOut = Graphics.FromImage(bmOutput))
                                    {
                                        grOut.Clear(System.Drawing.Color.White);
                                        grOut.DrawImage(bmText, 0, 0,
                                         new RectangleF(0, 0, stringsize.Width, stringsize.Height),
                                         GraphicsUnit.Pixel);
                                    }
                                    return new Bitmap(bmOutput);
                            }
                        case EnumRotate._270:
                            _imagepixelSize = new System.Windows.Size(stringsize.Height, stringsize.Width);
                            gr.RotateTransform(-(float)_rotate);
                            //gr.TranslateTransform(0.0f, 0.0f);
                            gr.DrawString(Data, FontText, Brushes.Black, new PointF(0, -stringsize.Height));
                            using (Bitmap bmOutput = new Bitmap((int)stringsize.Height+10, (int)stringsize.Width+10))
                            {
                                using (Graphics grOut = Graphics.FromImage(bmOutput))
                                {
                                    grOut.Clear(System.Drawing.Color.White);
                                    grOut.DrawImage(bmText, 0, 0,
                                     new RectangleF(0, 0, stringsize.Height, stringsize.Width),
                                     GraphicsUnit.Pixel);
                                }
                                return new Bitmap(bmOutput);
                            }
                        case EnumRotate._90:
                            _imagepixelSize = new System.Windows.Size(stringsize.Height, stringsize.Width);
                            gr.RotateTransform(-(float)_rotate);
                            //gr.TranslateTransform(0.0f, 0.0f);
                            gr.DrawString(Data, FontText, Brushes.Black, new PointF(-(int)stringsize.Width, 0));
                            using (Bitmap bmOutput = new Bitmap((int)stringsize.Height+10, (int)stringsize.Width+10))
                                {
                                    using (Graphics grOut = Graphics.FromImage(bmOutput))
                                    {
                                        grOut.Clear(System.Drawing.Color.White);
                                        grOut.DrawImage(bmText, 0, 0,
                                         new RectangleF(0, 0, stringsize.Height, stringsize.Width),
                                         GraphicsUnit.Pixel);
                                    }
                                    return new Bitmap(bmOutput);
                                }
                            
                        case EnumRotate._180:
                            _imagepixelSize = new System.Windows.Size(stringsize.Width, stringsize.Height);
                            gr.RotateTransform((float)_rotate);
                            //gr.TranslateTransform(0.0f, 0.0f);
                            gr.DrawString(Data, FontText, Brushes.Black, new PointF(-stringsize.Width, -stringsize.Height));
                            using (Bitmap bmOutput = new Bitmap((int)stringsize.Width+10, (int)stringsize.Height+10))
                            {
                                using (Graphics grOut = Graphics.FromImage(bmOutput))
                                {
                                    grOut.Clear(System.Drawing.Color.White);
                                    grOut.DrawImage(bmText, 0, 0,
                                     new RectangleF(0, 0, stringsize.Width, stringsize.Height),
                                     GraphicsUnit.Pixel);
                                }
                                return new Bitmap(bmOutput);
                            }                            
                    }
                    }                  
                }
            }

        public PrintText_Base DeepCopy()
        {
            PrintText_Base printText_Base_copy;
            printText_Base_copy =(PrintText_Base) MemberwiseClone();
            return printText_Base_copy;
        }
    }
}
