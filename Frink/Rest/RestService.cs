﻿using Frink.Helpers;
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
                String[] fromfile = await FileHelper.readHttpFromFile(ConstantsHelper.LOCAL_FILE_APPLICATION_THEME);
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
                String responseFormatted = System.Text.Encoding.UTF8.GetString(response, 0, response.Length);
                await FileHelper.writeHttpToFile(responseFormatted, ConstantsHelper.LOCAL_FILE_APPLICATION_THEME, asynctask._header[ConstantsHelper.API_ETAG]);
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
    }
}
