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
            Response = Command.RegisterAsyncFunction(_ => {

                Model.ResponseEvaluationItem response = null;
                if (string.IsNullOrEmpty(link.Method))
                    return response;

                switch (link.Method)
                {
                    default:
                        var current = _;
                        var sourceProps = current.GetType().GetProperties().ToDictionary(o => o.Name, o => o);
                        var props = link.Request.GetType().GetProperties();
                        foreach (var prop in props)
                        {
                            System.Reflection.PropertyInfo sourceProp;
                            if (sourceProps.TryGetValue(prop.Name, out sourceProp))
                            {
                                var value = sourceProp.GetValue(current, null);
                                prop.SetValue(link.Request, value, null);
                            }
                        }
                        response = client.Send<Model.ResponseEvaluationItem>(link.Method, link.Request.ToUrl(link.Method), link.Request);
                        break;
                    case "GET":
                        response = client.Send<Model.ResponseEvaluationItem>(link.Method, link.Request.ToUrl(link.Method), null);
                        break;
                }

                //if (response != null && string.IsNullOrEmpty(response.Name) && !string.IsNullOrEmpty(response.Id))
                //{
                //    response = client.Get(new Model.GetEvaluationItem(response.Id));
                //}

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