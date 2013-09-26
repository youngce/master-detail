using Grandsys.Wfm.Services.Outsource.ServiceModel;
using ReactiveUI;

namespace WpfApplication4.ViewModels
{
    public class SlideFormulaViewModel : FormulaViewModel
    {
        private double _FinalIndicator;
        private double _StartIndicator;
        private double _StepScore;

        public SlideFormulaViewModel(object model)
            : base(model)
        {
        }

        public double StepScore
        {
            get { return _StepScore; }
            set { this.RaiseAndSetIfChanged(x => x.StepScore, value); }
        }

        public double StartIndicator
        {
            get { return _StartIndicator; }
            set { this.RaiseAndSetIfChanged(x => x.StartIndicator, value); }
        }

        public double FinalIndicator
        {
            get { return _FinalIndicator; }
            set { this.RaiseAndSetIfChanged(x => x.FinalIndicator, value); }
        }

        public override FormulaInfo ToValue()
        {
            return new FormulaInfo
            {
                Type = Name,
                BaseIndicator = BaseIndicator,
                BaseScore = BaseScore,
                Scale = Scale,
                StepScore = StepScore,
                StartIndicator = StartIndicator,
                FinalIndicator = FinalIndicator
            };
        }
    }
}