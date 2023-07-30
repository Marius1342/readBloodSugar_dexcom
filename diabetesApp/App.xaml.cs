
using Android.Content;
using Android.SE.Omapi;
using System.Net.Sockets;

namespace diabetesApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();

        //new Channel()

    }
}
