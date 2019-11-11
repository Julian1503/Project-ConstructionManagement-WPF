using GestionObraWPF.DTOs;
using GestionObraWPF.Helpers;
using GestionObraWPF.ViewModels.ABMs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GestionObraWPF.ViewModels
{
    public class PersonaABMViewModel : BaseABMViewModel
    {
        private PersonaDto _Persona;
        public PersonaDto Persona { get { return _Persona; } set { SetProperty(ref _Persona, value); } }

        private ObservableCollection<PersonaDto> _Personas;
        public ObservableCollection<PersonaDto> Personas { get { return _Personas; } set { SetProperty(ref _Personas, value); } }

        public override async Task Inicializar()
        {
            Personas = new ObservableCollection<PersonaDto>(await Servicios.ApiProcessor.GetApi<PersonaDto[]>("Persona/GetAll"));
        }
        protected async override Task CrearNuevoElemento()
        {
            if (!string.IsNullOrWhiteSpace(Persona.ApYNom) && !string.IsNullOrWhiteSpace(Persona.Dni) && (!string.IsNullOrWhiteSpace(Persona.Celular)|| !string.IsNullOrWhiteSpace(Persona.Telefono)) && Persona.FechaNacimiento!=null)
            {
                await Servicios.ApiProcessor.PostApi(Persona, "Persona/Insert");
                await Inicializar();
                Persona = new PersonaDto();
            }
        }
        protected async override Task EliminarElemento()
        {
            await Servicios.ApiProcessor.DeleteApi($"Persona/{Persona.Id}");
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            if (!string.IsNullOrWhiteSpace(Persona.ApYNom) && !string.IsNullOrWhiteSpace(Persona.Dni) && (!string.IsNullOrWhiteSpace(Persona.Celular) || !string.IsNullOrWhiteSpace(Persona.Telefono)) && Persona.FechaNacimiento != null)
            {
                await Servicios.ApiProcessor.PutApi(Persona, $"Persona/{Persona.Id}");
                await Inicializar();
            }
        }
        private async void Buscando()
        {
            if (string.IsNullOrWhiteSpace(Busqueda))
            {
                Personas = new ObservableCollection<PersonaDto>(await Servicios.ApiProcessor.GetApi<PersonaDto[]>("Persona/GetAll"));

            }
            else
            {
                Personas = new ObservableCollection<PersonaDto>(await Servicios.ApiProcessor.GetApi<PersonaDto[]>($"Persona/GetByFilter/{Busqueda}"));
            }
        }
        protected override void Nuevo()
        {
            base.Nuevo();
            Persona = new PersonaDto();
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            Persona = null;
        }
        public PersonaABMViewModel()
        {
            Persona = null;
            Buscar = new DelegateCommand(Buscando);
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, ()=> ObjetoNull.IsNull(Persona)).ObservesProperty(() => Persona);
            EliminarObraCommand = new DelegateCommand(Eliminar, ()=> ObjetoNull.IsNull(Persona)).ObservesProperty(() => Persona);
        }
    }
}
