using GestionObraWPF.DTOs;
using GestionObraWPF.Servicios;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class CuentaCorrienteViewModel : BindableBase
    {
        public CuentaCorrienteViewModel()
        {
            FiltrarCommand = new DelegateCommand(Filtrar);
        }

        private ObservableCollection<BancoDto> _bancos;
        private ObservableCollection<OperacionDto> _operaciones;
        private BancoDto _banco;
        private decimal _debe;
        private decimal _haber;
        private decimal _diferencia;
        private decimal _descontado;
        private DateTime _fechaDesde = DateTime.Now;
        private DateTime _fechaHasta = DateTime.Now;
        public DateTime FechaDesde { get { return _fechaDesde; } set { SetProperty(ref _fechaDesde, value); } }
        public DateTime FechaHasta { get { return _fechaHasta; } set { SetProperty(ref _fechaHasta, value); } }

        public ICommand FiltrarCommand { get; set; }
        public ObservableCollection<BancoDto> Bancos { get { return _bancos; } set { SetProperty(ref _bancos, value); } }
        public BancoDto Banco { get { return _banco; } set { SetProperty(ref _banco, value); } }
        public decimal Debe { get { return _debe; } set { SetProperty(ref _debe, value); } }
        public decimal Haber { get { return _haber; } set { SetProperty(ref _haber, value); } }
        public decimal Diferencia { get { return _diferencia; } set { SetProperty(ref _diferencia, value); } }
        public ObservableCollection<OperacionDto> Operaciones { get { return _operaciones; } set { SetProperty(ref _operaciones, value); } }

        public decimal Descontado { get { return _descontado; } set { SetProperty(ref _descontado, value); } }

        public async Task Inicializar()
        {
            Bancos = new ObservableCollection<BancoDto>(await ApiProcessor.GetApi<BancoDto[]>("Banco/GetAll"));
        }

        private async void Filtrar()
        {
            if (Banco!=null)
            {
                Operaciones = new ObservableCollection<OperacionDto>(await ApiProcessor.GetApi<OperacionDto[]>($"Operacion/GetByFecha/{FechaDesde.ToString("MM-dd-yyyy")}/{FechaHasta.ToString("MM-dd-yyyy")}/{Banco.Id}"));
                Descontado = (decimal)Operaciones.Where(x=>x.Descontado!=null).Sum(x => x.Descontado);
                Debe = (decimal)Operaciones.Where(x =>x.Debe!=null).Sum(x => x.Debe);
                Haber = (decimal)Operaciones.Where(x=>x.Haber != null).Sum(x => x.Haber);
                Diferencia = Debe - Haber - Descontado;
            }
        }
    }
}
