using Cardex.DAL.Enums;
using Cardex.Model.Dtos;
using Cardex.Model.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ViewModel;

public partial class StudySessionViewModel(
    DeckService deckService,
    StudySessionService studySessionService,
    INavigationService navigationService) : ObservableObject
{
    private int? sessionId;
    private bool hasLoaded;

    [ObservableProperty]
    public partial DeckDto? Deck { get; set; }

    [ObservableProperty]
    public partial bool StartNew { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<CardDto> Cards { get; set; } = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentCardNumber))]
    [NotifyPropertyChangedFor(nameof(CanShowCurrentCard))]
    [NotifyPropertyChangedFor(nameof(CanShowRatingButtons))]
    [NotifyPropertyChangedFor(nameof(DisplayedCardText))]
    public partial CardDto? CurrentCard { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentCardNumber))]
    [NotifyPropertyChangedFor(nameof(StudyProgress))]
    public partial int CurrentCardIndex { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanShowCurrentCard))]
    [NotifyPropertyChangedFor(nameof(CanShowRatingButtons))]
    [NotifyPropertyChangedFor(nameof(DisplayedCardText))]
    [NotifyPropertyChangedFor(nameof(CardSideLabel))]
    public partial bool IsAnswerVisible { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanShowCurrentCard))]
    [NotifyPropertyChangedFor(nameof(CanShowRatingButtons))]
    public partial bool IsComplete { get; set; }

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanShowRatingButtons))]
    public partial bool IsBusy { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StudyProgress))]
    [NotifyPropertyChangedFor(nameof(ReviewedCardsText))]
    public partial int TotalCards { get; set; }

    public int CurrentCardNumber => CurrentCardIndex + 1;

    public double StudyProgress => TotalCards == 0
        ? 0
        : (double)CurrentCardNumber / TotalCards;

    public bool CanShowCurrentCard => CurrentCard is not null && !IsComplete;

    public bool CanShowRatingButtons => CanRateCurrentCard();

    public string CardSideLabel => IsAnswerVisible ? "Answer" : "Question";

    public string ReviewedCardsText => TotalCards == 1
        ? "You reviewed 1 card."
        : $"You reviewed {TotalCards} cards.";

    public string DisplayedCardText => CurrentCard is null
        ? string.Empty
        : IsAnswerVisible ? CurrentCard.Back : CurrentCard.Front;

    [RelayCommand]
    private async Task LoadSessionAsync()
    {
        if (IsLoading || hasLoaded)
        {
            return;
        }

        try
        {
            IsLoading = true;

            if (Deck is null)
            {
                throw new InvalidOperationException("Deck was not provided.");
            }

            Cards = new ObservableCollection<CardDto>(await deckService.GetCardsForDeckAsync(Deck.Id));
            TotalCards = Cards.Count;

            if (TotalCards == 0)
            {
                throw new InvalidOperationException("This deck has no cards to study.");
            }

            var session = StartNew
                ? await studySessionService.RestartStudySessionAsync(Deck.Id)
                : await studySessionService.GetActiveSessionForDeckAsync(Deck.Id)
                  ?? await studySessionService.StartStudySessionAsync(Deck.Id);
            sessionId = session.Id;

            CurrentCardIndex = session.CurrentCardIndex;
            LoadCurrentCard();
            IsAnswerVisible = false;
            hasLoaded = true;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void FlipCard()
    {
        if (!CanShowCurrentCard)
        {
            return;
        }

        IsAnswerVisible = !IsAnswerVisible;
    }

    [RelayCommand]
    private Task RateAgainAsync() => RateCurrentCardAsync(CardRating.Again);

    [RelayCommand]
    private Task RateHardAsync() => RateCurrentCardAsync(CardRating.Hard);

    [RelayCommand]
    private Task RateEasyAsync() => RateCurrentCardAsync(CardRating.Easy);

    [RelayCommand]
    private Task LeaveSessionAsync() => navigationService.GoBackAsync();

    [RelayCommand]
    private async Task EndSessionAsync()
    {
        if (sessionId is null || IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            await studySessionService.FinishStudySessionAsync(sessionId.Value);
            await navigationService.GoBackAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task RateCurrentCardAsync(CardRating rating)
    {
        if (!CanRateCurrentCard())
        {
            return;
        }

        try
        {
            IsBusy = true;
            var ratedCard = CurrentCard!;

            await studySessionService.RecordCardResultAsync(sessionId!.Value, ratedCard.Id, rating, CurrentCardIndex + 1);

            if (rating == CardRating.Again)
            {
                Cards.Add(ratedCard);
                TotalCards = Cards.Count;
            }

            IsAnswerVisible = false;
            CurrentCardIndex++;
            LoadCurrentCard();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void LoadCurrentCard()
    {
        if (CurrentCardIndex >= TotalCards)
        {
            CurrentCard = null;
            IsComplete = true;
            return;
        }

        CurrentCard = Cards[CurrentCardIndex];
        IsComplete = false;
    }

    private bool CanRateCurrentCard() =>
        IsAnswerVisible
        && !IsBusy
        && sessionId is not null
        && CurrentCard is not null
        && !IsComplete;
}
