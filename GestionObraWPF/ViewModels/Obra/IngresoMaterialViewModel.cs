using GestionObraWPF.DTOs;
using GestionObraWPF.Helpers;
using GestionObraWPF.Model;
using GestionObraWPF.Servicios;
using GestionObraWPF.ViewModels.ABMs;
using GestionObraWPF.ViewModels.EventAgreggator;
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
    public class IngresoMaterialViewModel : BaseABMViewModel
    {
        private IEventAggregator eventAggregator;
        private ObraDto _obra;
        private IngresoMaterialDto _ingresoMaterial;
        private ObservableCollection<IngresoMaterialDto> _ingresoMateriales;

        public IngresoMaterialDto IngresoMaterial { get { return _ingresoMaterial; } set { SetProperty(ref _ingresoMaterial, value); } }

        public ObservableCollection<IngresoMaterialDto> IngresoMateriales { get { return _ingresoMateriales; } set { SetProperty(ref _ingresoMateriales, value); } }

        public ObraDto Obra { get { return _obra; } set { SetProperty(ref _obra, value); } }


        public ICommand EjecutarMaterialCommand { get; set; }
        public ICommand EditarUsadosCommand { get; set; }
        public ICommand CargarUsadosCommand { get; set; }
        private Visibility _visible = Visibility.Visible;
        public Visibility Visible { get { return _visible; } set { SetProperty(ref _visible, value); } }

        public int CantidadVieja { get; private set; }

        public override async Task Inicializar()
        {
            IngresoMateriales = new ObservableCollection<IngresoMaterialDto>(await ApiProcessor.GetApi<IngresoMaterialDto[]>($"IngresoMaterial/GetByObra/{Obra.Id}"));
        }
        public IngresoMaterialViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<PubSubEvent<IngresoMaterialDto>>().Subscribe(ObtenerIngreso);
            eventAggregator.GetEvent<ObraAgreggator>().Subscribe(ObtenerObra);
            eventAggregator.GetEvent<PubSubEvent<Visibility>>().Subscribe(Ocultar);
            EditarUsadosCommand = new DelegateCommand(EditarMaterialesUsados, ()=> ObjetoNull.IsNull(IngresoMaterial)).ObservesProperty(() => IngresoMaterial);
            CargarUsadosCommand = new DelegateCommand(MaterialesUsados, ()=> ObjetoNull.IsNull(IngresoMaterial)).ObservesProperty(() => IngresoMaterial);
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, ()=> ObjetoNull.IsNull(IngresoMaterial)).ObservesProperty(() => IngresoMaterial);
            EliminarObraCommand = new DelegateCommand(Eliminar, ()=> ObjetoNull.IsNull(IngresoMaterial)).ObservesProperty(() => IngresoMaterial);
        }

        private void Ocultar(Visibility obj)
        {
            Visible = obj;
        }

        private  void EditarMaterialesUsados()
        {
            eventAggregator.GetEvent<PubSubEvent<int>>().Subscribe(EditarCantidadUsado);
            eventAggregator.GetEvent<IngresoMaterialAggregator>().Publish(IngresoMaterial);
            eventAggregator.GetEvent<BulAgreggator>().Publish(true);
        }

        private async void CantidadUsado(int obj)
        {
            eventAggregator.GetEvent<PubSubEvent<bool>>().Publish(false);
            IngresoMaterial.CantidadDevuelta += obj;
            await ApiProcessor.PutApi(IngresoMaterial, $"IngresoMaterial/{IngresoMaterial.Id}");
            StockHelper.AgregarStock(IngresoMaterial.MaterialId, obj);
            eventAggregator.GetEvent<PubSubEvent<int>>().Unsubscribe(CantidadUsado);
            await Inicializar();
        }

        private void MaterialesUsados()
        {
            eventAggregator.GetEvent<PubSubEvent<int>>().Subscribe(CantidadUsado);
            eventAggregator.GetEvent<IngresoMaterialAggregator>().Publish(IngresoMaterial);
            eventAggregator.GetEvent<PubSubEvent<bool>>().Publish(true);
        }

        private async void EditarCantidadUsado(int obj)
        {
            eventAggregator.GetEvent<BulAgreggator>().Publish(false);
            IngresoMaterial.CantidadDevuelta -= obj;
            await ApiProcessor.PutApi(IngresoMaterial, $"IngresoMaterial/{IngresoMaterial.Id}");
            IngresoMaterial = null;
            eventAggregator.GetEvent<PubSubEvent<int>>().Unsubscribe(EditarCantidadUsado);
            await Inicializar();
        }


        protected async override Task CrearNuevoElemento()
        {
            if (await StockHelper.ConsultarStock(IngresoMaterial.Material.Id, IngresoMaterial.Cantidad))
            {
                IngresoMaterial.Obra = Obra;
                IngresoMaterial.ObraId = Obra.Id;
                IngresoMaterial.MaterialId = IngresoMaterial.Material.Id;
                IngresoMaterial.Encargado = Obra.Encargado;
                IngresoMaterial.EncargadoId = Obra.EncargadoId;
                await ApiProcessor.PostApi(IngresoMaterial, "IngresoMaterial/Insert");
                eventAggregator.GetEvent<PubSubEvent<PopUp>>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
                StockHelper.QuitarStock(IngresoMaterial.MaterialId, IngresoMaterial.Cantidad);
                await Inicializar();
                IngresoMaterial = null;
                IngresoMaterial = new IngresoMaterialDto();
            }
            else
            {
                MessageBox.Show("Insumo sin stock");
            }
        }

        protected async override Task EliminarElemento()
        {
            eventAggregator.GetEvent< PubSubEvent<PopUp>>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
            await ApiProcessor.DeleteApi($"IngresoMaterial/{IngresoMaterial.Id}");
            StockHelper.AgregarStock(IngresoMaterial.MaterialId, IngresoMaterial.Cantidad);
            IngresoMaterial = null;
            await Inicializar();
        }

        protected async override Task EditarElemento()
        {
            eventAggregator.GetEvent<PubSubEvent<PopUp>>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
            if (IngresoMaterial.CantidadDevuelta < IngresoMaterial.Cantidad)
            {
                IngresoMaterial.Cantidad = IngresoMaterial.CantidadDevuelta;
            }
            if (CantidadVieja < IngresoMaterial.Cantidad)
            {
                StockHelper.QuitarStock(IngresoMaterial.MaterialId, IngresoMaterial.Cantidad- CantidadVieja);
            }
            else if (CantidadVieja > IngresoMaterial.Cantidad)
            {
                StockHelper.AgregarStock(IngresoMaterial.MaterialId, IngresoMaterial.Cantidad - CantidadVieja);
            }
            await ApiProcessor.PutApi(IngresoMaterial, $"IngresoMaterial/{IngresoMaterial.Id}");
            IngresoMaterial = null;
            await Inicializar();
        }

        private void ObtenerIngreso(IngresoMaterialDto obj)
        {
            IngresoMaterial = obj;
            eventAggregator.GetEvent<PubSubEvent<int>>().Unsubscribe(EditarCantidadUsado);
            eventAggregator.GetEvent<PubSubEvent<int>>().Unsubscribe(CantidadUsado);
            if (IngresoMaterial != null)
            {
                BotonABM();
            }
        }

        private async void ObtenerObra(ObraDto obj)
        {
            Obra = obj;
           await Inicializar();
        }

        protected override void Editar()
        {
            eventAggregator.GetEvent<IngresoMaterialAggregator>().Publish(IngresoMaterial);
            CantidadVieja=IngresoMaterial.Cantidad;
            base.Editar();
            eventAggregator.GetEvent<PubSubEvent<PopUp>>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
        }

        protected override void Eliminar()
        {
            eventAggregator.GetEvent<IngresoMaterialAggregator>().Publish(IngresoMaterial);
            base.Eliminar();
            eventAggregator.GetEvent<PubSubEvent<PopUp>>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));

        }

        protected override void Nuevo()
        {
            base.Nuevo();
            IngresoMaterial = new IngresoMaterialDto();
            eventAggregator.GetEvent<PubSubEvent<PopUp>>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
        }

        protected override void Cancelar()
        {
            base.Cancelar();
            IngresoMaterial = null;
            eventAggregator.GetEvent<PubSubEvent<PopUp>>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
        }

    }
}
