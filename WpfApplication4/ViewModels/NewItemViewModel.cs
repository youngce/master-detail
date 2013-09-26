using System;
using System.Linq;
using ReactiveUI;

namespace WpfApplication4.ViewModels
{
    public class NewItemViewModel : ItemViewModel
    {
        private string _Name;

        public NewItemViewModel()
        {
            IsEditing = true;
        }

        public override string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                this.RaiseAndSetIfChanged(x => x.Name, value);
            }
        }
    }
}