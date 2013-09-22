using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using WpfApplication4.ViewModels;

namespace WpfApplication4
{
    public class HyperCommand
    {
        public HyperCommand(Action<object> execute)
        {
            Command = new DelegateCommand(execute);
        }

        public string Content { get; set; }

        public ICommand Command { get; private set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MasterViewModel();

            // var client = new ServiceStack.ServiceClient.Web.JsonServiceClient("http://localhost:8888");



            DataContext = vm;
        }
    }
}
