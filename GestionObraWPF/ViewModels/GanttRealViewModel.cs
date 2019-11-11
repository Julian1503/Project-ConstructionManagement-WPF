using GestionObraWPF.DTOs;
using GestionObraWPF.Servicios;
using GestionObraWPF.ViewModels.EventAgreggator;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GestionObraWPF.ViewModels
{
    public class GanttRealViewModel : BindableBase
    {
        private IEventAggregator eventAggregator;

        private ObraDto _obra;
        private ObservableCollection<TareaDto> _tareas;
        private double _from = 0;
        private double _to = 100;
        private ChartValues<GanttPoint> _values;
        private SeriesCollection _series;
        private string[] _labels;
        private Func<double, string> _formatter;

        public SeriesCollection Series { get { return _series; } set { SetProperty(ref _series, value); } }
        public Func<double, string> Formatter { get { return _formatter; } set { SetProperty(ref _formatter, value); } }
        public string[] Labels { get { return _labels; } set { SetProperty(ref _labels, value); } }

        public double From
        {
            get { return _from; }
            set
            {
                SetProperty(ref _from, value);
            }
        }

        public double To
        {
            get { return _to; }
            set
            {
                SetProperty(ref _to, value);
            }
        }
        private void ResetZoomOnClick(object sender, RoutedEventArgs e)
        {
            if (_values.Count > 0)
            {
                From = _values.First().StartPoint;
                To = _values.Last().EndPoint;
            }
        }

        public ObraDto Obra { get { return _obra; } set { SetProperty(ref _obra, value); } }
        public ObservableCollection<TareaDto> Tareas
        {
            get { return _tareas; }
            set
            {
                SetProperty(ref _tareas, value);
            }
        }
        public GanttRealViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<ObraAgreggator>().Subscribe(ObtenerObra);
            _values = new ChartValues<GanttPoint> { new GanttPoint() };
            Labels = new string[] { "" };
            Series = new SeriesCollection();
            Formatter = value => new DateTime((long)value).ToString("H:mm");
        }

        private async void ObtenerObra(ObraDto obj)
        {
            Obra = obj;
            await Inicializar();
        }

        public async Task Inicializar()
        {
            if (Obra != null)
            {
                Tareas = new ObservableCollection<TareaDto>(await ApiProcessor.GetApi<TareaDto[]>($"Tarea/GetByObra/{Obra.Id}"));
                var now = DateTime.Now;
                _values = new ChartValues<GanttPoint>();
                if (Tareas.Count > 0)
                {
                    var labels = new List<string>();
                    foreach (var item in Tareas)
                    {
                        labels.Add($"{item.NumeroOrden}: {item.DescripcionTarea.Descripcion}");
                        if (_values.Count > 0)
                        {
                            var ultimo = _values.Last();
                            if (!item.Precede)
                            {
                                _values.Add(new GanttPoint(ultimo.StartPoint, ultimo.StartPoint + item.TiempoEmpleado.Ticks));
                            }
                            else
                            {
                                _values.Add(new GanttPoint(ultimo.EndPoint, ultimo.EndPoint + item.TiempoEmpleado.Ticks));
                            }
                        }
                        else
                        {
                            _values.Add(new GanttPoint(new TimeSpan(0, 0, 0).Ticks, item.TiempoEmpleado.Ticks));
                        }
                    }
                    Labels = labels.ToArray();
                    Series = new SeriesCollection
                     {
                new RowSeries
                       {
                    Values = _values,
                    DataLabels = true
                                 }
                   };
                }

                Formatter = value => new DateTime((long)value).ToString("H:mm");

                ResetZoomOnClick(null, null);
            }
        }
    }
}
