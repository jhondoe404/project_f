using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Frink.Helpers
{
    public static class FileHelper
    {
        #region FILE HELPERS





        /// <summary>
        ///     Check to see if the file exists or not
        /// </summary>
        /// <param name="storagefolder">StorageFolder</param>
        /// <param name="filename">string</param>
        /// <returns>StorageFile or null</returns>
        public static async Task<bool> ValidateFile(StorageFolder storagefolder, string filename)
        {
            try
            {
                await storagefolder.GetFileAsync(filename);
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
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


        /// <summary>
        /// Reads a string from a text file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
        public static async Task<string> ReadFromFile(string fileName, StorageFolder folder = null)
        {
            folder = folder ?? ApplicationData.Current.LocalFolder;
            var file = await folder.GetFileAsync(fileName);

            using (var fs = await file.OpenAsync(FileAccessMode.Read))
            {
                using (var inStream = fs.GetInputStreamAt(0))
                {
                    using (var reader = new DataReader(inStream))
                    {
                        await reader.LoadAsync((uint)fs.Size);
                        string data = reader.ReadString((uint)fs.Size);
                        reader.DetachStream();

                        return data;
                    }
                }
            }
        }

        /// <summary>
        /// Writes a string to a text file.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="options">
        /// The enum value that determines how responds if the fileName is the same
        /// as the name of an existing file in the current folder. Defaults to ReplaceExisting.
        /// </param>
        /// <returns></returns>
        public static async Task WriteToFile(this string text, string fileName, StorageFolder folder = null, CreationCollisionOption options = CreationCollisionOption.ReplaceExisting)
        {
            folder = folder ?? ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(fileName, options);
            using (var fs = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (var outStream = fs.GetOutputStreamAt(0))
                {
                    using (var dataWriter = new DataWriter(outStream))
                    {
                        if (text != null)
                            dataWriter.WriteString(text);

                        await dataWriter.StoreAsync();
                        dataWriter.DetachStream();
                    }

                    await outStream.FlushAsync();
                }
            }
        }


        /// <summary>
        ///     Writes a byte array to a specified file
        /// </summary>
        /// <param name="data">byte array to be written</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="options">
        /// The enum value that determines how responds if the fileName is the same
        /// as the name of an existing file in the current folder. Defaults to ReplaceExisting.
        /// </param>
        /// <returns></returns>
        public static async Task WriteToFile(byte[] data, string fileName, StorageFolder folder = null, CreationCollisionOption options = CreationCollisionOption.ReplaceExisting)
        {
            folder = folder ?? ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(fileName, options);
            await FileIO.WriteBytesAsync(file, data);
        }





        #endregion
        #region HTTP FILE HELPERS


        /// <summary>
        ///     Writes a http request to a file, encrypted, with it's etag
        /// </summary>
        /// <param name="body">body of the http request to store</param>
        /// <param name="file">file to which the http request will be written to</param>
        /// <param name="etag">etag of the http request for the validation</param>
        /// <returns></returns>
        async public static Task writeHttpToFile(string body, string file, string etag = null)
        {
            string bodyToSave = body + ConstantsHelper.API_ETAG_CACHE_DELIMITER + etag;
            var encrypted = EncryptHelper.AES_Encrypt(bodyToSave, ConstantsHelper.LOCALE_PASSWORD);
            await WriteToFile(encrypted, file);
        }


        /// <summary>
        ///     Reads http response and etag, if any, from a file
        /// </summary>
        /// <param name="file">file to read from</param>
        /// <returns>array of decrypted data from the file. if there's any, 0 index is the body, 1 index is the etag</returns>
        async public static Task<String[]> readHttpFromFile(string file)
        {
            String readfile = await FileHelper.ReadFromFile(file);
            readfile = EncryptHelper.AES_Decrypt(readfile, ConstantsHelper.LOCALE_PASSWORD);

            String[] separators = new String[1];
            separators[0] = ConstantsHelper.API_ETAG_CACHE_DELIMITER;
            String[] readfilesplit = readfile.Split(separators, StringSplitOptions.RemoveEmptyEntries);

#if DEBUG
            Debug.WriteLine("[FileHelper][ReadHtpFromFile] split: " + readfilesplit[0]);
            Debug.WriteLine("[FileHelper][ReadHtpFromFile] split: " + readfilesplit[1]);
#endif

            return readfilesplit;
        }





        #endregion

    }
}