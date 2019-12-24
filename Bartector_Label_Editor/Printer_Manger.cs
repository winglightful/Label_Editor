using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Printing;
using System.Drawing;
namespace Bartector_Label_Editor
{
     public static class Printer_Manger
    {
         
        public static List<string> GetPrinterNameList()
        {
            List<string> list = new List<string>();
            foreach (string printername in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                list.Add(printername);
            }
            return list;
        }

        public static bool CleanPrintQueue(string PrinterName)
        {
            try
            {
                LocalPrintServer _printServer = new LocalPrintServer(PrintSystemDesiredAccess.AdministratePrinter);
                PrintQueue printqueue = new PrintQueue(_printServer, PrinterName);
                printqueue.Refresh();
                if (printqueue.NumberOfJobs != 0)
                {
                    printqueue.Purge();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GetPrinterStaus(string PrinterName)
        {
            try
            {
                LocalPrintServer _printServer = new LocalPrintServer(PrintSystemDesiredAccess.AdministratePrinter);
                PrintQueue printqueue = new PrintQueue(_printServer, PrinterName);
                return printqueue.QueueStatus.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
