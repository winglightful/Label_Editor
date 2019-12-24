using System;
using System.Collections.Generic;
using System.Windows;
using ZintNet;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media;
using System.Drawing;

namespace Bartector_Label_Editor
{
    [Serializable]
    internal class PrintBarcode_Base : PrintObject_Base
    {

        /// <summary>
        /// Barcode catagory
        /// </summary>    
        private Symbology _barcodeSymbology;
        [Description("Barcode catagory")]
        public Symbology BarcodeSymbology
        {
            get
            {
                return _barcodeSymbology;
            }
            set
            {
                _barcodeSymbology = value;
                OnpropertyChanged("BarcodeSymbology");
            }
        }
        protected float _width;
        /// <summary>
        /// cell width
        /// </summary>
        [Description(" cell width")]
        public float Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                OnpropertyChanged("Width");
            }
        }
        /// <summary>
        ///Barcode Hight(mm)
        /// </summary>
        private float _height;
        [Description("Barcode Height(mm)")]
        public float Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                OnpropertyChanged("Height");
            }
        }
  
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
        public double Y {
            get
            {
                return _location.Y;
            }
            set
            {
                Location = new System.Windows.Point( _location.X,value);
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

        private string fontfamily= "Arial";
       
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

        private int _fontsize=10;
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
        private bool _textVisable;
        [Category("Text")]
        [Description("TextVisable")]
        public bool TextVisable
        {
            get
            {
                return _textVisable;
            }
            set
            {
                _textVisable = value;
                OnpropertyChanged("TextVisable");
            }
        }
        private ZintNet.TextAlignment  _textAligment;
        [Category("Text")]
        [Description("TextAligment")]
        public ZintNet.TextAlignment TextAligment
        {
            get
            {
                return _textAligment;
            }
            set
            {
                _textAligment = value;
                OnpropertyChanged("TextAligment");
            }
        }

        private ZintNet.TextPosition _textPosition;
        [Category("Text")]
        [Description("TextPosition")]
        public ZintNet.TextPosition TextPosition
        {
            get
            {
                return _textPosition;
            }
            set
            {
                _textPosition = value;
                OnpropertyChanged("TextPosition");
            }
        }
       
        public PrintBarcode_Base()
        { }
        public PrintBarcode_Base(PrintBarcode_Base printBarcode_Base)
        {
            var sourceProps = typeof(PrintBarcode_Base).GetProperties().Where(x => x.CanRead).ToList();
            var destProps = typeof(PrintBarcode_DataMatric).GetProperties()
                    .Where(x => x.CanWrite)
                    .ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (destProps.Any(x => x.Name == sourceProp.Name))
                {
                    var p = destProps.First(x => x.Name == sourceProp.Name);
                    if (p.CanWrite)
                    { // check if the property can be set or no.
                        p.SetValue(this, sourceProp.GetValue(printBarcode_Base, null), null);
                    }
                }
            }
        }
        public override  Bitmap Paint()
        {
            ZintNetLib zintNetLib = new ZintNetLib();
            zintNetLib.TextVisible = _textVisable;
            zintNetLib.Multiplier = _width;
            zintNetLib.BarcodeHeight = _height;
            zintNetLib.Rotation = (int)_rotate;
            zintNetLib.CreateBarcode(BarcodeSymbology, Data);
            Font font = new Font(fontfamily, (float)FontSize, (System.Drawing.FontStyle)Enum.ToObject(typeof(System.Drawing.FontStyle),(int)FontStyle ));
            if(_fontFamily?.ToString() != fontfamily)
            {
                _fontFamily = new System.Windows.Media.FontFamily(fontfamily);
            }
            zintNetLib.Font = font;
            zintNetLib.TextAlignment = _textAligment;
            zintNetLib.TextPosition = _textPosition;
            if (!zintNetLib.IsValid)
            {
                return null;
            }
            switch(_rotate)
            {
                case EnumRotate._0:
                case EnumRotate._180:
                default:
                    using (Bitmap bmBarcode = new Bitmap(10000, 10000))
                    {
                        using (Graphics gr = Graphics.FromImage(bmBarcode))
                        {
                            var bcSize = zintNetLib.SymbolSize(gr);
                            zintNetLib.DrawBarcode(gr, new System.Drawing.Point(0, 0));
                            using (Bitmap bmOutput = new Bitmap(bcSize.Width, bcSize.Height))
                            {
                                using (Graphics grOut = Graphics.FromImage(bmOutput))
                                {
                                    grOut.Clear(System.Drawing.Color.White);
                                    grOut.DrawImage(bmBarcode, 0.0f, 0.0f,
                                     new RectangleF(0.0f, 0.0f, (float)bcSize.Width, (float)bcSize.Height),
                                     GraphicsUnit.Pixel);                                    
                                    _imagepixelSize = new System.Windows.Size((float)bcSize.Width, (float)bcSize.Height);
                                }
                                return new Bitmap(bmOutput);
                            }
                        }
                    }
                case EnumRotate._90:
                case EnumRotate._270:
                    using (Bitmap bmBarcode = new Bitmap(10000, 10000))
                    {
                        using (Graphics gr = Graphics.FromImage(bmBarcode))
                        {
                            var bcSize = zintNetLib.SymbolSize(gr);
                            zintNetLib.DrawBarcode(gr, new System.Drawing.Point(0, 0+ (bcSize.Width - bcSize.Height) / 2));
                            using (Bitmap bmOutput = new Bitmap(bcSize.Height, bcSize.Width))
                            {
                                using (Graphics grOut = Graphics.FromImage(bmOutput))
                                {
                                    grOut.Clear(System.Drawing.Color.White);
                                    grOut.DrawImage(bmBarcode, 0.0f, 0.0f,
                                     new RectangleF(0.0f + (bcSize.Width - bcSize.Height) / 2, 0.0f , (float)bcSize.Height, (float)bcSize.Width),
                                     GraphicsUnit.Pixel);
                                    _imagepixelSize = new System.Windows.Size((float)bcSize.Height, (float)bcSize.Width);
                                }
                                return new Bitmap(bmOutput);
                            }
                        }
                    }
            }       
        }
        public  PrintBarcode_Base DeepCopy()
        {
            PrintBarcode_Base barcode_Base_copy;
            barcode_Base_copy =(PrintBarcode_Base) MemberwiseClone();
            return barcode_Base_copy;
        }
    }
}
