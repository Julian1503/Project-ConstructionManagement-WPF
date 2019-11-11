using GestionObraWPF.Helpers;
using GestionObraWPF.ViewModels;
using LiveCharts.Wpf;
using System;
using System.Windows.Controls;

namespace GestionObraWPF.Views.Reportes
{
    /// <summary>
    /// Interaction logic for ComprobanteEntrada
    /// </summary>
    public partial class ReporteComprobanteEntrada : UserControl
    {
        public ReporteComprobanteEntrada()
        {
            InitializeComponent();
        }

        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((ReporteComprobanteEntradaViewModel)this.DataContext).Inicializar();
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
            Excel.ExportToExcelAndCsv(grilla, "ComprobantesEntrada");
        }
    }
}
