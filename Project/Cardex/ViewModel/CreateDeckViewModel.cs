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
            SelectedCategory = EmptyCategory;
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

            var deck = await deckService.CreateDeckAsync(new DeckDto
            {
                Name = Name.Trim(),
                CategoryId = SelectedCategory == EmptyCategory ? null : SelectedCategory?.Id
            });

            foreach (var card in CardsToCreate)
            {
                await deckService.CreateCardInDeckAsync(deck.Id, new CardDto
                {
                    Front = card.Front,
                    Back = card.Back
                });
            }

            Name = string.Empty;
            SelectedCategory = EmptyCategory;
            CardsToCreate.Clear();
            CurrentCardFront = string.Empty;
            CurrentCardBack = string.Empty;
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
}
