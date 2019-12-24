using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZintNet;

namespace Bartector_Label_Editor
{
    [Serializable]
    internal class PrintBarcode_DataMatric:PrintBarcode_Base
    {

        private ZintNet.DataMatrixSize _DataMatrixSize=ZintNet.DataMatrixSize.Automatic;
        public ZintNet.DataMatrixSize DataMatrixSize
        {
            get
            {
                return _DataMatrixSize;
            }
            set
            {
                _DataMatrixSize = value;
                OnpropertyChanged("DataMatrixSize");
            }
        }

        public PrintBarcode_DataMatric(PrintBarcode_Base printBarcode_Base )
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

        public override Bitmap Paint()
        {
            ZintNetLib zintNetLib = new ZintNetLib();          
            zintNetLib.Rotation = (int)_rotate;     
            zintNetLib.Multiplier = _width;
            zintNetLib.DataMatrixSize = _DataMatrixSize;
            zintNetLib.CreateBarcode(BarcodeSymbology, Data);
            switch (_rotate)
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
                            zintNetLib.DrawBarcode(gr, new System.Drawing.Point(0, 0 + (bcSize.Width - bcSize.Height) / 2));
                            using (Bitmap bmOutput = new Bitmap(bcSize.Height, bcSize.Width))
                            {
                                using (Graphics grOut = Graphics.FromImage(bmOutput))
                                {
                                    grOut.Clear(System.Drawing.Color.White);
                                    grOut.DrawImage(bmBarcode, 0.0f, 0.0f,
                                     new RectangleF(0.0f + (bcSize.Width - bcSize.Height) / 2, 0.0f, (float)bcSize.Height, (float)bcSize.Width),
                                     GraphicsUnit.Pixel);
                                    _imagepixelSize = new System.Windows.Size((float)bcSize.Height, (float)bcSize.Width);
                                }
                                return new Bitmap(bmOutput);
                            }
                        }
                    }
            }
        }
    }
}
