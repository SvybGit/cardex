using CommunityToolkit.Mvvm.Input;
using ViewModel;

namespace View;

public partial class DeckListPage : ContentPage
{
    public DeckListPage(DeckListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    public async void CreateDeckButton_OnClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CreateDeckPage));
    }
}
