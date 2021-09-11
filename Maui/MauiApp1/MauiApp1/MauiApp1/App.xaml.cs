using Application = Microsoft.Maui.Controls.Application;

namespace MauiApp1
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new MainPage();
		}
	}
}
