using GestionObraWPF.DTOs;
using GestionObraWPF.Servicios;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class ObraViewViewModel : BindableBase
    {
        private ObraDto _obraDTo;
        private ObservableCollection<ObraDto> _obras;
        private Visibility _sinObra = Visibility.Collapsed;

        public ObraDto Obra
        {
            get { return _obraDTo; }
            set
            {
                _obraDTo = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand<ObraDto> Command { get; set; }

        private IEventAggregator eventAggregator;

        public IRegionManager regionManager { get; set; }
        public ObraViewViewModel(IRegionManager regionManager,IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.regionManager = regionManager;
            Command = new DelegateCommand<ObraDto>(DobleClick);
        }

        private void DobleClick(ObraDto obj)
        {
            if (obj != null )
            {
                var nav = new NavigationParameters();
                nav.Add("Obra", obj);
                regionManager.RequestNavigate("Contenido", "ObraDetalle", nav);
                eventAggregator.GetEvent<PubSubEvent<Visibility>>().Publish(Visibility.Visible);
            }
        }

        public async Task Inicialize()
        {
            Obras = new ObservableCollection<ObraDto>(await ApiProcessor.GetApi<ObraDto[]>("Obra/GetAllP"));
        }

        public ObservableCollection<ObraDto> Obras
        {
            get { return _obras; }
            set
            {
                SetProperty(ref _obras, value);
                if (Obras.Count == 0)
                {
                    SinObras = Visibility.Visible;
                }
                else
                {
                    SinObras = Visibility.Collapsed;
                }
            }
        }

        public Visibility SinObras {
            get { return _sinObra; }
            set
            {
                SetProperty(ref _sinObra, value);
            }
        }
    }
}
