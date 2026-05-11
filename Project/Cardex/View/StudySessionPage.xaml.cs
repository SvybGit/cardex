using Cardex.Model.Dtos;
using ViewModel;

namespace View;

[QueryProperty(nameof(Deck), "deck")]
public partial class StudySessionPage : ContentPage
{
    private readonly StudySessionViewModel viewModel;

    public DeckDto Deck
    {
        set => viewModel.Deck = value;
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
