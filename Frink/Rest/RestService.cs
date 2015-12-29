using Frink.Delegates;
using Frink.Helpers;
using Frink.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Frink.Rest
{
    class RestService
    {
        /// <summary>
        ///     Executes the api call for grabbing the applications theme
        /// </summary>
        /// <returns></returns>
        async public static Task getMenu()
        {
            AsyncTask asynctask = new AsyncTask()
                        .setUrl(ConstantsHelper.API_HOST_URL + ConstantsHelper.API_METHOD_MENU + "/" + ConstantsHelper.API_APP_ID);

            // Load data from a stored file, if any
            bool fileExists = await FileHelper.ValidateFile(ApplicationData.Current.TemporaryFolder, ConstantsHelper.LOCAL_FILE_APPLICATION_THEME);
            string etag = null;
            if (fileExists)
            {
                string[] fromfile = await FileHelper.readHttpFromFile(ConstantsHelper.LOCAL_FILE_APPLICATION_THEME, ApplicationData.Current.TemporaryFolder);
                if (fromfile.Length > 1)
                {
                    asynctask.setETag(fromfile[1]);
                    etag = fromfile[1];
                }
                    
                DataHelper.Instance._themeModel = await JSONHelper.ParseDataObject<ThemeModel>(fromfile[0]);                
            }

            byte[] response = await asynctask.execute();

            // Load response if etags don't match or there wasn't any data before hand
            if (response != null && !isEtags(asynctask._header[ConstantsHelper.API_ETAG], etag))
            {
                string responseFormatted = System.Text.Encoding.UTF8.GetString(response, 0, response.Length);
                await FileHelper.writeHttpToFile(responseFormatted, ConstantsHelper.LOCAL_FILE_APPLICATION_THEME, 
                    asynctask._header[ConstantsHelper.API_ETAG], ApplicationData.Current.TemporaryFolder);
                DataHelper.Instance._themeModel = await JSONHelper.ParseDataObject<ThemeModel>(responseFormatted);
            }
            else
            {
#if DEBUG
                Debug.WriteLine("[RestService][getMenu()] Response was an empty object ");
#endif
            }

            // TODO: when better object cleaner method is found, 
            // simply set the objects to null and let GC clear them from the memory
            asynctask = null;
            response = null;

            ThemeDelegate.refreshTheme();
        }


        /// <summary>
        ///     Validates Etags
        /// </summary>
        /// <param name="etag_one">First etag to compare</param>
        /// <param name="etag_two">Second etag to compare</param>
        /// <returns></returns>
        private static bool isEtags(string etag_one, string etag_two)
        {
            if (etag_one == null && etag_two == null)
                return false;

            return etag_one == etag_two;
        }


        async public static Task getEntry(string url)
        {
            AsyncTask asynctask = new AsyncTask()
                        .setUrl(url);

            string fileName = url.Substring(url.LastIndexOf('/') + 1);
            fileName = "entry" + fileName + ".txt";
            bool fileExists = await FileHelper.ValidateFile(ApplicationData.Current.TemporaryFolder, fileName);
            string etag = null;
            if (fileExists)
            {
                string[] fromfile = await FileHelper.readHttpFromFile(fileName, ApplicationData.Current.TemporaryFolder);
                if (fromfile.Length > 1)
                {
                    asynctask.setETag(fromfile[1]);
                    etag = fromfile[1];
                }

                asynctask.setETag(etag);
                DataHelper.Instance._contentItemModel = await JSONHelper.ParseDataObservableCollection<ContentItemModel>(fromfile[0]);
            }

            byte[] response = await asynctask.execute();

            if (response != null && !isEtags(asynctask._header[ConstantsHelper.API_ETAG], etag))
            {
                string responseFormatted = System.Text.Encoding.UTF8.GetString(response, 0, response.Length);
                await FileHelper.writeHttpToFile(responseFormatted, fileName, 
                    asynctask._header[ConstantsHelper.API_ETAG], ApplicationData.Current.TemporaryFolder);
                DataHelper.Instance._contentItemModel = await JSONHelper.ParseDataObservableCollection<ContentItemModel>(responseFormatted);
            }
            else
            {
#if DEBUG
                Debug.WriteLine("[RestService][getEntry] Response was an empty object ");
#endif
            }

            // TODO: when better object cleaner method is found, 
            // simply set the objects to null and let GC clear them from the memory
            asynctask = null;
            response = null;

            Utils.formatAllContentItemModelData();
        }
    }
}
