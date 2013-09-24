using Grandsys.Wfm.Services.Outsource.ServiceModel;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;
using System.Linq;

namespace WpfApplication4.ViewModels
{
    public class ItemEditViewModel : ItemViewModel
    {
        public IEnumerable<Grandsys.Wfm.Services.Outsource.ServiceModel.Link> SetFormulaOptions { get; set; }

        public ItemEditViewModel()
        {
            IsEditing = true;
        }

        private Link _selectedFormula;
        public Link SelectedFormula
        {
            get
            {   
                return _selectedFormula;
            }
            set
            {
                _selectedFormula = value;
                var putOperation = Operations.FirstOrDefault(o => o.Method == "PUT");
                if (putOperation != null)
                {
                    putOperation.Request = _selectedFormula.Request;
                    putOperation.Method = _selectedFormula.Method;
                }
                OnPropertyChanged(() => SelectedFormula);
            }
        }

        public override void OperationAdded()
        {
            if (!string.IsNullOrEmpty(Formula))
            {
                SelectedFormula = SetFormulaOptions.FirstOrDefault(o => Formula.ToLower().Contains(o.Name.ToLower()));
            }
        }
    }
}