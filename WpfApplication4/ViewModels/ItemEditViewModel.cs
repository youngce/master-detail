using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Grandsys.Wfm.Services.Outsource.ServiceModel;
using ReactiveUI;
using WpfApplication4.Model;

namespace WpfApplication4.ViewModels
{
    public class ItemEditViewModel : ItemViewModel
    {
        private readonly FormulaViewModel _SelectedFormula;
        private readonly UpdateEvaluationItem _updateValues;
        private string _name;

        public ItemEditViewModel(ResponseEvaluationItem model)
        {
            Id = model.Id;
            _name = model.Name;
            StatisticalWay = model.StatisticalWay;

            var obsvr = Observer.Create<FormulaInfo>(o =>
            {
                if (_updateValues == null)
                    return;
                _updateValues.Formula = o;
            });


            SetFormulaOptions = model.SetFormulaOptions.Select(o =>
            {
                FormulaViewModel vm;
                if (o.Name == "Linear")
                    vm = new LinearFormulaViewModel(o.Request);
                else if (o.Name == "Slide")
                    vm = new SlideFormulaViewModel(o.Request);
                else
                    vm = new UnsupportFormulaViewModel();
                vm.Name = o.Name;

                Observable.FromEventPattern<PropertyChangedEventArgs>(vm, "PropertyChanged").Select(args => args.Sender)
                    .OfType<FormulaViewModel>().Select(v => v.ToValue())
                    .Subscribe(obsvr);

                return vm;
            }).ToList();

            if (!string.IsNullOrEmpty(model.Formula))
            {
                _SelectedFormula = SetFormulaOptions.FirstOrDefault(o => model.Formula.ToLower().Contains(o.Name.ToLower()));
            }

            this.WhenAny(x => x.SelectedFormula, x => x.Value)
                .Select(o => o == null ? null : o.ToValue())
                .Subscribe(obsvr);

            IsEditing = true;
            _updateValues = new UpdateEvaluationItem();
        }

        public IEnumerable<FormulaViewModel> SetFormulaOptions { get; set; }

        public UpdateEvaluationItem UpdateValues { get { return _updateValues; } }

        public override string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                WriteToRequest(updateValues => { updateValues.Name = value; });
            }
        }

        public FormulaViewModel SelectedFormula
        {
            get { return _SelectedFormula; }
            set { this.RaiseAndSetIfChanged(x => x.SelectedFormula, value); }
        }

        private void WriteToRequest(Action<UpdateEvaluationItem> set)
        {
            if (UpdateValues != null)
                set(UpdateValues);
        }
    }
}