using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bartector_Label_Editor
{
    /// <summary>
    /// LabelControl.xaml 的互動邏輯
    /// </summary>
    public partial class LabelControl : System.Windows.Controls.UserControl
    {

        #region "Property"
        /// <summary>
        /// Manger of printobjects
        /// </summary>
        PrintObject_Action Labels = new PrintObject_Action();
        PrintObject_Base printObject_Base;
        double CanvasScale = 1.0;
        public string PrinterName { get; set; }
        #endregion
        public LabelControl()
        {
            InitializeComponent();
        }
        #region "UI Action"
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            MyCanvas.Children.Clear();
            MyCanvas.Children.Add(Labels);
            Labels.BarcodeMouseHitEvent -= MouseHit;
            Labels.BarcodeMouseHitEvent += MouseHit;
        }

        private void ButtonNewCode_Click(object sender, RoutedEventArgs e)
        {
            PrintBarcode_Base barcode_Base = new PrintBarcode_Base()
            {
                BarcodeSymbology = ZintNet.Symbology.Code128,
                Width = 1f,
                Height = 5f,
                Data = "ABCDEFG",
                Location = new Point(10, 10)
            };
            Labels.PrintObject_List.Add(barcode_Base);
        }

        private void ButtonNewText_Click(object sender, RoutedEventArgs e)
        {
            PrintText_Base text_Base = new PrintText_Base()
            {
                Data = "AAA",
                Location = new Point(10, 10)
            };
            Labels.PrintObject_List.Add(text_Base);
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Labels.BarcodeDelete(printObject_Base);
            printObject_Base = null;
            MypropertyGrid.SelectedObject = null;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            ModelSave();
        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            ModelLoad();
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            Labels.PrintObject_List.Clear();
        }

        private void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {
            if(PrinterName!=null)
            {
                Dictionary<string, string> pairs = new Dictionary<string, string>();
                Print(PrinterName, pairs);
            }
        }

        private void ButtonPrintQueueClear_Click(object sender, RoutedEventArgs e)
        {
            if (PrinterName != null)
            {
                Printer_Manger.CleanPrintQueue(PrinterName);
            }
        }

        private void ButtonPrinterStaus_Click(object sender, RoutedEventArgs e)
        {
            if (PrinterName != null)
            {
                MessageBox.Show(Printer_Manger.GetPrinterStaus(PrinterName));
            }
        }

        private void ButtonPrinterSelect_Click(object sender, RoutedEventArgs e)
        {
            Form_PrinterSelect f = new Form_PrinterSelect(Printer_Manger.GetPrinterNameList());
            var result = f.ShowDialog();
            if(result== false)
            {
                PrinterName = f.PrinterName;
            }
        }
        /// <summary>
        /// Action when MouseHit event occur
        /// </summary>
        /// <param name="b">
        /// recive PrintObject binding to propertygrid
        /// </param>
        private void MouseHit(PrintObject_Base b)
        {
            printObject_Base = b;
            MypropertyGrid.SelectedObject  = printObject_Base;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ComboBox comboBox)) return;
            TransformGroup tg = MyCanvas.RenderTransform as TransformGroup;
            if (tg == null)
                tg = new TransformGroup();

            double scalefactor = 0;
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    scalefactor = 0.2 / CanvasScale;
                    tg.Children.Add(new ScaleTransform(scalefactor, scalefactor, 0, 0));
                    CanvasScale = 0.2;
                    break;
                case 1:
                    scalefactor = 0.5 / CanvasScale;
                    tg.Children.Add(new ScaleTransform(scalefactor, scalefactor, 0, 0));
                    CanvasScale = 0.5;
                    break;
                case 2:
                    scalefactor = 1.0 / CanvasScale;
                    tg.Children.Add(new ScaleTransform(scalefactor, scalefactor, 0, 0));
                    CanvasScale = 1.0;
                    break;
                case 3:
                    scalefactor = 1.25 / CanvasScale;
                    tg.Children.Add(new ScaleTransform(scalefactor, scalefactor, 0, 0));
                    CanvasScale = 1.25;
                    break;
                default:
                    break;
            }
            MyCanvas.RenderTransform = tg;
        }
        #endregion

        #region "Functions"
        /// <summary>
        /// Use FolderBrowserDialog to Save Print Model
        /// </summary>
        public void ModelSave()
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.FileName = "NewFile.tmp";
            saveFileDialog.Filter = "*.tmp|";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFileDialog.FileName!="")
            {
                Labels.TemplateSave(saveFileDialog.FileName);
            }
        }

        /// <summary>
        /// Save Print Model by giving absolute path
        /// </summary>
        /// <param name="Path"></param>
        public void ModelSave(string Path)
        {
            Labels.TemplateSave(Path);
        }

        /// <summary>
        /// Use FolderBrowserDialog to Load Print Model
        /// </summary>
        public void ModelLoad()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "*.tmp|";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK && openFileDialog.FileName != "")
            {
                Labels.TemplateLoad(openFileDialog.FileName);
            }
        }

        /// <summary>
        /// Load Print Model by giving absolute path
        /// </summary>
        /// <param name="Path"></param>
        public void ModelLoad(string Path)
        {
            Labels.TemplateLoad(Path);
        }

        /// <summary>
        /// Print by give PrintInfo
        /// </summary>
        /// <param name="PrinterName"></param>
        /// <param name="PrintInfo">
        /// string1: PrintObject Name
        /// string2: Data want to change
        /// </param>
        public void Print(string PrinterName, Dictionary<string,string> PrintInfo)
        {
            Labels.Print(PrinterName, PrintInfo);
        }

        /// <summary>
        /// Print by give strings
        /// </summary>
        /// <param name="PrinterName"></param>
        /// <param name="args">
        /// PrintObject Data will be change by Index
        /// </param>
        public void Print(string PrinterName,params string[] args)
        {
            Labels.Print(PrinterName, args);
        }
        #endregion

      

        
    }   
}
