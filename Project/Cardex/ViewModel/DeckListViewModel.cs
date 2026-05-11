using Cardex.Model.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ViewModel;

public partial class DeckListViewModel(
    DeckService deckService,
    ImportExportService importExportService,
    INavigationService navigationService) : ObservableObject
{
    [ObservableProperty] public partial ObservableCollection<DeckItemViewModel> Decks { get; set; } = [];

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
            var decks = await deckService.GetAllDecksAsync();
            var deckItems = new List<DeckItemViewModel>();

            foreach (var deck in decks)
            {
                var cards = await deckService.GetCardsForDeckAsync(deck.Id);
                deckItems.Add(new DeckItemViewModel(deck, cards.Count));
            }

            Decks = new ObservableCollection<DeckItemViewModel>(deckItems);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private Task CreateDeckAsync() => navigationService.GoToCreateDeckAsync();

    public Task<string> ExportAsync() => importExportService.ExportAsync();

    [RelayCommand]
    private async Task ImportAsync(string path)
    {
        await importExportService.ImportAsync(path);
        await LoadDecksAsync();
    }

    [RelayCommand]
    private Task EditDeckAsync(DeckItemViewModel? deck) =>
        deck is null
            ? Task.CompletedTask
            : navigationService.GoToEditDeckAsync(deck.Deck);

    [RelayCommand]
    private Task StudyDeckAsync(DeckItemViewModel? deck) =>
        deck is null
            ? Task.CompletedTask
            : navigationService.GoToStudySessionAsync(deck.Deck);

    [RelayCommand]
    private async Task DeleteDeckAsync(DeckItemViewModel? deck)
    {
        if (deck is null)
        {
            return;
        }

        await deckService.DeleteDeckAsync(deck.Id);
        Decks.Remove(deck);
    }
}
