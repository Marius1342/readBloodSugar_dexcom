
using diabetisApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ModernHttpClient;

namespace diabetesApp.Classes
{
    internal static class Updater
    {

        //Needs 5 min to check if new version, GitHub caches all content for 5 min
    public static async Task<bool> NewVersion()
        {          HttpClient client = new HttpClient(new NativeMessageHandler());

            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true
            };
            HttpResponseMessage res = await client.GetAsync(GlobalVars.UPDATE_CHECK_URL);

            string verions = await res.Content.ReadAsStringAsync();

            //Get all platforms
            string[] platforms = verions.Split('\n');


            //Get only the line where the TEXT before the : is the platform
            string currentVersion;
            try
            {
                currentVersion = platforms.Single(x => x.StartsWith(GlobalVars.PLATFORM));
            }catch {
                Logger.Error($"Error with data: {verions}");
                return false;
            }
            //Check current platform
            if (currentVersion.Split(':')[1] == GlobalVars.VERSION)
            {
                Logger.Log($"New version available: {currentVersion.Split(':')[1]}");
                return false;
            }
            return true;
        }

        public static async void openDownloadSite()
        {
            Uri uri = new(GlobalVars.UPDATE_URL);
            try
            {
                await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch(Exception ex) 
            {
                Logger.Error($"Error with WebBrowser open: {ex.Message}");
            }
        }

    }
}
