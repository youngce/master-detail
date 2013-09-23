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
                        if (link.Method != "GET")
                        {
                            cmd.Response.Throttle(TimeSpan.FromMilliseconds(500)).Subscribe(_ =>
                              {
                                  GetAll.Execute(null);
                              });
                        }
                        cmd.Response.Subscribe(obsvr);
                        vm.Operations.Add(cmd);
                    }
                }
                source.OnNext(vm);
            });


            var selectedItemChanged = this.WhenAny(x => x.SelectedItem, x => x.Value);

            var getEvaluationItem = selectedItemChanged.ObserveOn(Scheduler.Default)
                                        .Where(x => x != null && x.Id != CurrentViewModel.Id)
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

            this.WhenAny(x => x.Items, x => x.Value).Where(o => o != null).Throttle(TimeSpan.FromMilliseconds(200)).Subscribe(o =>
            {
                SelectedItem = o.FirstOrDefault(a => Equals(a.Id, CurrentViewModel.Id));
            });
        }

        public ReactiveAsyncCommand GetAll { get; set; }
        public ReactiveAsyncCommand New { get; set; }

        ObservableAsPropertyHelper<ReactiveCollection<EvaluationItemTitle>> _Items;
        public ReactiveCollection<EvaluationItemTitle> Items { get { return _Items.Value; } }

        private EvaluationItemTitle _selectedItem;
        public EvaluationItemTitle SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (!CurrentViewModel.IsEditing)
                {
                    _selectedItem = value;
                }

                Application.Current.Dispatcher.BeginInvoke(new Action(() => { this.RaisePropertyChanged(x => x.SelectedItem); }), DispatcherPriority.ContextIdle);
            }
        }
    }
}

