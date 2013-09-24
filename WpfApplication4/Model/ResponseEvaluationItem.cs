using System.Collections.Generic;
using Grandsys.Wfm.Services.Outsource.ServiceModel;

namespace WpfApplication4.Model
{
    public class ResponseEvaluationItem
    {
        public ResponseEvaluationItem()
        {
            this.Links = new List<Link>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public string StatisticalWay { get; set; }

        public string Formula { get; set; }


        public IEnumerable<Link> Links { get; set; }

        public IEnumerable<Link> SetFormulaOptions { get; set; }
    }
}