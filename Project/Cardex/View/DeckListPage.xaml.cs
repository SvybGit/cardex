using ViewModel;

namespace View;

public partial class DeckListPage : ContentPage
{
    public DeckListPage(DeckListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
