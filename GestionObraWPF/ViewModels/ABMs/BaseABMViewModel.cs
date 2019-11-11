using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels.ABMs
{
    public  abstract class BaseABMViewModel : BindableBase
    {

        

        public string Busqueda { get { return _busqueda; } set { SetProperty(ref _busqueda, value); } }
        public ICommand Buscar { get; set; }

        public BaseABMViewModel()
        {
            CrearObraCommand = new DelegateCommand(Nuevo);
            CancelarCommand = new DelegateCommand(Cancelar);
            CrearCommand = new DelegateCommand(BotonABM);
        }

        protected async void BotonABM()
        {
            try
            {
                switch (btnDialogText)
                {
                    case "Editar":
                        MostrarCrearObra = false;
                        await EditarElemento();
                        break;

                    case "Eliminar":
                        MostrarCrearObra = false;
                        await EliminarElemento();
                        break;

                    case "Crear":
                        await CrearNuevoElemento();
                        break;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Error de conexion");
            }
        }

        protected virtual async Task EditarElemento()
        {
        }

        protected virtual async Task EliminarElemento()
        {
        }

        protected virtual async Task CrearNuevoElemento()
        {
        }

        private string _dialogText;
        public string btnDialogText
        {
            get { return _dialogText; }
            set { SetProperty(ref _dialogText, value); }
        }

        public virtual async Task Inicializar()
        {

        }
        public ICommand CrearCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        
        private bool _mostrarCrearObra;
        public bool MostrarCrearObra
        {
            get { return _mostrarCrearObra; }
            set
            {
                _mostrarCrearObra = value;
                RaisePropertyChanged();
            }
        }
        private bool _controlesDialog = true;
        private string _busqueda;

        public bool ControlesDialog
        {
            get { return _controlesDialog; }
            set
            {
                _controlesDialog = value;
                RaisePropertyChanged();
            }
        }
        
        public ICommand CrearObraCommand { get; set; }
        public ICommand EditarObraCommand { get; set; }
        public ICommand EliminarObraCommand { get; set; }

        protected virtual void Eliminar()
        {
            MostrarCrearObra = true;
            btnDialogText = "Eliminar";
            ControlesDialog = false;
        }

        protected async virtual void Cancelar()
        {
            MostrarCrearObra = false;
           await Inicializar();
        }

        protected virtual void Editar()
        {
            MostrarCrearObra = true;
            btnDialogText = "Editar";
            ControlesDialog = true;
        }

        protected virtual void Nuevo()
        {
            MostrarCrearObra = true;
            btnDialogText = "Crear";
            ControlesDialog = true;
        }
    }
}
