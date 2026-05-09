using CommunityToolkit.Mvvm.Input;
using ViewModel;

namespace View;

public partial class CreateDeckPage : ContentPage
{
    public CreateDeckPage(CreateDeckViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    public async void GoBack(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
