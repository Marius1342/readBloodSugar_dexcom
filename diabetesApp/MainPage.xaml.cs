using diabetisApp;

namespace diabetesApp;

public partial class MainPage : ContentPage
{
	

	public MainPage()
	{
		InitializeComponent();
        if (Preferences.ContainsKey("setup"))
        {
            autoRead();
        }
    }
    async void autoRead()
    {
        if (GlobalVars.haveRead)
        {
            return;
        }
        await Task.Delay(2000);
        GlobalVars.autoClose = true;
        Read(this, null);

    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PopToRootAsync();
        await Application.Current.MainPage.Navigation.PushAsync(new Settings());
    }
    private async void Read(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PopToRootAsync();
        await Application.Current.MainPage.Navigation.PushAsync(new Read());
    }

}

