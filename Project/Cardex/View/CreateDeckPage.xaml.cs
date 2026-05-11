using Cardex.Model.Dtos;
using ViewModel;

namespace View;

[QueryProperty(nameof(Deck), "deck")]
public partial class CreateDeckPage : ContentPage
{
    private readonly CreateDeckViewModel viewModel;

    public DeckDto Deck
    {
        set => viewModel.Deck = value;
    }

    public CreateDeckPage(CreateDeckViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
    }
}
