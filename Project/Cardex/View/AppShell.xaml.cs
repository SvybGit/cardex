namespace View;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Items.Add(new ShellContent
        {
            Title = "Decks",
            Route = nameof(DeckListPage),
            ContentTemplate = new DataTemplate(typeof(DeckListPage))
        });

        Routing.RegisterRoute(nameof(CreateDeckPage), typeof(CreateDeckPage));
        Routing.RegisterRoute(nameof(StudySessionPage), typeof(StudySessionPage));
    }
}
