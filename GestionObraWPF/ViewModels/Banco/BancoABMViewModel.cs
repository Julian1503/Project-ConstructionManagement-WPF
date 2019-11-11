using GestionObraWPF.DTOs;
using GestionObraWPF.Helpers;
using GestionObraWPF.Servicios;
using GestionObraWPF.ViewModels.ABMs;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class BancoABMViewModel : BaseABMViewModel
    {
        private BancoDto _Banco;

        public BancoDto Banco { get { return _Banco; } set { SetProperty(ref _Banco, value); } }

        public override async Task Inicializar()
        {
            Bancos = new ObservableCollection<BancoDto>(await Servicios.ApiProcessor.GetApi<BancoDto[]>("Banco/GetAll"));
        }
        private ObservableCollection<BancoDto> _Bancos;
        public ObservableCollection<BancoDto> Bancos
        {
            get { return _Bancos; }
            set
            {
                SetProperty(ref _Bancos, value);
            }
        }
        protected async override Task CrearNuevoElemento()
        {
            if (!string.IsNullOrWhiteSpace(Banco.Descripcion))
            {
                var banco = await Servicios.ApiProcessor.PostApi(Banco, "Banco/Insert");
                if(long.TryParse(banco,out long bancoId))
                {
                    var ctaCte = new CuentaCorrienteDto
                    {
                        BancoId = bancoId,
                        EstaEliminado = false
                    };
                    await Servicios.ApiProcessor.PostApi(ctaCte, "CuentaCorriente/Insert");
                }
                await Inicializar();
                Banco = new BancoDto();
            }
        }
        protected async override Task EliminarElemento()
        {
            await Servicios.ApiProcessor.DeleteApi($"Banco/{Banco.Id}");
            await Inicializar();
        }
        protected async override Task EditarElemento()
        {
            if (!string.IsNullOrWhiteSpace(Banco.Descripcion))
            {
                await Servicios.ApiProcessor.PutApi(Banco, $"Banco/{Banco.Id}");
                await Inicializar();
            }
        }

        private async void Buscando()
        {
            if (string.IsNullOrWhiteSpace(Busqueda))
            {
                Bancos = new ObservableCollection<BancoDto>(await Servicios.ApiProcessor.GetApi<BancoDto[]>("Banco/GetAll"));

            }
            else
            {
                Bancos = new ObservableCollection<BancoDto>(await Servicios.ApiProcessor.GetApi<BancoDto[]>($"Banco/GetByFilter/{Busqueda}"));
            }
        }

        public ICommand BuscarImagenCommand { get; set; }
        protected override void Nuevo()
        {
            base.Nuevo();
            Banco = new BancoDto();
        }
        protected override void Cancelar()
        {
            base.Cancelar();
            Banco = null;
        }
        public BancoABMViewModel()
        {
            Banco = null;
            Buscar = new DelegateCommand(Buscando);
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            EditarObraCommand = new DelegateCommand(Editar, ()=> ObjetoNull.IsNull(Banco)).ObservesProperty(() => Banco);
            EliminarObraCommand = new DelegateCommand(Eliminar, ()=> ObjetoNull.IsNull(Banco)).ObservesProperty(() => Banco);
            BuscarImagenCommand = new DelegateCommand(BuscarImagen);
        }

        private void BuscarImagen()
        {
            Banco.Path = CloudImage.BuscarImagen();
        }
    }
}
