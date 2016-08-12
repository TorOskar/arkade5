using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Arkivverket.Arkade.UI.MVVM_ViewModel
{
    public class ViewModel : ObservableObjects
    {

        private string _textString;
        private string _infoString;


        public string TextString
        {
            get { return _textString; }
            set
            {
                _textString = value;
                OnPropertyChanged("TextString");
            }
        }

        public string InfoString
        {
            get { return _infoString; }
            set
            {
                _infoString = value;
                OnPropertyChanged("InfoString");
            }
        }


        public ICommand DoSomething
        {
            get { return new DelegateCommand(DoSomethingImplemented);}
        }

        private void DoSomethingImplemented()
        {
            InfoString = TextString.ToUpper();
            TextString = string.Empty;
        }
    }
}
