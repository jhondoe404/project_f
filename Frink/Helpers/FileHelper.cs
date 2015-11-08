using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Frink.Helpers
{
    class FileHelper
    {
        /// <summary>
        ///     Check to see if the file exists or not
        /// </summary>
        /// <param name="storagefolder">StorageFolder</param>
        /// <param name="filename">string</param>
        /// <returns>StorageFile or null</returns>
        public static async Task<StorageFile> ValidateFile(StorageFolder storagefolder, string filename)
        {
            try
            {
                return await storagefolder.GetFileAsync(filename);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        ///     Get data from localSettings, if any
        /// </summary>
        /// <param name="result">string</param>
        /// <param name="storagefolder">StorageFolder</param>
        /// <param name="filename">string</param>
        /// <param name="passphrase">string</param>
        /// <returns>string</returns>
        public static async Task<string> GetFromLocal(string result, StorageFolder storagefolder, string filename,
            string passphrase)
        {
            try
            {
                var sampleFile = await GetFile(storagefolder, filename);
                var readData = await FileIO.ReadTextAsync(sampleFile, Windows.Storage.Streams.UnicodeEncoding.Utf8);
                return await ValidateStoredData(result, sampleFile, readData, passphrase);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        /// <summary>
        ///     Validates received data (if any) and encoded data already stored
        ///     locally (if any)
        /// </summary>
        /// <param name="result">string</param>
        /// <param name="sampleFile">StorageFile</param>
        /// <param name="readData">string</param>
        /// <param name="passphrase">string</param>
        /// <returns></returns>
        private static async Task<string> ValidateStoredData(string result, StorageFile sampleFile, string readData,
            string passphrase)
        {
            var encodedData = EncryptHelper.AES_Encrypt(result, passphrase);

            if (result != null && !result.Equals("") && !encodedData.Equals(readData))
            {
                await FileIO.WriteTextAsync(sampleFile, encodedData);
                return result;
            }

            return EncryptHelper.AES_Decrypt(readData, passphrase);
        }

        /// <summary>
        ///     Loads file from appropriate storage folder
        /// </summary>
        /// <param name="folder">StorageFolder</param>
        /// <param name="filename">string</param>
        /// <returns>StorageFile</returns>
        public static async Task<StorageFile> GetFile(StorageFolder folder, string filename)
        {
            return await folder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
        }

        /// <summary>
        ///     Deletes all of the files in the passed list
        /// </summary>
        /// <param name="items">IReadOnlyList<IStorageItem></param>
        public static async void ClearFiles(IReadOnlyList<IStorageItem> items)
        {
            foreach (var item in items)
                if (item is StorageFile) await item.DeleteAsync();
        }
    }
}
