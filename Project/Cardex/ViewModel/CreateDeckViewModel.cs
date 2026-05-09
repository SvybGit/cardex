using Cardex.Model.Dtos;
using Cardex.Model.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveDeckCommand))]
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

    [RelayCommand(CanExecute = nameof(CanSaveDeck))]
    private async Task SaveDeckAsync()
    {
        try
        {
            IsSaving = true;
            ErrorMessage = string.Empty;

            await deckService.CreateDeckAsync(new DeckDto
            {
                Name = Name.Trim(),
                CategoryId = SelectedCategory == EmptyCategory ? null : SelectedCategory?.Id
            });

            Name = string.Empty;
            SelectedCategory = EmptyCategory;
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
}
