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
    internal class PrintBarcode_QRCode:PrintBarcode_Base
    {
        private ZintNet.QRCodeErrorLevel _QRCodeErrorLevel = ZintNet.QRCodeErrorLevel.Low;
        public ZintNet.QRCodeErrorLevel QRCodeErrorLevel
        {
            get
            {
                return _QRCodeErrorLevel;
            }
            set
            {
                _QRCodeErrorLevel = value;
                OnpropertyChanged("QRCodeErrorLevel");
            }
        }

        private EnumQRsize _QRsize;
        public EnumQRsize QRsize
        {
            get
            {
                return _QRsize;
            }
            set
            {
                _QRsize = value;
                OnpropertyChanged("QRsize");
            }
                }

        public PrintBarcode_QRCode(PrintBarcode_Base printBarcode_Base)
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
            zintNetLib.QRCodeErrorLevel = _QRCodeErrorLevel;
            zintNetLib.QRVersion = (int)_QRsize+1;
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

        private static string[] qrSizes = {
            "21 x 21 (Version 1)", "25 x 25 (Version 2)", "29 x 29 (Version 3)", "33 x 33 (Version 4)", "37 x 37 (Version 5)", "41 x 41 (Version 6)",
            "45 x 45 (Version 7)", "49 x 49 (Version 8)", "53 x 53 (Version 9)", "57 x 57 (Version 10)", "61 x 61 (Version 11)", "65 x 65 (Version 12)",
            "69 x 69 (Version 13)", "73 x 73 (Version 14)", "77 x 77 (Version 15)", "81 x 81 (Version 16)", "85 x 85 (Version 17)", "89 x 89 (Version 18)",
            "93 x 93 (Version 19)", "97 x 97 (Version 20)", "101 x 101 (Version 21)", "105 x 105 (Version 22)", "109 x 109 (Version 23)", "113 x 113 (Version 24)",
            "117 x 117 (Version 25)", "121 x 121 (Version 26)", "125 x 125 (Version 27)", "129 x 129 (Version 28)", "133 x 133 (Version 29)", "137 x 137 (Version 30)",
            "141 x 141 (Version 31)", "145 x 145 (Version 32)", "149 x 149 (Version 33)", "153 x 153 (Version 34)", "157 x 151 (Version 35)", "161 x 161 (Version 36)",
            "165 x 165 (Version 37)", "169 x 169 (Version 38)", "173 x 173 (Version 39)", "177 x 177 (Version 40)" };

        public enum EnumQRsize
        {
            QR21x21,QR25x25,QR29x29,QR33x33,QR37x37,QR41x41,QR45x45,QR49x49,QR53x53,QR57x57,
            QR61x61,QR65x65,QR69x69,QR73x73,QR77x77,QR81x81,QR85x85,QR89x89,QR93x93,QR97x97,
            QR101x101,QR105x105,QR109x109,QR113x113,QR117x117,QR121x121,QR125x125,QR129x129,QR133x133,QR137x137,
            QR141x141,QR145x145,QR149x149,QR153x153,QR157x157,QR161x161,QR165x165,QR169x169,QR173x173,QR177x177
        }
    }
}
