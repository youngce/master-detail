using Grandsys.Wfm.Services.Outsource.ServiceModel;

namespace WpfApplication4.ViewModels
{
    public class LinearFormulaViewModel : FormulaViewModel
    {
        private double _increaseStepScore;
        private double _decreaseStepScore;

        public LinearFormulaViewModel(object model)
            : base(model)
        {
        }

        public double IncreaseStepScore
        {
            get { return this._increaseStepScore; }
            set
            {
                this._increaseStepScore = value;
                WriteToRequestFormula();
            }
        }

        public double DecreaseStepScore
        {
            get { return this._decreaseStepScore; }
            set
            {
                this._decreaseStepScore = value;
                WriteToRequestFormula();
            }
        }

        public override string Name { get { return "Linear"; } }

        public override void WriteToRequestFormula()
        {
            if (TryGetRequest == null) return;
            var request = TryGetRequest();
            if (request != null)
            {
                request.Formula = new FormulaInfo()
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