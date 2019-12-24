using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Bartector_Label_Editor
{
    internal partial class PrintObject_Action : FrameworkElement
    {
        // Create a collection of child visual objects.
        private readonly VisualCollection _children;
        public ObservableCollection<PrintObject_Base> PrintObject_List{ get;set; } = new ObservableCollection<PrintObject_Base>();
        public delegate void BarcodeMouseHit(PrintObject_Base barcode);
        public event BarcodeMouseHit BarcodeMouseHitEvent;
        private DrawingVisual DashRect;
        private Bitmap _ImageTotal;
        public PrintObject_Action()
        {
            _children = new VisualCollection(this);
            PrintObject_List.CollectionChanged += OnPrintObjectListChanged;
            MouseLeftButtonUp += BarcodeAction_MouseLeftButtonUp;
        }
        /// <summary>
        /// Action when PrintObjectList change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPrintObjectListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _children.Clear();
            //_children.Add( CreateObjectDrawingVisual(Barcode_Base_List));
            int index = 0;
           foreach(PrintObject_Base b in PrintObject_List)
            {
                b._Index = index;
                index++;
                _children.Add(CreateObjectDrawingVisual(b));
            }
            if (_children.Contains(DashRect))
            {
                _children.Remove(DashRect);
            }
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= ItemList_PropertyChanged;
            }
            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                    item.PropertyChanged += ItemList_PropertyChanged;
            }
        }     

        private void ItemList_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName== "BarcodeSymbology")
            {
                PrintBarcode_Base _base = (PrintBarcode_Base)sender;
                if (_base.BarcodeSymbology == ZintNet.Symbology.DataMatrix)
                {
                    _base = new PrintBarcode_DataMatric(_base);
                }
                else if(_base.BarcodeSymbology == ZintNet.Symbology.QRCode)
                {
                    _base = new PrintBarcode_QRCode(_base);
                }
                else
                {
                    _base = new PrintBarcode_Base(_base);
                }
                PrintObject_List[PrintObject_List.IndexOf((PrintObject_Base)sender)] = _base;
            }
            _children.Clear();
            _children.Add(CreateObjectDrawingVisual(PrintObject_List));
            if (_children.Contains(DashRect))
            {
                _children.Remove(DashRect);
            }
        }   

        public void TemplateSave(string Path)
        {          
            FileStream mywriter = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryFormatter myBFwriter = new BinaryFormatter();
            myBFwriter.Serialize(mywriter,PrintObject_List);
            mywriter.Dispose();
        }

        public void TemplateLoad(string Path)
        {
            PrintObject_List.Clear();
            ObservableCollection<PrintObject_Base> PrintObject_Base_list_copy;
            FileStream myReader = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryFormatter myBFReader = new BinaryFormatter();
            PrintObject_Base_list_copy = (ObservableCollection<PrintObject_Base>)(myBFReader.Deserialize(myReader));
            myReader.Dispose();
            for(int i=0;i< PrintObject_Base_list_copy.Count;i++)
            {
                PrintObject_List.Add(PrintObject_Base_list_copy[i]);
            }          
        }

        public void BarcodeDelete(PrintObject_Base barcode_Base )
        {
            //if(PrintObject_List.Contains(barcode_Base))
            //{
            //    PrintObject_List.Remove(barcode_Base);
            //}
            PrintObject_List.RemoveAt(barcode_Base.Index);
        }
        public bool Print(string PrintName,string[] args)
        {
            SetPrintObjectData(args);
            using (Bitmap bmLabel = GetTotalBitmap())
            {
                if (null != bmLabel)
                {
                    using (PrintDocument pd = new PrintDocument())
                    {
                        pd.PrintPage += Pd_PrintPage;
                        pd.PrinterSettings.PrinterName = PrintName;
                        int xDPI = pd.PrinterSettings.DefaultPageSettings.PrinterResolution.X;
                        int yDPI = pd.PrinterSettings.DefaultPageSettings.PrinterResolution.Y;
                        bmLabel.SetResolution((float)xDPI, (float)yDPI);
                        _ImageTotal = bmLabel;
                        pd.Print();
                        pd.PrintPage -= Pd_PrintPage;
                    }
                    return true;
                }
            }
            return false;
        }

        public bool Print(string PrintName,Dictionary<string,string> PrintInfo)
        {
            SetPrintObjectData(PrintInfo);
            using (Bitmap bmLabel = GetTotalBitmap())
            {
                if (null != bmLabel)
                {
                    using (PrintDocument pd = new PrintDocument())
                    {
                        pd.PrintPage += Pd_PrintPage;
                        pd.PrinterSettings.PrinterName = PrintName;
                        int xDPI = pd.PrinterSettings.DefaultPageSettings.PrinterResolution.X;
                        int yDPI = pd.PrinterSettings.DefaultPageSettings.PrinterResolution.Y;
                        bmLabel.SetResolution((float)xDPI, (float)yDPI);
                        _ImageTotal = bmLabel;
                        pd.Print();
                        pd.PrintPage -= Pd_PrintPage;
                    }
                    return true;
                }
            }
            return false;
        }

        private void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(_ImageTotal, 0, 0, new Rectangle(0, 0, _ImageTotal.Width, _ImageTotal.Height), GraphicsUnit.Pixel);
        }

        private Bitmap GetTotalBitmap()
        {
            SizeF ImageTotalsize = GetTotalSize();
            using (Bitmap bmLabel = new Bitmap((int)ImageTotalsize.Width, (int)ImageTotalsize.Height))
            {
                using (Graphics gr = Graphics.FromImage(bmLabel))
                {
                    gr.Clear(System.Drawing.Color.White);
                    for (int i = 0; i < PrintObject_List.Count; i++)
                    {
                        gr.DrawImage(PrintObject_List[i].Paint(), (float)PrintObject_List[i].Location.X, (float)PrintObject_List[i].Location.Y,
                        new RectangleF(0, 0, (float)PrintObject_List[i].ImagepixelSize.Width, (float)PrintObject_List[i].ImagepixelSize.Height), GraphicsUnit.Pixel);
                    }
                }
                return new Bitmap(bmLabel);
            }
        }
        private SizeF GetTotalSize()
        {
            float tmpwidth = 0;
            float tmpheight = 0;
            for(int i=0;i<PrintObject_List.Count;i++)
            {
                PrintObject_Base p = PrintObject_List[i];
                if(p.Location.X+p.ImagepixelSize.Width>tmpwidth)
                {
                    tmpwidth =(float)p.Location.X + (float)p.ImagepixelSize.Width;
                }
                if (p.Location.Y + p.ImagepixelSize.Height > tmpheight)
                {
                    tmpheight = (float)p.Location.Y + (float)p.ImagepixelSize.Height;
                }
            }
            return new SizeF(tmpwidth+1, tmpheight+1);
        }
        private void SetPrintObjectData(params string[] args)
        {
            if(args.Count() >PrintObject_List.Count)
            {
                throw new Exception("Data number is too big");
            }
            for (int i = 0; i < args.Count(); i++)
            {
                PrintObject_Base p = PrintObject_List[i];
                p.Data = args[i];
            }
        }

        private void SetPrintObjectData(Dictionary<string, string> PrintInfo)
        {
            for (int i = 0; i < PrintObject_List.Count; i++)
            {
                PrintObject_Base p = PrintObject_List[i];
                if (p.Name != null)
                {
                    if (PrintInfo.Keys.Contains(p.Name))
                    {
                        p.Data = PrintInfo[p.Name];
                    }
                }
            }
        }
    }
}
