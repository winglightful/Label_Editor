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
    /// Form_PrinterSelect.xaml 的互動邏輯
    /// </summary>
    public partial class Form_PrinterSelect
    {
        public Form_PrinterSelect(List<string> list)
        {
            InitializeComponent();          
            PrinterList.ItemsSource = list;
        }
        public string PrinterName { get; set; }
        private void PrinterList_Selected(object sender, RoutedEventArgs e)
        {
            SelectItem.Text = PrinterList.SelectedItem.ToString();
            PrinterName= PrinterList.SelectedItem.ToString();
        }

        private void ButtonComform_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
      
    }
}
