using Grandsys.Wfm.Services.Outsource.ServiceModel;

namespace WpfApplication4.ViewModels
{
    public class SlideFormulaViewModel : FormulaViewModel
    {
        private double _finalIndicator;
        private double _startIndicator;
        private double _stepScore;

        public SlideFormulaViewModel(object model)
            : base(model)
        {
        }

        public double StepScore
        {
            get { return _stepScore; }
            set
            {
                _stepScore = value;
                WriteToRequestFormula();
            }
        }

        public double StartIndicator
        {
            get { return _startIndicator; }
            set
            {
                _startIndicator = value;
                WriteToRequestFormula();
            }
        }

        public double FinalIndicator
        {
            get { return _finalIndicator; }
            set
            {
                _finalIndicator = value;
                WriteToRequestFormula();
            }
        }

        public override void WriteToRequestFormula()
        {
            if (TryGetRequest == null) return;

            UpdateEvaluationItem request = TryGetRequest();
            if (request != null)
            {
                request.Formula = new FormulaInfo
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
}