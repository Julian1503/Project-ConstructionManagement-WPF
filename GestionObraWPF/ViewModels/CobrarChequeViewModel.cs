using GestionObraWPF.DTOs;
using GestionObraWPF.Views.ViewControls.Caja;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class CobrarChequeViewModel : BindableBase
    {
        private ObservableCollection<ChequeEntradaDto> _cheques;
        private ChequeEntradaDto _cheque;

        public CobrarChequeViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            CancelarCommand = new DelegateCommand(Cancelar);
            CrearCommand = new DelegateCommand(Crear);
            Cheque = new ChequeEntradaDto();
        }

        private async void Crear()
        {
            if (await Servicios.ApiProcessor.GetApi<bool>("Caja/CajasEstado"))
            {
                if (Cheque != null)
                {
                    await Servicios.ApiProcessor.PostApi(Cheque, "Cheque/Cobrar");
                    var Caja = await Servicios.ApiProcessor.GetApi<CajaDto>("Caja/CajaAbierta");
                    var detalleCaja = new DetalleCajaDto
                    {
                        CajaId = Caja.Id,
                        Monto = Cheque.Monto,
                        TipoMovimiento = Constantes.TipoMovimiento.Ingreso,
                        TipoPago = Constantes.TipoPago.Cheque
                    };
                }
                else
                {
                    MessageBox.Show("Por favor seleccione un cheque");
                }
            }
            else
            {
                MessageBox.Show("Por favor abra la caja");
            }
        }

        private void Cancelar()
        {
            Cheque = new ChequeEntradaDto();
            var diccionario = new Dictionary<string, bool>();
            diccionario.Add("ChequeEntrada", false);
            eventAggregator.GetEvent<PubSubEvent<Dictionary<string, bool>>>().Publish(diccionario);
        }

        public ObservableCollection<ChequeEntradaDto> Cheques { get { return _cheques; } set { SetProperty(ref _cheques, value); } }
        public ChequeEntradaDto Cheque { get { return _cheque; } set { SetProperty(ref _cheque, value); } }
        private IEventAggregator eventAggregator;
        public ICommand CancelarCommand { get; set;}
        public ICommand CrearCommand { get; set; }


        public async Task Initialize()
        {
            Cheques = new ObservableCollection<ChequeEntradaDto>(await Servicios.ApiProcessor.GetApi<ChequeEntradaDto[]>("ChequeEntrada/GetAll"));
        }

    }
}
