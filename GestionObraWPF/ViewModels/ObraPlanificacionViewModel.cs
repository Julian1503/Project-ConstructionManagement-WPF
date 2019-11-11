using GestionObraWPF.DTOs;
using GestionObraWPF.Servicios;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GestionObraWPF.ViewModels
{
    public class ObraPlanificacionViewModel : BindableBase
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
        public ObraPlanificacionViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.regionManager = regionManager;
            Command = new DelegateCommand<ObraDto>(DobleClick);
        }

        private void DobleClick(ObraDto obj)
        {
            if (obj != null)
            {
                var nav = new NavigationParameters();
                nav.Add("ObraParametro", obj);
                regionManager.RequestNavigate("Contenido", "ObraPlanificacionDetalle", nav);
                eventAggregator.GetEvent<PubSubEvent<Visibility>>().Publish(Visibility.Collapsed);
            }
        }

        public async Task Inicialize()
        {
            Obras = new ObservableCollection<ObraDto>(await ApiProcessor.GetApi<ObraDto[]>("Obra/GetPlanificando"));
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

        public Visibility SinObras
        {
            get { return _sinObra; }
            set
            {
                SetProperty(ref _sinObra, value);
            }
        }
    }
}
