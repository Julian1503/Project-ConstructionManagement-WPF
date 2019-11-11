using GestionObraWPF.DTOs;
using MaterialDesignThemes.Wpf.Transitions;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class CuentaCorrienteEntradaViewModel : BindableBase
    {
        public CuentaCorrienteEntradaViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            CancelarCommand = new DelegateCommand<Transitioner>(Cancelar);
            CerrarCommand = new DelegateCommand(Cancelar);
            CrearCommand = new DelegateCommand(Crear);
        }

        private async void Crear()
        {

        }

        private void Cancelar()
        {
            var diccionario = new Dictionary<string, bool>();
            diccionario.Add("CuentaCorrienteEntrada", false);
            eventAggregator.GetEvent<PubSubEvent<Dictionary<string, bool>>>().Publish(diccionario);
        }

        private void Cancelar(Transitioner transitioner)
        {
            transitioner.SelectedIndex = 0;
            var diccionario = new Dictionary<string, bool>();
            diccionario.Add("CuentaCorrienteEntrada", false);
            eventAggregator.GetEvent<PubSubEvent<Dictionary<string, bool>>>().Publish(diccionario);
        }

        public async Task Inicializar()
        {
                Bancos = new ObservableCollection<BancoDto>(await Servicios.ApiProcessor.GetApi<BancoDto[]>("Banco/GetAll"));
              Clientes = new ObservableCollection<EmpresaDto>(await Servicios.ApiProcessor.GetApi<EmpresaDto[]>("Empresa/GetAll"));
        }


        public OperacionDto Operacion { get { return _operacion; } set { SetProperty(ref _operacion, value); } }
        public ObservableCollection<BancoDto> Bancos{ get { return _bancos; } set { SetProperty(ref _bancos, value); } }
        public ObservableCollection<EmpresaDto> Clientes { get { return _clientes; } set { SetProperty(ref _clientes, value); } }
        private IEventAggregator eventAggregator;
        private ObservableCollection<BancoDto> _bancos;
        private ObservableCollection<EmpresaDto> _clientes;
        private OperacionDto _operacion;

        public ICommand CancelarCommand { get; set; }
        public ICommand CerrarCommand { get; }
        public ICommand CrearCommand { get; set; }
    }
}
