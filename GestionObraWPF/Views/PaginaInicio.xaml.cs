using GestionObraWPF.ViewModels;
using LiveCharts.Wpf;
using System;
using System.Drawing;
using System.Windows.Controls;

namespace GestionObraWPF.Views
{
    /// <summary>
    /// Interaction logic for PaginaInicio
    /// </summary>
    public partial class PaginaInicio : UserControl
    {
        public PaginaInicio()
        {
            InitializeComponent();
        }
        protected async override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            await ((PaginaInicioViewModel)this.DataContext).Inicializar();
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
    }
}
