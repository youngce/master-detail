using System.Collections.Generic;

namespace WpfApplication4.Model
{
    public class ResponseEvaluationItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public string StatisticalWay { get; set; }

        public IEnumerable<Grandsys.Wfm.Services.Outsource.ServiceModel.Link> Links { get; set; }

        public IEnumerable<Grandsys.Wfm.Services.Outsource.ServiceModel.Link> SetFormulaOptions { get; set; }
        //public IEnumerable<Link> Links { get; set; }
        //public IEnumerable<Link> SetFormulaOptions { get; set; }
    }
}