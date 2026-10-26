using Cardex.Model.Dtos;

namespace ViewModel;

public interface INavigationService
{
    Task GoToCreateDeckAsync();
    Task GoToEditDeckAsync(DeckDto deck);
    Task GoToStudySessionAsync(DeckDto deck, bool startNew);
    Task GoBackAsync();
}
