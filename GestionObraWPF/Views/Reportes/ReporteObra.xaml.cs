using GestionObraWPF.Helpers;
using GestionObraWPF.ViewModels;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace GestionObraWPF.Views.Reportes
{
    /// <summary>
    /// Interaction logic for ReporteObra
    /// </summary>
    public partial class ReporteObra : UserControl
    {
        public ReporteObra()
        {
            InitializeComponent();
        }

        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((ReporteObraViewModel)this.DataContext).Inicializar();
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
            var lista = new List<DataGrid>();
            lista.Add(Obra0);
            lista.Add(Obra1);
            lista.Add(Obra2);
            lista.Add(Obra3);
            Excel.ExportToExcelAndCsv(lista,"");
            lista.Clear();
        }
    }
}
