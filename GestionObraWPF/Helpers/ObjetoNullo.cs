using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionObraWPF.Helpers
{
   static public  class ObjetoNull
    {
        public static bool IsNull(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return true;
        }

        public static bool IsNull<T>(object obj)
        {
            if ((obj is IEnumerable<T> ))
            {
                if (((IEnumerable<T>)obj).Count() == 0)
                {
                    return false;
                }
                return true;
            }
            return true;
        }
    }
}
