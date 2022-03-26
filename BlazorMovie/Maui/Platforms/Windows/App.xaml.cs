// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Maui.WinUI;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
    }


    /* Unmerged change from project 'BlazorMovie.Maui (net6.0-ios)'
    Before:
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    After:
        protected override MauiApp CreateMauiApp()
        {
            return MauiProgram.CreateMauiApp();
    */

    /* Unmerged change from project 'BlazorMovie.Maui (net6.0-maccatalyst)'
    Before:
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    After:
        protected override MauiApp CreateMauiApp()
        {
            return MauiProgram.CreateMauiApp();
    */
    protected override MauiApp CreateMauiApp()
    {
        return MauiProgram.CreateMauiApp();
    }
}

