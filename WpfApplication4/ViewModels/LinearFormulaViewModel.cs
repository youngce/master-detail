using Grandsys.Wfm.Services.Outsource.ServiceModel;
using ReactiveUI;

namespace WpfApplication4.ViewModels
{
    public class LinearFormulaViewModel : FormulaViewModel
    {
        private double _DecreaseStepScore;
        private double _IncreaseStepScore;

        public LinearFormulaViewModel(object model)
            : base(model)
        {
        }

        public double IncreaseStepScore
        {
            get { return _IncreaseStepScore; }
            set { this.RaiseAndSetIfChanged(x => x.IncreaseStepScore, value); }
        }

        public double DecreaseStepScore
        {
            get { return _DecreaseStepScore; }
            set { this.RaiseAndSetIfChanged(x => x.DecreaseStepScore, value); }
        }

        public override FormulaInfo ToValue()
        {
            return new FormulaInfo
            {
                Type = Name,
                BaseIndicator = BaseIndicator,
                BaseScore = BaseScore,
                Scale = Scale,
                IncreaseStepScore = IncreaseStepScore,
                DecreaseStepScore = DecreaseStepScore
            };
        }
    }
}