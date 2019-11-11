using Prism.Mvvm;
using System.Windows.Input;

namespace GestionObraWPF.ViewModels
{
    public class BaseViewModel : BindableBase
    {
        private bool imBuzy;
        private Cursor cursor;

        public bool ImBuzy
        {
            get { return imBuzy; }
            set
            {
                SetProperty(ref imBuzy, value);
                if (imBuzy)
                {
                     this.Cursor = Cursors.Wait;
                }
                else
                {
                    this.Cursor = Cursors.Arrow;
                }
            }
        }

        public Cursor Cursor
        {
            get { return cursor; }
            set
            {
                SetProperty(ref cursor, value);
            }
        }
    }
}