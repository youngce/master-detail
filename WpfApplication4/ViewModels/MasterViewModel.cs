using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceHost;
using Telerik.Windows.Controls;
using System.Linq;
using WpfApplication4.Model;
using System.Reactive.Subjects;
using ReactiveUI;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using ReactiveUI.Xaml;
using System.Reactive;

namespace WpfApplication4.ViewModels
{
    public partial class MasterViewModel
    {
        class EvaluationItems : IReturn<List<EvaluationItemTitle>> { }
    }

    public partial class MasterViewModel : ReactiveObject
    {

        private JsonServiceClient _client;
        private ReactiveUI.ObservableAsPropertyHelper<ItemViewModel> _CurrentViewModel;

        public ItemViewModel CurrentViewModel { get { return _CurrentViewModel.Value; } }

        public MasterViewModel()
        {
            var source = new BehaviorSubject<ItemViewModel>(new UndefinedViewModel());
            source.ToProperty(this, x => x.CurrentViewModel);

            _client = new JsonServiceClient("http://localhost:29315");
            GetAll = new ReactiveAsyncCommand();
            New = new ReactiveAsyncCommand();

            IObserver<ResponseEvaluationItem> obsvr = null;

            obsvr = Observer.Create<ResponseEvaluationItem>(o =>
            {
                var vm = ItemViewModel.Create(o);

                if (o != null && o.Links != null)
                {
                    foreach (var link in o.Links)
                    {
                        var cmd = new HyperCommand(_client, link);
                        cmd.Response.Subscribe(obsvr);
                        vm.Operations.Add(cmd);
                    }
                }
                source.OnNext(vm);
            });


            var selectedItemChanged = this.WhenAny(x => x.SelectedItem, x => x.Value);

            var getEvaluationItem = selectedItemChanged
                                        .Where(x => x != null)
                                        .ObserveOn(Scheduler.Default)
                                        .Select(o => _client.Get(new GetEvaluationItem(o.Id)));

            getEvaluationItem.Subscribe(obsvr);

            var howToNew = New.RegisterAsyncFunction(_ =>
            {
                var response = _client.Get(new Grandsys.Wfm.Services.Outsource.ServiceModel.EvaluationItemsCreationWays());
                var links = new List<Grandsys.Wfm.Services.Outsource.ServiceModel.Link>(response.Links);

                links.Add(new Grandsys.Wfm.Services.Outsource.ServiceModel.Link() { Name = "Discard" });
                return new ResponseEvaluationItem() { Links = links };
            });

            howToNew.Subscribe(obsvr);

            GetAll.RegisterAsyncFunction(_ =>
            {
                return _client.Get(new EvaluationItems()).ToObservable().CreateCollection();
            }).ToProperty(this, x => x.Items);

            //GetAll.AsyncCompletedNotification.Subscribe(_ => { source.OnNext(new UndefinedViewModel()); });

            source.OfType<UndefinedViewModel>().Subscribe(o => { SelectedItem = null; });
            source.OfType<DeletedViewModel>().Throttle(TimeSpan.FromSeconds(1)).Subscribe(o =>
            {
                GetAll.Execute(null);
            });
            source.OfType<NewItemViewModel>().Throttle(TimeSpan.FromSeconds(1)).Subscribe(o =>
            {
                GetAll.Execute(null);
            });

            source.OfType<UndefinedViewModel>().Throttle(TimeSpan.FromSeconds(1)).Subscribe(o =>
            {
                GetAll.Execute(null);
            });

        }

        public ReactiveAsyncCommand GetAll { get; set; }
        public ReactiveAsyncCommand New { get; set; }

        ObservableAsPropertyHelper<ReactiveCollection<EvaluationItemTitle>> _Items;
        public ReactiveCollection<EvaluationItemTitle> Items { get { return _Items.Value; } }

        //private void SetActiveViewModel(object _)
        //{
        //    if (_selectedItem == null)
        //    {
        //        return;
        //    }

        //    dynamic evaluationItem = _selectedItem;

        //    if (ActivedItem != null && evaluationItem.Id == ActivedItem.Id)
        //        return;

        //    var response = _client.Get(new ReadOnlyEvaluationItem.GetEvaluationItem(evaluationItem.Id));

        //    var vm = new ItemViewModel { Name = response.Name, StatisticalWay = response.StatisticalWay, Id = evaluationItem.Id };
        //    AddOperations(response.Links, vm);

        //    ActivedItem = vm;
        //}

        //private void AddOperations(IEnumerable<Grandsys.Wfm.Services.Outsource.ServiceModel.Link> links, ItemViewModel vm)
        //{
        //    if (links == null)
        //        return;

        //    foreach (var link in links)
        //    {
        //        var request = link.Request;
        //        var verb = link.Method;
        //        vm.Operations.Add(new HyperCommand(__ =>
        //        {
        //            ResponseEvaluationItem rt;
        //            if (verb == "GET")
        //            {
        //                rt = _client.Send<ResponseEvaluationItem>(verb, request.ToUrl(verb), null);
        //            }
        //            else
        //            {
        //                var current = ActivedItem;
        //                var sourceProps = current.GetType().GetProperties().ToDictionary(o => o.Name, o => o);
        //                var props = request.GetType().GetProperties();
        //                foreach (var prop in props)
        //                {
        //                    System.Reflection.PropertyInfo sourceProp;
        //                    if (sourceProps.TryGetValue(prop.Name, out sourceProp))
        //                    {
        //                        var value = sourceProp.GetValue(current, null);
        //                        prop.SetValue(request, value, null);
        //                    }
        //                }
        //                rt = _client.Send<ResponseEvaluationItem>(verb, request.ToUrl(verb), request);
        //            }

        //            var newVm = ItemViewModel.Create(rt);

        //            AddOperations(rt.Links, newVm);
        //            ActivedItem = newVm;

        //            if (verb == "POST")
        //            {
        //                Items.Insert(0, new EvaluationItemTitle() { Name = rt.Name, Id = rt.Id });
        //                SelectedItem = Items[0];
        //            }
        //            if (verb == "DELETE")
        //                Items.Remove(SelectedItem);
        //            if (verb == "PUT")
        //            {
        //                GetAll.Execute(null);
        //                var item = Items.FirstOrDefault(o => o.Id == rt.Id);
        //                SelectedItem = item;
        //            }

        //        }) { Content = link.Name, Method = verb, Request = request });
        //    }
        //}

        private EvaluationItemTitle _selectedItem;
        public EvaluationItemTitle SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (CurrentViewModel == null || !CurrentViewModel.IsEditing)
                {
                    _selectedItem = value;

                    //SetActiveViewModel(null);

                }

                Application.Current.Dispatcher.BeginInvoke(new Action(() => { this.RaisePropertyChanged(x => x.SelectedItem); }), DispatcherPriority.ContextIdle);
            }
        }



        //private ItemViewModel _activedItem;
        //public ItemViewModel ActivedItem
        //{
        //    get { return _activedItem; }
        //    set
        //    {
        //        _activedItem = value;

        //        OnPropertyChanged(() => ActivedItem);
        //    }
        //}
    }
}

