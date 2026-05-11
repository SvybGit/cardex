using ViewModel;

namespace View;

[QueryProperty(nameof(DeckId), "deckId")]
public partial class StudySessionPage : ContentPage
{
    private readonly StudySessionViewModel viewModel;

    public int DeckId
    {
        set => viewModel.DeckId = value;
    }

    public StudySessionPage(StudySessionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        this.viewModel = viewModel;
    }

    public async void GoBack(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
