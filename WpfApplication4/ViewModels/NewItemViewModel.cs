using System;
using System.Linq;

namespace WpfApplication4.ViewModels
{
    public class NewItemViewModel : ItemViewModel
    {
        private string _name;

        public NewItemViewModel()
        {
            IsEditing = true;
        }

        public override string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(() => Name);
            }
        }

        public override void OperationAdded()
        {
            
                PropertyChanged += (sender, e) =>
                {
                    var value = this.GetType().GetProperty(e.PropertyName).GetValue(this, null);

                    foreach (var link in Operations.Where(o => !string.IsNullOrEmpty(o.Method) && o.Method != "GET"))
                    {
                        var propInfo = link.Request.GetType().GetProperty(e.PropertyName);
                        if (propInfo != null)
                        {
                            propInfo.SetValue(link.Request, value, null);
                        }
                    } 
                };
            


        }
    }
}