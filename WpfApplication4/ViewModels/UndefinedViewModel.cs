namespace WpfApplication4.ViewModels
{
    public class UndefinedViewModel :ItemViewModel
    {
        
    }

    public class NewItemViewModel : ItemViewModel
    {
        public NewItemViewModel()
        {
            IsEditing = true;
        }
    }

    public class DeletedViewModel : ItemViewModel
    {
    }
}