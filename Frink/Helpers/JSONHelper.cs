using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Frink.Helpers
{
    class JSONHelper
    {
        /// <summary>
        ///     Serializes JSON into a data model as ObservableCollection
        /// </summary>
        /// <typeparam name="T">Type of class to be used as a template for parsing</typeparam>
        /// <param name="json">string</param>
        /// <returns>ObservableCollection</returns>
        public static async Task<ObservableCollection<T>> ParseDataObservableCollection<T>(string json)
        {
            var token = JToken.Parse(json);
            var dataCollection = new ObservableCollection<T>();
            if (token is JArray)
            {
                dataCollection = JsonConvert.DeserializeObject<ObservableCollection<T>>(json);
            }
            else
            {
                var myMessageDialog = new MessageDialog("Invalid Json format");
                await myMessageDialog.ShowAsync();
            }

            return dataCollection;
        }



        /// <summary>
        ///     Serializes JSON into a data model 
        /// </summary>
        /// <typeparam name="T">Type of class to be used as a template for parsing</typeparam>
        /// <param name="json">string</param>
        /// <returns>ObservableCollection</returns>
        public static async Task<T> ParseDataObject<T>(string json)
        {
            var token = JToken.Parse(json);

            if (token is JObject)
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                var myMessageDialog = new MessageDialog("Invalid Json format");
                myMessageDialog.ShowAsync();
                return default(T);
            }
        }
    }
}
