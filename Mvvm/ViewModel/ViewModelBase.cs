using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Mvvm
{
    public class ViewModelBase : INotifyPropertyChanged
    {

        #region NoitifyPropetyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifityPropetyChanged(string PropertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
        #endregion
    }
}
