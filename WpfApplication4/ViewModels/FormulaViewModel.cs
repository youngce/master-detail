using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Grandsys.Wfm.Services.Outsource.ServiceModel;
using ReactiveUI;

namespace WpfApplication4.ViewModels
{
    public abstract class FormulaViewModel : ReactiveObject
    {
        private double _BaseIndicator;
        private double _BaseScore;
        private double _Scale;

        protected FormulaViewModel(object model)
        {
            ToProperties(model, this);
        }

        public double BaseIndicator
        {
            get { return _BaseIndicator; }
            set { this.RaiseAndSetIfChanged(x => x.BaseIndicator, value); }
        }

        public double BaseScore
        {
            get { return _BaseScore; }
            set { this.RaiseAndSetIfChanged(x => x.BaseScore, value); }
        }

        public double Scale { get { return _Scale; } set { this.RaiseAndSetIfChanged(x => x.Scale, value); } }

        public string Name { get; set; }

        public abstract FormulaInfo ToValue();

        private void ToProperties(object source, object target)
        {
            if (source == null) return;

            Dictionary<string, PropertyInfo> sourceProps = source.GetType()
                .GetProperties()
                .ToDictionary(o => o.Name, o => o);
            PropertyInfo[] targetProps = target.GetType().GetProperties();
            foreach (PropertyInfo targetProp in targetProps)
            {
                PropertyInfo sourceProp;
                if (sourceProps.TryGetValue(targetProp.Name, out sourceProp))
                {
                    object value = sourceProp.GetValue(source, null);
                    targetProp.SetValue(this, value, null);
                }
            }
        }
    }

    public class UnsupportFormulaViewModel : FormulaViewModel
    {
        public UnsupportFormulaViewModel() : base(null)
        {
        }

        public override FormulaInfo ToValue()
        {
            return null;
        }
    }
}