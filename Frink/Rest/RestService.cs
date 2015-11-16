using Frink.Helpers;
using Frink.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Frink.Rest
{
    class RestService
    {
        async public static Task getMenu() 
        {
                AsyncTask asynctask = new AsyncTask()
                        .setUrl(ConstantsHelper.API_HOST_URL + ConstantsHelper.API_METHOD_MENU + "/" + ConstantsHelper.API_APP_ID);

                byte[] response = await asynctask.execute();

                if (response != null)
                {
                    String responseFormatted = System.Text.Encoding.UTF8.GetString(response, 0, response.Length);

#if DEBUG
                    Debug.WriteLine("[RestService][getMenu()] Response: " + responseFormatted);
#endif
                    // Store the received data in the locale, encrypted
                    await FileHelper.GetFromLocal
                    (
                        responseFormatted,
                        ApplicationData.Current.TemporaryFolder,
                        ConstantsHelper.LOCAL_FILE_APPLICATION_THEME,
                        ConstantsHelper.LOCALE_PASSWORD
                    );

                    DataHelper.Instance._themeModel = await JSONHelper.ParseDataObject<ThemeModel>(responseFormatted);
                }
                else
                {
#if DEBUG
                    Debug.WriteLine("[RestService][getMenu()] Response was an empty object ");
#endif
                }
        }
  
    }
}
