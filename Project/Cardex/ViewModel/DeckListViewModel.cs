using Cardex.Model.Dtos;
using Cardex.Model.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ViewModel;

public partial class DeckListViewModel : ObservableObject
{
    private readonly DeckService _deckService = new();

    [ObservableProperty] public partial ObservableCollection<DeckDto> Decks { get; set; } = [];

    [ObservableProperty]
    public partial DeckDto? SelectedDeck { get; set; }

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    public async Task LoadDecksAsync()
    {
        if (IsLoading)
        {
            return;
        }

        try
        {
            IsLoading = true;
            Decks = new ObservableCollection<DeckDto>(await _deckService.GetAllDecksAsync());
            
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void CreateDeck()
    {
        // TODO: Navigate to the deck creation view when that page exists.
    }
}
