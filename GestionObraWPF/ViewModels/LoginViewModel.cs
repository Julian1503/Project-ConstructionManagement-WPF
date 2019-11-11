using GestionObraWPF.Helpers;
using GestionObraWPF.Model;
using GestionObraWPF.Servicios;
using GestionObraWPF.Views;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _title = "Login";
        private string _usuario ;
        private string _password ;
        private bool _error;
        public bool Error
        {
            get { return _error; }
            set { SetProperty(ref _error, value); }
        }
        public string Usuario
        {
            get { return _usuario; }
            set { SetProperty(ref _usuario, value); }
        }
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DelegateCommand<Window> LoginCommand { get; set; }

        private IRegionManager regionManager;

        public bool Login { get; internal set; }

        private async void Ingresar(Window frmLogin)
        {
            if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Vacio");
                return;     
            }
            try
            {
                ImBuzy = true;
                var encrip = Encriptar.Encriptador(Password).Replace('/',' ');

                var usuario = await ApiProcessor.GetApi<Model.UsuarioDto>($"Usuario/Login/{Usuario}/{encrip}");
                if (usuario != null)
                {
                    WebServices.HttpClient.DefaultRequestHeaders.Authorization =
                     new AuthenticationHeaderValue("Bearer", usuario.Token);
                    UsuarioGral.UsuarioId = usuario.Id;
                    UsuarioGral.Identificacion.Configuracion = usuario.Identificacion.Configuracion;
                    UsuarioGral.Identificacion.Obra = usuario.Identificacion.Obra;
                    UsuarioGral.Identificacion.Tesoreria= usuario.Identificacion.Tesoreria;
                    UsuarioGral.Identificacion.Administracion = usuario.Identificacion.Administracion;
                    UsuarioGral.Identificacion.Usuarios = usuario.Identificacion.Usuarios;
                    regionManager.RequestNavigate("Contenido", "PantallaPrincipal");
                    frmLogin.Close();
                    Login = true;
                }
                else
                {
                    Error = true;
                }
            }
            catch (Exception)
            {
                Error = true;
            }
            ImBuzy =false;
        }

        public LoginViewModel(IRegionManager regionManager)
        {
            LoginCommand = new DelegateCommand<Window>(Ingresar);
            this.regionManager = regionManager;
        }

    }
}
