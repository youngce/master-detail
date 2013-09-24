using System.Collections.ObjectModel;
using Telerik.Windows.Controls;
using System.Linq;
using WpfApplication4.Model;
using System.Collections.Generic;
using System;

namespace WpfApplication4.ViewModels
{
    public class ItemViewModel : ViewModelBase
    {
        public static ItemViewModel Create(ResponseEvaluationItem model)
        {
            if (model == null)
                return new UndefinedViewModel();
            if (string.IsNullOrEmpty(model.Id))
                return new NewItemViewModel() { Name = "unnamed" };
            if (model.Status == "deleted")
            {
                return new DeletedViewModel() { Id = model.Id, Name = model.Name, StatisticalWay = model.StatisticalWay };
            }

            if (model.Links != null && model.Links.Any(o => o.Method == "PUT"))
            {
                var vm = new ItemEditViewModel()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Formula = model.Formula,
                    StatisticalWay = model.StatisticalWay,
                    SetFormulaOptions = model.SetFormulaOptions
                };
                return vm;
            }

            return new ItemViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                StatisticalWay = model.StatisticalWay,
                Formula = model.Formula
            };
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string StatisticalWay { get; set; }
        public string Formula { get; set; }

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

        public virtual void OperationAdded()
        {
           
        }

        public ObservableCollection<HyperCommand> Operations { get; set; }
    }
}