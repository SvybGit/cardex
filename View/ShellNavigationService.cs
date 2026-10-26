using Cardex.Model.Dtos;
using ViewModel;

namespace View;

public class ShellNavigationService : INavigationService
{
    private const string DeckQueryKey = "deck";
    private const string StartNewQueryKey = "startNew";
    private const string BackRoute = "..";

    public Task GoToCreateDeckAsync() =>
        Shell.Current.GoToAsync(nameof(CreateDeckPage));

    public Task GoToEditDeckAsync(DeckDto deck) =>
        Shell.Current.GoToAsync(nameof(CreateDeckPage), new Dictionary<string, object>
        {
            { DeckQueryKey, deck }
        });

    public Task GoToStudySessionAsync(DeckDto deck, bool startNew) =>
        Shell.Current.GoToAsync(nameof(StudySessionPage), new Dictionary<string, object>
        {
            { DeckQueryKey, deck },
            { StartNewQueryKey, startNew }
        });

    public Task GoBackAsync() =>
        Shell.Current.GoToAsync(BackRoute);
}
