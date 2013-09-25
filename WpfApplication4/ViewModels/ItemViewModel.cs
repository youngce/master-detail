using System.Collections.ObjectModel;
using System.Linq;
using Telerik.Windows.Controls;
using WpfApplication4.Model;

namespace WpfApplication4.ViewModels
{
    public class ItemViewModel : ViewModelBase
    {
        private bool _isEditing;

        public ItemViewModel()
        {
            Operations = new ObservableCollection<HyperCommand>();
        }

        public string Id { get; set; }

        public virtual string Name { get; set; }

        public string StatisticalWay { get; set; }
        public string Formula { get; set; }

        public bool IsEditing
        {
            get { return _isEditing; }
            set
            {
                _isEditing = value;
                OnPropertyChanged(() => IsEditing);
            }
        }

        public ObservableCollection<HyperCommand> Operations { get; set; }

        public static ItemViewModel Create(ResponseEvaluationItem model)
        {
            if (model == null)
                return new UndefinedViewModel();
            if (string.IsNullOrEmpty(model.Id))
                return new NewItemViewModel {Name = "unnamed"};
            if (model.Status == "deleted")
            {
                return new DeletedViewModel {Id = model.Id, Name = model.Name, StatisticalWay = model.StatisticalWay};
            }

            if (model.Links != null && model.Links.Any(o => o.Method == "PATCH"))
            {
                var vm = new ItemEditViewModel(model);

                return vm;
            }

            return new ItemViewModel
            {
                Id = model.Id,
                Name = model.Name,
                StatisticalWay = model.StatisticalWay,
                Formula = model.Formula
            };
        }

        public virtual void OperationAdded()
        {
        }
    }
}