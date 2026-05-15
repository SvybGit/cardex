using Cardex.Model.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ViewModel;

public partial class DeckListViewModel(
    DeckService deckService,
    StudySessionService studySessionService,
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
            var activeSessionDeckIds = await studySessionService.GetDeckIdsWithActiveSessionsAsync();
            var deckItems = new List<DeckItemViewModel>();

            foreach (var deck in decks)
            {
                var cards = await deckService.GetCardsForDeckAsync(deck.Id);
                deckItems.Add(new DeckItemViewModel(deck, cards.Count, activeSessionDeckIds.Contains(deck.Id)));
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
    private Task EditDeckAsync(DeckItemViewModel deck) =>
        navigationService.GoToEditDeckAsync(deck.Deck);

    [RelayCommand]
    private Task StudyDeckAsync(DeckItemViewModel deck) =>
        navigationService.GoToStudySessionAsync(deck.Deck, !deck.HasActiveSession);

    [RelayCommand]
    private Task ContinueStudyAsync(DeckItemViewModel deck) =>
        navigationService.GoToStudySessionAsync(deck.Deck, false);

    [RelayCommand]
    private Task StartNewStudyAsync(DeckItemViewModel deck) =>
        navigationService.GoToStudySessionAsync(deck.Deck, true);

    [RelayCommand]
    private async Task DeleteDeckAsync(DeckItemViewModel deck)
    {
        await deckService.DeleteDeckAsync(deck.Id);
        Decks.Remove(deck);
    }
}
