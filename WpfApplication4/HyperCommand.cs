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

                Model.ResponseEvaluationItem response = null;
                if (string.IsNullOrEmpty(Method))
                    return response;

                switch (Method)
                {
                    default:
                        var current = _;
                        var sourceProps = current.GetType().GetProperties().ToDictionary(o => o.Name, o => o);
                        var props = Request.GetType().GetProperties();
                        foreach (var prop in props)
                        {
                            System.Reflection.PropertyInfo sourceProp;
                            if (sourceProps.TryGetValue(prop.Name, out sourceProp))
                            {
                                var value = sourceProp.GetValue(current, null);
                                prop.SetValue(Request, value, null);
                            }
                        }
                        response = client.Send<Model.ResponseEvaluationItem>(Method, Request.ToUrl(Method), Request);
                        break;
                    case "GET":
                        response = client.Send<Model.ResponseEvaluationItem>(Method, Request.ToUrl(Method), null);
                        break;
                }
                return response;
            });

        }

        public IObservable<Model.ResponseEvaluationItem> Response { get; private set; }

        public string Content { get; set; }

        public ReactiveAsyncCommand Command { get; set; }

        public string Method { get; set; }

        public IReturn Request { get; set; }
    }
}