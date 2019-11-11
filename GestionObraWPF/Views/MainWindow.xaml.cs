using GestionObraWPF.ViewModels;
using GestionObraWPF.Views.ViewControls;
using GestionObraWPF.Views.ViewControls.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GestionObraWPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //var menuObra = new List<SubItem>();
            //menuObra.Add(new SubItem("Ingreso materiales", new InicioView()));
            //menuObra.Add(new SubItem("Nadaa"));
            //menuObra.Add(new SubItem("Nada"));
            //var item = new ItemMenu("Obra", menuObra, MaterialDesignThemes.Wpf.PackIconKind.Highway);



            //  var menu = new List<SubItem>();
            //menu.Add(new SubItem("asd materiales", new IngresoMaterialControl()));
            //var item2 = new ItemMenu("Asdas", menu, MaterialDesignThemes.Wpf.PackIconKind.Highway);


            //Menu.Children.Add(new BotonDesplegable(item, this));
            //Menu.Children.Add(new BotonDesplegable(item2, this));
        }
        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        internal void SwitchScreen(object sender)
        {
            var screen = ((UserControl)sender);
            //if (screen != null)
            //{
            //    StackPanelMain.Children.Clear();
            //    StackPanelMain.Children.Add(screen);
            //}
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SwitchScreen(new ObraABM());
        }

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show(((TextBlock)e.Source).Text);
        }

        private void BtnMaximizar_Click(object sender, RoutedEventArgs e)
        {
            if(Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                IconoMax.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                IconoMax.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
        }

        private void BtnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
    }
}
