using System.Collections.ObjectModel;
using Telerik.Windows.Controls;
using System.Linq;

namespace WpfApplication4.ViewModels
{
    public class ItemViewModel : ViewModelBase
    {
        public static ItemViewModel Create(ResponseEvaluationItem model)
        {
            if (model == null)
                return new NewItemViewModel() { Name = "unnamed"};
            if (model.Status == "deleted")
            {
                return new DeletedViewModel() { Id = model.Id ,Name = model.Name, StatisticalWay = model.StatisticalWay};
            }

            if (model.Links.Any(o=> o.Method == "PUT"))
            {
                return new ItemEditViewModel()
                {
                    Id = model.Id,
                    Name = model.Name,
                    StatisticalWay = model.StatisticalWay,
                    SetFormulaOptions = model.SetFormulaOptions
                };
            }
          
            return new ItemViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                StatisticalWay = model.StatisticalWay
            };
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string StatisticalWay { get; set; }

        public ItemViewModel()
        {
            Operations = new ObservableCollection<HyperCommand>();
        }

        private bool _isEditing;

        public bool IsEditing
        {
            get
            {
                return _isEditing;
            }
            set
            {
                _isEditing = value; OnPropertyChanged(() => IsEditing);
            }
        }

        public ObservableCollection<HyperCommand> Operations { get; set; }
    }
}