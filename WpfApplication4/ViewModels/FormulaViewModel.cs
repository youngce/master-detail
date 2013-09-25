using System;
using System.Linq;
using System.Reflection;
using Grandsys.Wfm.Services.Outsource.ServiceModel;

namespace WpfApplication4.ViewModels
{
    public abstract class FormulaViewModel
    {
        private double _baseIndicator;
        private double _baseScore;
        private double _scale;

        protected FormulaViewModel(object model)
        {
            ToProperties(model, this);
        }

        public double BaseIndicator
        {
            get { return _baseIndicator; }
            set
            {
                _baseIndicator = value;
                WriteToRequestFormula();
            }
        }

        public double BaseScore
        {
            get { return _baseScore; }
            set
            {
                _baseScore = value;
                WriteToRequestFormula();
            }
        }

        public double Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                WriteToRequestFormula();
            }
        }

        public string Name { get; set; }

        public Func<UpdateEvaluationItem> TryGetRequest { get; set; }

        private void ToProperties(object source, object target)
        {
            if (source == null) return;

            var sourceProps = source.GetType()
                .GetProperties()
                .ToDictionary(o => o.Name, o => o);
            var targetProps = target.GetType().GetProperties();
            foreach (var targetProp in targetProps)
            {
                PropertyInfo sourceProp;
                if (sourceProps.TryGetValue(targetProp.Name, out sourceProp))
                {
                    var value = sourceProp.GetValue(source, null);
                    targetProp.SetValue(this, value, null);
                }
            }
        }

        public abstract void WriteToRequestFormula();
    }

    public class UnsupportFormulaViewModel : FormulaViewModel
    {
        public UnsupportFormulaViewModel() : base(null)
        {
        }

        public override void WriteToRequestFormula()
        {
        }
    }
}