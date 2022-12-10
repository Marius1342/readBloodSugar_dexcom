using ModernHttpClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diabetesApp
{
     class DexcomApi
    {
        private string username = null;
        private string password = null;
        private string server = null;
        private string sessionId = null;

        public string SessionId = "";

        private const string EU_HOST = "shareous1.dexcom.com";
        //private const string US_HOST = "share2.dexcom.com";
        //From github 
        private const string APPID = "d8665ade-9673-4e27-9ff6-92db4ce13d13";


        public enum Language
        {
            EN,
            DE
        }


        public DexcomApi(string username, string password, string oldSessionId = null)
        {
            this.username = username;
            this.password = password;
            if (oldSessionId == null)
            {
                Task<bool> sucess = getSessionToken();
                sucess.Wait();
                if (sucess.Result)
                {
                    return;
                }
                else
                {
                    throw new Exception("Wrong User/Password");
                }
            }
            sessionId = oldSessionId;
            SessionId = sessionId;

        }

        private bool testSessionId(string id)
        {
            try
            {
                getValue().Wait();
            }
            catch
            {
                return false;
            }
            return true;


        }


        private async Task<string> getAccountId()
        {
            string accountUrl = getApiUrl("General/AuthenticatePublisherAccount");
            JObject valuePairs = new JObject();
            valuePairs.Add("password", password);
            valuePairs.Add("accountName", username);
            valuePairs.Add("applicationId", APPID);
            string json = await PostData(accountUrl, valuePairs.ToString(Newtonsoft.Json.Formatting.None));
            if (json.Contains("{"))
            {
                throw new Exception("Invalid password or username");
            }
            return json;


        }
        private async Task<bool> getSessionToken()
        {
            string id;
            try
            {
                id = await getAccountId();
            }
            catch
            {
                return false;
            }
            JObject json = new JObject();
            json.Add("applicationId", APPID);
            json.Add("accountId", id.Replace("\"", ""));
            json.Add("password", password);
            string getUrl = getApiUrl("General/LoginPublisherAccountById");
            string re = await PostData(getUrl, json.ToString(Newtonsoft.Json.Formatting.None));

            sessionId = re.Replace("\"", "");

            if (sessionId.StartsWith("{"))
            {
                return false;
            }

            SessionId = sessionId;

            return true;
        }


        public async Task<string> getValue()
        {
            string data;
            try
            {

                string getUrl = getApiUrl("Publisher/ReadPublisherLatestGlucoseValues");
                JObject json = new JObject();
                json.Add("maxCount", 1);
                json.Add("minutes", 60);
                json.Add("sessionId", sessionId);
                var client = new HttpClient();
                data = await PostData(getUrl, json.ToString(Newtonsoft.Json.Formatting.None));
                if (data.StartsWith("[") == false)
                {
                    throw new Exception("New session token");

                }
            }
            catch
            {
                await getSessionToken();
                string getUrl = getApiUrl("Publisher/ReadPublisherLatestGlucoseValues");
                JObject json = new JObject();
                json.Add("maxCount", 1);
                json.Add("minutes", 60);
                json.Add("sessionId", sessionId);
                data = await PostData(getUrl, json.ToString(Newtonsoft.Json.Formatting.None));

            }

            return data;
        }
        private async Task<string> PostData(string url, string json)
        {

            var client = new HttpClient(new NativeMessageHandler());

            var response = client.PostAsync(
            url,
             new StringContent(json, Encoding.UTF8, "application/json"));
            return await response.Result.Content.ReadAsStringAsync();
        }


        private string getApiUrl(string resource)
        {
            return $"https://{EU_HOST}/ShareWebServices/Services/{resource}";
        }

        public static string ConvertSignToText(string sign, Language lang = Language.EN)
        {

            if (lang == Language.EN)
            {
                return sign;
            }
            switch (sign)
            {
                case "DoubleUp":
                    sign = "doppel nach oben";
                    break;
                case "SingleUp":
                    sign = "einfach nach oben";
                    break;
                case "FortyFiveUp":
                    sign = "langsam nach oben";
                    break;
                case "Flat":
                    sign = "gleich bleibend";
                    break;
                case "FortyFiveDown":
                    sign = "langsam nach unten";
                    break;
                case "SingleDown":
                    sign = "einfach nach unten";
                    break;
                case "DoubleDown":
                    sign = "doppel nach unten";
                    break;
                default:
                    sign = sign;
                    break;
            }
            return sign;

        }


    }
}

