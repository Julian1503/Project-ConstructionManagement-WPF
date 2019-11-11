using GestionObraWPF.Helpers;
using GestionObraWPF.ViewModels;
using LiveCharts.Wpf;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.Reportes
{
    /// <summary>
    /// Interaction logic for ComprobanteSalida
    /// </summary>
    public partial class ReporteComprobanteSalida : UserControl
    {
        public ReporteComprobanteSalida()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
          await  ((ReporteComprobanteSalidaViewModel)this.DataContext).Inicializar();
        }
        private void PieChart_DataClick(object sender, LiveCharts.ChartPoint chartPoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartPoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartPoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Excel.ExportToExcelAndCsv(grilla, "ComprobantesSalida");
        }
    }
}
