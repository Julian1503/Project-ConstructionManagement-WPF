using GestionObraWPF.Views.ViewControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionObraWPF.Helpers
{
    public static class Excel
    {
        public static void ExportToExcelAndCsv(DataGrid dgDisplay, string nombre = "")
        {
            string ruta = "C:\\ReportesGonelectProgram";
            try
            {
                if (!(Directory.Exists(ruta)))
                {
                    Directory.CreateDirectory(ruta);
                }
            }
            catch (Exception errorC)
            {
                MessageBox.Show("Ha habido un error al intentar " +
                         "crear el fichero temporal:" +
                         Environment.NewLine + Environment.NewLine +
                         ruta + Environment.NewLine +
                         Environment.NewLine + errorC.Message,
                         "Error al crear fichero temporal");
            }

            try
            {
                dgDisplay.SelectAllCells();
                dgDisplay.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                ApplicationCommands.Copy.Execute(null, dgDisplay);
                String resultat = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
                String result = (string)Clipboard.GetData(DataFormats.Text);
                dgDisplay.UnselectAllCells();
                Mensaje ms = new Mensaje();
                ms.ShowDialog();
                if (ms.Acepto)
                {
                    System.IO.StreamWriter file1 = new StreamWriter($@"C:\ReportesGonelectProgram\{ms.Result}.xml");
                    file1.WriteLine(result.Replace(',', ' '));
                    file1.Close();
                    MessageBox.Show("Archivo creado correctamente");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Hubo un error con la exportacion");
            }
        }
    }
}
