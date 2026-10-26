using System.Text;
using CommunityToolkit.Maui.Storage;
using ViewModel;

namespace View;

public partial class DeckListPage : ContentPage
{
    public DeckListPage(DeckListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private DeckListViewModel ViewModel => (DeckListViewModel)BindingContext;

    private async void DeckItemTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is not DeckItemViewModel deck || ViewModel.IsLoading)
        {
            return;
        }

        if (!deck.HasActiveSession)
        {
            await ViewModel.StudyDeckCommand.ExecuteAsync(deck);
            return;
        }

        var action = await DisplayActionSheetAsync(
            "Study session in progress",
            "Close",
            null,
            "Continue",
            "Start new");

        switch (action)
        {
            case "Continue":
                await ViewModel.ContinueStudyCommand.ExecuteAsync(deck);
                return;
            case "Start new":
                await ViewModel.StartNewStudyCommand.ExecuteAsync(deck);
                break;
        }
    }

    private async void ExportClicked(object? sender, EventArgs e)
    {
        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(await ViewModel.ExportAsync()));
        var result = await SaveExportFileAsync(stream);
        if (result.IsCancelled)
        {
            return;
        }

        result.EnsureSuccess();
    }

    private async void ImportClicked(object? sender, EventArgs e)
    {
        var file = await PickFileAsync("Select import file");
        if (file?.FullPath is null)
        {
            return;
        }

        await ViewModel.ImportCommand.ExecuteAsync(file.FullPath);
    }

    private static async Task<FileResult?> PickFileAsync(string title)
    {
        try
        {
            return await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = title
            });
        }
        catch (OperationCanceledException)
        {
            return null;
        }
    }

    private static async Task<FileSaverResult> SaveExportFileAsync(Stream stream)
    {
        try
        {
            return await FileSaver.Default.SaveAsync("cardex.json", stream, CancellationToken.None);
        }
        catch (OperationCanceledException exception)
        {
            return new FileSaverResult(string.Empty, exception);
        }
    }
}