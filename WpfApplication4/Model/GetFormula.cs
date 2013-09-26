using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grandsys.Wfm.Services.Outsource.ServiceModel;
using ServiceStack.ServiceHost;

namespace WpfApplication4.Model
{
    public class GetFormula:IReturn<FormulaInfo>
    {
        public GetFormula(string id)
        {
            Id = id;

        }

        public string Id { get; set; }
        
    }
}
