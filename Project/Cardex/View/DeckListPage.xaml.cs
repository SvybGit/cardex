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

    public async void StudyButton_OnClicked(object? sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: int deckId })
        {
            return;
        }

        await Shell.Current.GoToAsync($"{nameof(StudySessionPage)}?deckId={deckId}");
    }
}
