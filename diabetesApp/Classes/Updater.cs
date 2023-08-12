using diabetisApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace diabetesApp.Classes
{
    internal class Updater
    {
        public async Task<bool> newVersion()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage res = await client.GetAsync(GlobalVars.UPDATE_CHECK_URL);

            string verions = await res.Content.ReadAsStringAsync();

            //Get all platforms
            string[] platforms = verions.Split(Environment.NewLine);


            //Get only the line where the TEXT before the : is the platform
            string currentVersion;
            try
            {
                currentVersion = platforms.Single(x => x.StartsWith(GlobalVars.PLATFROM));
            }catch {
#warning Add lgger here
                return false;
            }
            //Check current platform
            if(currentVersion == GlobalVars.VERSION)
            {
                return false;
            }
            return true;
        }

        public async void openDownloadSite()
        {
            Uri uri = new(GlobalVars.UPDATE_URL);
            try
            {
                await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch
            {
#warning Add lgger here
            }
        }

    }
}
