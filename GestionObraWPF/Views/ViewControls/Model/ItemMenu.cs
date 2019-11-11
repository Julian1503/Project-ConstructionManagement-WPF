using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GestionObraWPF.Views.ViewControls.Model
{
    public class ItemMenu
    {
        public ItemMenu(string header, List<SubItem> subItems, PackIconKind icon,bool enabled)
        {
            Header = header;
            SubItems = subItems;
            Icon = icon;
            Enabled = enabled;
        }

        public string Header { get; private set; }
        public bool Enabled { get; private set; }
        public PackIconKind Icon { get; private set; }
        public List<SubItem> SubItems { get; private set; }
    }
}
