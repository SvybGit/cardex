using Cardex.Model.Dtos;
using Cardex.Model.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ViewModel;

public partial class CreateDeckViewModel(
    DeckService deckService,
    CategoryService categoryService) : ObservableObject
{
    private static readonly CategoryDto EmptyCategory = new()
    {
        Id = 0,
        Name = "No category"
    };

    [ObservableProperty]
    public partial List<CategoryDto> Categories { get; set; } = [];

    public DeckDto Deck
    {
        get => deck;
        set
        {
            deck = value;
            PageTitle = "Edit deck";
            Name = deck.Name;
        }
    }

    [ObservableProperty]
    public partial string PageTitle { get; set; } = "Create deck";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveDeckCommand))]
    public partial string Name { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveDeckCommand))]
    public partial CategoryDto? SelectedCategory { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddCardCommand))]
    public partial string CurrentCardFront { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddCardCommand))]
    public partial string CurrentCardBack { get; set; } = string.Empty;

    [ObservableProperty]
    public partial ObservableCollection<CardDto> CardsToCreate { get; set; } = [];

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveDeckCommand))]
    [NotifyCanExecuteChangedFor(nameof(AddCardCommand))]
    [NotifyCanExecuteChangedFor(nameof(RemoveCardCommand))]
    public partial bool IsSaving { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    public partial string ErrorMessage { get; set; } = string.Empty;

    private readonly HashSet<int> originalCardIds = [];
    private DeckDto deck = new();
    private bool hasLoadedDeckCards;

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    [RelayCommand]
    private async Task LoadCategoriesAsync()
    {
        if (IsLoading)
        {
            return;
        }

        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            Categories =
            [
                EmptyCategory,
                .. await categoryService.GetAllCategoriesAsync()
            ];
            SelectCategory(deck.Category?.Id);
            await LoadDeckCardsAsync();
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand(CanExecute = nameof(CanAddCard))]
    private void AddCard()
    {
        CardsToCreate.Add(new CardDto
        {
            DeckId = deck.Id,
            Front = CurrentCardFront.Trim(),
            Back = CurrentCardBack.Trim()
        });

        CurrentCardFront = string.Empty;
        CurrentCardBack = string.Empty;
    }

    [RelayCommand(CanExecute = nameof(CanRemoveCard))]
    private void RemoveCard(CardDto? card)
    {
        if (card is null)
        {
            return;
        }

        CardsToCreate.Remove(card);
    }

    [RelayCommand(CanExecute = nameof(CanSaveDeck))]
    private async Task SaveDeckAsync()
    {
        try
        {
            IsSaving = true;
            ErrorMessage = string.Empty;

            deck.Name = Name.Trim();
            deck.Category = SelectedCategory == EmptyCategory ? null : SelectedCategory;

            deck = deck.Id == 0
                ? await deckService.CreateDeckAsync(deck)
                : await deckService.UpdateDeckAsync(deck);

            foreach (var card in CardsToCreate)
            {
                if (card.Id == 0)
                {
                    card.DeckId = deck.Id;
                    await deckService.CreateCardInDeckAsync(deck.Id, card);
                }
                else
                {
                    await deckService.UpdateCardAsync(card);
                }
            }

            var remainingCardIds = CardsToCreate
                .Where(card => card.Id != 0)
                .Select(card => card.Id);

            foreach (var removedCardId in originalCardIds.Except(remainingCardIds).ToList())
            {
                await deckService.DeleteCardAsync(removedCardId);
            }

            ResetForm();
        }
        catch (Exception exception)
        {
            ErrorMessage = exception.Message;
        }
        finally
        {
            IsSaving = false;
        }
    }

    private bool CanSaveDeck() =>
        !IsSaving
        && !string.IsNullOrWhiteSpace(Name);

    private bool CanAddCard() =>
        !IsSaving
        && !string.IsNullOrWhiteSpace(CurrentCardFront)
        && !string.IsNullOrWhiteSpace(CurrentCardBack);

    private bool CanRemoveCard(CardDto? card) =>
        !IsSaving
        && card is not null;

    private void SelectCategory(int? categoryId)
    {
        SelectedCategory = categoryId.HasValue
            ? Categories.FirstOrDefault(category => category.Id == categoryId.Value) ?? EmptyCategory
            : EmptyCategory;
    }

    private async Task LoadDeckCardsAsync()
    {
        if (deck.Id == 0 || hasLoadedDeckCards)
        {
            return;
        }

        var cards = await deckService.GetCardsForDeckAsync(deck.Id);
        CardsToCreate = new ObservableCollection<CardDto>(cards);
        originalCardIds.Clear();

        foreach (var card in cards)
        {
            originalCardIds.Add(card.Id);
        }

        hasLoadedDeckCards = true;
    }

    private void ResetForm()
    {
        deck = new DeckDto();
        hasLoadedDeckCards = false;
        originalCardIds.Clear();
        PageTitle = "Create deck";
        Name = string.Empty;
        SelectedCategory = EmptyCategory;
        CardsToCreate.Clear();
        CurrentCardFront = string.Empty;
        CurrentCardBack = string.Empty;
    }
}
