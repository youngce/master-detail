using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace WpfApplication4.Model
{
    public class ResponseEvaluationItem
    {
        public ResponseEvaluationItem()
        {
            Links = new List<Grandsys.Wfm.Services.Outsource.ServiceModel.Link>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public string StatisticalWay { get; set; }

        public IEnumerable<Grandsys.Wfm.Services.Outsource.ServiceModel.Link> Links { get; set; }

        public IEnumerable<Grandsys.Wfm.Services.Outsource.ServiceModel.Link> SetFormulaOptions { get; set; }
        //public IEnumerable<Link> Links { get; set; }
        //public IEnumerable<Link> SetFormulaOptions { get; set; }
    }

    public class EvaluationItemTitle
    {
        public string Id { get; set; }
        public string Name { get; set; }

       
    }

    public class GetEvaluationItem : IReturn<ResponseEvaluationItem>
    {
        public GetEvaluationItem(string id)
        {
            Id = id;
            Mode = "read";
        }
        public string Id { get; private set; }
        public string Mode { get; private set; }
    }
}