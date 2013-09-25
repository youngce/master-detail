using System;
using System.Windows.Input;
using ServiceStack.ServiceHost;
using Telerik.Windows.Controls;
using ReactiveUI.Xaml;
using ServiceStack.ServiceClient.Web;
using System.Linq;


namespace WpfApplication4
{
    public class HyperCommand
    {
        public HyperCommand(JsonServiceClient client ,Grandsys.Wfm.Services.Outsource.ServiceModel.Link link)
        {
            Content = link.Name;
            Command = new ReactiveAsyncCommand();
            Method = link.Method;
            Request = link.Request;
            Response = Command.RegisterAsyncFunction(_ => {

                Model.ResponseEvaluationItem response;
                if (string.IsNullOrEmpty(Method))
                    return null;

                switch (Method)
                {
                    default:
                        response = client.Send<Model.ResponseEvaluationItem>(Method, Request.ToUrl(Method), Request);
                        break;
                    case "GET":
                        response = client.Get<Model.ResponseEvaluationItem>(Request.ToUrl(Method));
                        break;
                }
                return response;
            });
            Command.ThrownExceptions.Subscribe(ex => Console.WriteLine(ex.Message));
        }

        public IObservable<Model.ResponseEvaluationItem> Response { get; private set; }

        public string Content { get; set; }

        public ReactiveAsyncCommand Command { get; set; }

        public string Method { get; set; }

        public IReturn Request { get; set; }
    }
}