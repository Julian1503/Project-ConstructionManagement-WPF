using GestionObraWPF.DTOs;
using GestionObraWPF.Model;
using GestionObraWPF.Servicios;
using GestionObraWPF.ViewModels.EventAgreggator;
using GestionObraWPF.Views.ViewControls.Obra;
using LiveCharts;
using LiveCharts.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Exportable.Engines;
using Exportable.Engines.Excel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using GestionObraWPF.Views.ViewControls;
using System.Windows;

namespace GestionObraWPF.ViewModels
{
    public class ReporteObraViewModel : BindableBase
    {
        IExcelExportEngine engine;
        MemoryStream memory;
        public ReporteObraViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            Export = new DelegateCommand(Exportar);
            FiltrarCommand = new DelegateCommand(Filtrar);
        }

        private void Exportar()
        {
            memory = new MemoryStream();
            engine = new ExcelExportEngine();
            engine.SetFormat(ExcelVersion.XLS);
            if (ComprobantesCompra != null)
            {
                engine.AddData(ComprobantesCompra.ToList(),"ComprobantesDeCompra");
            }
            if (Jornales != null)
            {
                engine.AddData(Jornales.ToList(), "Jornales");
            }
            if (Tareas!=null)
            {
                engine.AddData(Tareas.ToList(), "Tareas");
            }
            if (Utilitarios != null)
            {
                engine.AddData(Utilitarios.ToList(), "Utilitarios");
            }
            if (Materiales != null)
            {
            engine.AddData(Materiales.ToList(), "Materiales");
            }
            Mensaje m = new Mensaje();
            m.ShowDialog();
            var texto = "";
            if (m.Acepto)
            {
                try
                {
                    texto = $@"C:\ReportesGonelectProgram\{m.Result}.xls";
                    engine.Export(texto);
                }
                catch (Exception)
                {
                    MessageBox.Show($"Error al exportar el archivo {texto}");
                }
            }
        }

        private ObservableCollection<ObraDto> _bancos;
        private ObservableCollection<JornalDto> _operaciones;
        private ObraDto _obra;
        private ObservableCollection<JornalMaterialDto> _materiales;
        private decimal _otro;
        private decimal _viatico;
        private decimal _gasolina;
        private decimal _repuesto;
        private decimal _multa;
        private decimal _total;
        private ObservableCollection<IngresoMaterialDto> _utilitarios;
        private SeriesCollection _series;
        private ObservableCollection<TareaDto> _tareas;
        private IEventAggregator eventAggregator;
        private ObservableCollection<ComprobanteCompraDto> _comprobanteCompra;
        private decimal _compras;

        public ICommand FiltrarCommand { get; set; }
        public ICommand Export { get; set; }
        public DelegateCommand<ChartPoint> Click { get; set; }
        public ObservableCollection<ObraDto> Obras { get { return _bancos; } set { SetProperty(ref _bancos, value); } }
        public ObraDto Obra { get { return _obra; } set { SetProperty(ref _obra, value); } }
        public decimal Otro { get { return _otro; } set { SetProperty(ref _otro, value); } }

        public decimal Compras { get { return _compras; } set { SetProperty(ref _compras, value); } }

        public decimal Viatico { get { return _viatico; } set { SetProperty(ref _viatico, value); } }
        public decimal Gasolina { get { return _gasolina; } set { SetProperty(ref _gasolina, value); } }
        public decimal Repuesto { get { return _repuesto; } set { SetProperty(ref _repuesto, value); } }
        public decimal Multa { get { return _multa; } set { SetProperty(ref _multa, value); } }

        public decimal Total { get { return _total; } set { SetProperty(ref _total, value); } }

        public ObservableCollection<JornalDto> Jornales { get { return _operaciones; } set { SetProperty(ref _operaciones, value); } }

        public ObservableCollection<ComprobanteCompraDto> ComprobantesCompra { get { return _comprobanteCompra; } set { SetProperty(ref _comprobanteCompra, value); } }

        public ObservableCollection<TareaDto> Tareas { get { return _tareas; } set { SetProperty(ref _tareas, value); } }

        public ObservableCollection<JornalMaterialDto> Materiales { get { return _materiales; } set { SetProperty(ref _materiales, value); } }
        public ObservableCollection<IngresoMaterialDto> Utilitarios { get { return _utilitarios; } set { SetProperty(ref _utilitarios, value); } }
        public Func<ChartPoint, string> PointLabel { get; set; }
        public SeriesCollection Series { get { return _series; } set { SetProperty(ref _series, value); } }
        public async Task Inicializar()
        {
            Obras = new ObservableCollection<ObraDto>(await ApiProcessor.GetApi<ObraDto[]>($"Obra/GetAll"));
            PointLabel = chartPoint =>
              string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            Series = new SeriesCollection();
        }

        private async void Filtrar()
        {
            if (Obra != null)
            {
                Series.Clear();
                Jornales = new ObservableCollection<JornalDto>(await ApiProcessor.GetApi<JornalDto[]>($"Jornal/GetByObra/{Obra.Id}"));
                ComprobantesCompra = new ObservableCollection<ComprobanteCompraDto>(await ApiProcessor.GetApi<ComprobanteCompraDto[]>($"ComprobanteCompra/GetByObra/{Obra.Id}"));
                eventAggregator.GetEvent<ObraAgreggator>().Publish(Obra);
                Tareas = new ObservableCollection<TareaDto>(await ApiProcessor.GetApi<TareaDto[]>($"Tarea/GetByObra/{Obra.Id}"));
                Otro = Jornales.Sum(x => x.Otros);
                Compras = ComprobantesCompra.Sum(x => x.Total);
                Gasolina = Jornales.Sum(x => x.Gasolina);
                Repuesto = Jornales.Sum(x => x.Repuestos);
                Viatico = Jornales.Sum(x => x.Viatico);
                Multa = Jornales.Sum(x => x.Multas);
                Total = Multa + Viatico + Repuesto + Gasolina + Otro + Compras;
                Materiales = new ObservableCollection<JornalMaterialDto>(await ApiProcessor.GetApi<JornalMaterialDto[]>($"JornalMaterial/GetByObra/{Obra.Id}"));
                Utilitarios = new ObservableCollection<IngresoMaterialDto>(await ApiProcessor.GetApi<IngresoMaterialDto[]>($"IngresoMaterial/GetByObra/{Obra.Id}"));
                Series.Add(new PieSeries { Title="Gasolina",  Values = new ChartValues<decimal>(new decimal[] {Gasolina}) ,DataLabels=true,LabelPoint = PointLabel});
                Series.Add(new PieSeries { Title = "Repuesto", Values = new ChartValues<decimal>(new decimal[] { Repuesto }), DataLabels = true, LabelPoint = PointLabel });
                Series.Add(new PieSeries { Title = "Multa", Values = new ChartValues<decimal>(new decimal[] { Multa }), DataLabels = true, LabelPoint = PointLabel });
                Series.Add(new PieSeries { Title = "Viatico", Values = new ChartValues<decimal>(new decimal[] { Viatico }), DataLabels = true, LabelPoint = PointLabel });
                Series.Add(new PieSeries { Title = "Compras", Values = new ChartValues<decimal>(new decimal[] { Compras }), DataLabels = true, LabelPoint = PointLabel });
                Series.Add(new PieSeries { Title = "Otro", Values = new ChartValues<decimal>(new decimal[] { Otro }), DataLabels = true, LabelPoint = PointLabel });
            }
        }
    }
}
