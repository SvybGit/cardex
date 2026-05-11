using Cardex.Model.Dtos;
using Cardex.Model.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ViewModel;

public partial class DeckListViewModel(
    DeckService deckService,
    INavigationService navigationService) : ObservableObject
{
    [ObservableProperty] public partial ObservableCollection<DeckDto> Decks { get; set; } = [];

    [ObservableProperty]
    public partial DeckDto? SelectedDeck { get; set; }

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [RelayCommand]
    public async Task LoadDecksAsync()
    {
        if (IsLoading)
        {
            return;
        }

        try
        {
            IsLoading = true;
            Decks = new ObservableCollection<DeckDto>(await deckService.GetAllDecksAsync());
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private Task CreateDeckAsync() => navigationService.GoToCreateDeckAsync();

    [RelayCommand]
    private Task StudyDeckAsync(DeckItemViewModel? deck) =>
        deck is null
            ? Task.CompletedTask
            : navigationService.GoToStudySessionAsync(deck.Deck);
}
