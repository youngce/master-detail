using System;
using Grandsys.Wfm.Services.Outsource.ServiceModel;

namespace WpfApplication4.ViewModels
{
    public class SlideFormulaViewModel : FormulaViewModel
    {
        private double _finalIndicator;
        private double _stepScore;
        private double _startIndicator;

        public SlideFormulaViewModel(object model)
            : base(model)
        {
        }

        public double StepScore
        {
            get { return this._stepScore; }
            set
            {
                this._stepScore = value;
                WriteToRequestFormula();
            }
        }

        public double StartIndicator
        {
            get { return this._startIndicator; }
            set
            {
                this._startIndicator = value;
                WriteToRequestFormula();
            }
        }

        public double FinalIndicator
        {
            get { return this._finalIndicator; }
            set
            {
                this._finalIndicator = value;
                WriteToRequestFormula();
            }
        }

        public override string Name { get { return "Slide"; } }

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
                    StepScore = StepScore,
                    StartIndicator = StartIndicator,
                    FinalIndicator = FinalIndicator
                };
            }
        }
    }
}