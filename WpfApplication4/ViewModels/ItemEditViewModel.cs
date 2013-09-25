using System.Dynamic;
using Grandsys.Wfm.Services.Outsource.ServiceModel;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WpfApplication4.Model;

namespace WpfApplication4.ViewModels
{
    public class ItemEditViewModel : ItemViewModel
    {
        private string _name;
        public IEnumerable<FormulaViewModel> SetFormulaOptions { get; set; }

        public ItemEditViewModel(ResponseEvaluationItem model)
        {
            Id = model.Id;
            Name = model.Name;
            Formula = model.Formula;
            StatisticalWay = model.StatisticalWay;
            SetFormulaOptions = model.SetFormulaOptions.Select(o =>
            {
                FormulaViewModel vm;
                if (o.Name == "Linear")
                    vm = new LinearFormulaViewModel(o.Request) { TryGetRequest = GetRequest  };
                else if (o.Name == "Slide")
                    vm = new SlideFormulaViewModel(o.Request) { TryGetRequest = GetRequest };
                else
                    vm = new UnsupportFormulaViewModel();
                vm.Name = o.Name;
                return vm;
            }).ToList();

            IsEditing = true;
        }

        public override string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                WriteToRequest(request => { request.Name = value; });
            }
        }

        private FormulaViewModel _selectedFormula;
        public FormulaViewModel SelectedFormula
        {
            get
            {
                return _selectedFormula;
            }
            set
            {
                _selectedFormula = value;
                _selectedFormula.WriteToRequestFormula();
                OnPropertyChanged(() => SelectedFormula);
            }
        }


        public override void OperationAdded()
        {
            if (!string.IsNullOrEmpty(Formula))
            {
                SelectedFormula = SetFormulaOptions.FirstOrDefault(o => Formula.ToLower().Contains(o.Name.ToLower()));
            }

            var putOperation = Operations.FirstOrDefault(o => o.Method == "PATCH");
            if (putOperation != null)
            {
                _request = putOperation.Request as UpdateEvaluationItem;
            }
        }

        private UpdateEvaluationItem _request;

        private UpdateEvaluationItem GetRequest()
        {
            return _request;
        }

        private void WriteToRequest(Action<UpdateEvaluationItem> set)
        {
            if (_request != null)
            {
                set(_request);
            }
        }
    }
}