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

namespace GestionObraWPF.ViewModels
{
    public class SalidaMaterialViewModel : BaseABMViewModel
    {
        private ObraDto _obra;
        private ObservableCollection<SalidaMaterialDto> _salidaMateriales;
        private SalidaMaterialDto _salidaMaterial;
        private IEventAggregator eventAggregator;

        public ObraDto Obra { get { return _obra; } set { SetProperty(ref _obra, value); } }
        public ObservableCollection<SalidaMaterialDto> SalidaMateriales { get { return _salidaMateriales; } set { SetProperty(ref _salidaMateriales, value); } }
        public SalidaMaterialDto SalidaMaterial { get { return _salidaMaterial; } set { SetProperty(ref _salidaMaterial, value); } }
        public SalidaMaterialViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<ObraAgreggator>().Subscribe(ObtenerObra);
            eventAggregator.GetEvent<SalidaMaterialAgreggator>().Subscribe(ObtenerIngreso);
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, ()=> ObjetoNull.IsNull(SalidaMaterial)).ObservesProperty(() => SalidaMaterial);
            EliminarObraCommand = new DelegateCommand(Eliminar, ()=> ObjetoNull.IsNull(SalidaMaterial)).ObservesProperty(() => SalidaMaterial);
        }

        public async override Task Inicializar()
        {
            var contenedor = new ObservableCollection<SalidaMaterialDto>(await ApiProcessor.GetApi<SalidaMaterialDto[]>($"SalidaMaterial/GetByObra/{Obra.Id}"));
            //foreach (var item in contenedor)
            //{
            //    item.ParaObra = await ApiProcessor.GetApi<ObraDto>($"Obra/GetById/{item.ParaObraId}");
            //    item.Responsable = await ApiProcessor.GetApi<PersonaDto>($"Persona/GetById/{item.ResponsableId}");
            //    item.Material = await ApiProcessor.GetApi<MaterialDto>($"Material/GetById/{item.MaterialId}");
            //}
            SalidaMateriales = contenedor;
        }

        protected async override Task CrearNuevoElemento()
        {
            SalidaMaterial.DeObra = Obra;
            SalidaMaterial.DeObraId = Obra.Id;
            SalidaMaterial.ParaObraId = SalidaMaterial.ParaObra.Id;
            SalidaMaterial.MaterialId = SalidaMaterial.Material.Id;
            SalidaMaterial.EncargadoId = SalidaMaterial.Encargado.Id;
            await ApiProcessor.PostApi(SalidaMaterial, "SalidaMaterial/Insert");
            eventAggregator.GetEvent<PubSubEvent<PopUp>>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
            await Inicializar();
            SalidaMaterial = null;
            SalidaMaterial = new SalidaMaterialDto();
        }

        protected async override Task EliminarElemento()
        {
            eventAggregator.GetEvent<PubSubEvent<PopUp>>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
            await ApiProcessor.DeleteApi($"SalidaMaterial/{SalidaMaterial.Id}");
            SalidaMaterial = null;
            await Inicializar();
        }

        protected async override Task EditarElemento()
        {
            eventAggregator.GetEvent<PubSubEvent<PopUp>>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
            await ApiProcessor.PutApi(SalidaMaterial, $"SalidaMaterial/{SalidaMaterial.Id}");
            SalidaMaterial = null;
            await Inicializar();
        }

        private void ObtenerIngreso(SalidaMaterialDto obj)
        {
            SalidaMaterial = obj;
            if (SalidaMaterial != null)
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
            eventAggregator.GetEvent<PubSubEvent<SalidaMaterialDto>>().Publish(SalidaMaterial);
            base.Editar();
            eventAggregator.GetEvent<PubSubEvent<PopUp>>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
        }

        protected override void Eliminar()
        {
            eventAggregator.GetEvent<PubSubEvent<SalidaMaterialDto>>().Publish(SalidaMaterial);
            base.Eliminar();
            eventAggregator.GetEvent<PubSubEvent<PopUp>>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));

        }

        protected override void Nuevo()
        {
            base.Nuevo();
            SalidaMaterial = new SalidaMaterialDto();
            eventAggregator.GetEvent<PubSubEvent<PopUp>>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
        }

        protected override void Cancelar()
        {
            base.Cancelar();
            SalidaMaterial = null;
            eventAggregator.GetEvent<PubSubEvent<PopUp>>().Publish(new PopUp(btnDialogText, MostrarCrearObra, ControlesDialog));
        }

    }
}
