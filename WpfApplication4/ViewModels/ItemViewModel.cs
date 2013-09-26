using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;
using Telerik.Windows.Controls;
using WpfApplication4.Model;

namespace WpfApplication4.ViewModels
{
    public class ItemViewModel : ReactiveObject
    {
        private bool _IsEditing;

        public ItemViewModel()
        {
            Operations = new ObservableCollection<HyperCommand>();
        }

        public string Id { get; protected set; }

        public virtual string Name { get; set; }

        public string StatisticalWay { get; set; }
        public string Formula { get; set; }

        public bool IsEditing
        {
            get { return _IsEditing; }
            set
            {
                this.RaiseAndSetIfChanged(x => x.IsEditing, value);
            }
        }

        public ObservableCollection<HyperCommand> Operations { get; set; }

        public static ItemViewModel Create(ResponseEvaluationItem model)
        {
            if (model == null)
                return new UndefinedViewModel();
            if (string.IsNullOrEmpty(model.Id))
                return new NewItemViewModel { Name = "unnamed" };
            if (model.Status == "deleted")
            {
                return new DeletedViewModel { Id = model.Id, Name = model.Name, StatisticalWay = model.StatisticalWay };
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