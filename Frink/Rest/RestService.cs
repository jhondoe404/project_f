using Frink.Helpers;
using Frink.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frink.Rest
{
    class RestService
    {
        async public static Task getMenu() 
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) 
            {
                AsyncTask asynctask = new AsyncTask()
                        .setUrl(ConstantsHelper.API_HOST_URL + ConstantsHelper.API_METHOD_MENU + "/" + ConstantsHelper.API_APP_ID);

                byte[] response = await asynctask.execute();
                String responseFormatted = System.Text.Encoding.UTF8.GetString(response, 0, response.Length);

                // Store the received data in the locale 
                String responseencrypted = EncryptHelper.AES_Encrypt(responseFormatted, ConstantsHelper.LOCALE_PASSWORD);
                // var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                // localSettings.Values[ConstantsHelper.LOCALE_KEY_APPLICATION_THEME] = localSettings;

                Debug.WriteLine("Response: " + responseFormatted);

                DataHelper.Instance._themeModel = await JSONHelper.ParseDataObject<ThemeModel>(responseFormatted);
                Debug.WriteLine("Serialized: " + DataHelper.Instance._themeModel.app.name);
            }
            else
            {
                Debug.WriteLine("No internet connection");
            }
        }
  
    }
}
