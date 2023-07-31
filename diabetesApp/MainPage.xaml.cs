using diabetisApp;
using System.Net.Sockets;
using System.Net;
//using Android.Content;

namespace diabetesApp;

public partial class MainPage : ContentPage
{


    public MainPage()
    {
        InitializeComponent();

        //Auto read
        if (Preferences.ContainsKey("setup") && Preferences.Get("autoRead", false))
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

