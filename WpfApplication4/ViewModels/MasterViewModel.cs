using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
//using Grandsys.Wfm.Services.Outsource.ServiceModel;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceHost;
using Telerik.Windows.Controls;
using System.Linq;

namespace WpfApplication4.ViewModels
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


    //public class Link
    //{
    //    public string Name { get; set; }
    //    public string Method { get; set; }
    //    public IReturn<ResponseEvaluationItem> Request { get; set; }
    //}

    public partial class MasterViewModel
    {
        class EvaluationItemTitle
        {
            public string Id { get; set; }
            public string Name { get; set; }

            public class EvaluationItems : IReturn<List<EvaluationItemTitle>> { }
        }

        class ReadOnlyEvaluationItem
        {
            public class GetEvaluationItem : IReturn<ReadOnlyEvaluationItem>
            {
                public GetEvaluationItem(string id)
                {
                    Id = id;
                    Mode = "read";
                }
                public string Id { get; private set; }
                public string Mode { get; private set; }
            }


            public string Name { get; set; }
            public string StatisticalWay { get; set; }
            public IEnumerable<Grandsys.Wfm.Services.Outsource.ServiceModel.Link> Links { get; set; }
        }


    }

    public partial class MasterViewModel : ViewModelBase
    {

        private JsonServiceClient _client;

        public MasterViewModel()
        {
            ActivedItem = new UndefinedViewModel();

            _client = new JsonServiceClient("http://localhost:7284");
            
            Items = new ObservableCollection<object>();
            GetAll = new DelegateCommand(_ =>
                {
                    ActivedItem = new UndefinedViewModel();
                    Items.Clear();
                    foreach (var item in _client.Get(new EvaluationItemTitle.EvaluationItems()))
                        Items.Add(item);

                });
            New = new DelegateCommand(_ =>
                {
                    SelectedItem = null;
                    var vm = ItemViewModel.Create(null);
                    ActivedItem = vm;
                    var rt = _client.Get(new Grandsys.Wfm.Services.Outsource.ServiceModel.EvaluationItemsCreationWays());
                    AddOperations(rt.Links, vm);
                    vm.Operations.Add(new HyperCommand(__ => { ActivedItem = new UndefinedViewModel(); }) { Content = "Discard" });
                });
        }

        public ICommand GetAll { get; set; }
        public ICommand New { get; set; }

        public ObservableCollection<object> Items { get; private set; }

        private void TurnToReadOnlyMode(object _)
        {
            if (_selectedItem == null)
            {
                //ActivedItem = new UndefinedViewModel();
                return;
            }

            dynamic evaluationItem = _selectedItem;

            if (ActivedItem != null && evaluationItem.Id == ActivedItem.Id)
                return;

            var response = _client.Get(new ReadOnlyEvaluationItem.GetEvaluationItem(evaluationItem.Id));

            var vm = new ItemViewModel { Name = response.Name, StatisticalWay = response.StatisticalWay, Id = evaluationItem.Id };
            AddOperations(response.Links, vm);

            ActivedItem = vm;
        }

        private void AddOperations(IEnumerable<Grandsys.Wfm.Services.Outsource.ServiceModel.Link> links, ItemViewModel vm)
        {
            if (links == null)
                return;

            foreach (var link in links)
            {
                var request = link.Request;
                var verb = link.Method;
                vm.Operations.Add(new HyperCommand(__ =>
                {
                    ResponseEvaluationItem rt;
                    if (verb == "GET")
                    {
                        rt = _client.Send<ResponseEvaluationItem>(verb, request.ToUrl(verb), null);
                    }
                    else
                    {
                        var current = ActivedItem;
                        var sourceProps = current.GetType().GetProperties().ToDictionary(o => o.Name, o => o);
                        var props = request.GetType().GetProperties();
                        foreach (var prop in props)
                        {
                            System.Reflection.PropertyInfo sourceProp;
                            if (sourceProps.TryGetValue(prop.Name, out sourceProp))
                            {
                                var value = sourceProp.GetValue(current, null);
                                prop.SetValue(request, value, null);
                            }
                        }
                        rt = _client.Send<ResponseEvaluationItem>(verb, request.ToUrl(verb), request);
                    }

                    var newVm = ItemViewModel.Create(rt);
                    
                    AddOperations(rt.Links, newVm);
                    ActivedItem = newVm;
                   
                    if (verb == "POST")
                    {
                        Items.Insert(0, new EvaluationItemTitle() { Name = rt.Name, Id = rt.Id });
                        SelectedItem = Items[0];
                    }
                    if (verb == "DELETE")
                        Items.Remove(SelectedItem);
                    if (verb == "PUT")
                    {
                        Items.Insert(0, new EvaluationItemTitle() { Name = rt.Name, Id = rt.Id });
                        SelectedItem = Items[0];
                    }

                }) { Content = link.Name });
            }
        }

        private object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_activedItem == null || !_activedItem.IsEditing)
                {
                    _selectedItem = value;
                    TurnToReadOnlyMode(null);
                }

                Application.Current.Dispatcher.BeginInvoke(new Action(() => { OnPropertyChanged(() => SelectedItem); }), DispatcherPriority.ContextIdle);
            }
        }

        private ItemViewModel _activedItem;
        public ItemViewModel ActivedItem
        {
            get { return _activedItem; }
            set
            {
                _activedItem = value;

                OnPropertyChanged(() => ActivedItem);
            }
        }
    }
}