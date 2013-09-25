using Grandsys.Wfm.Services.Outsource.ServiceModel;

namespace WpfApplication4.ViewModels
{
    public class LinearFormulaViewModel : FormulaViewModel
    {
        private double _decreaseStepScore;
        private double _increaseStepScore;

        public LinearFormulaViewModel(object model)
            : base(model)
        {
        }

        public double IncreaseStepScore
        {
            get { return _increaseStepScore; }
            set
            {
                _increaseStepScore = value;
                WriteToRequestFormula();
            }
        }

        public double DecreaseStepScore
        {
            get { return _decreaseStepScore; }
            set
            {
                _decreaseStepScore = value;
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
                    IncreaseStepScore = IncreaseStepScore,
                    DecreaseStepScore = DecreaseStepScore
                };
            }
        }
    }
}