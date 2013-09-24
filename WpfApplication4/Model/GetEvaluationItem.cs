using ServiceStack.ServiceHost;

namespace WpfApplication4.Model
{
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