using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.Web.Http;
using Windows.Storage.Streams;
using Windows.Web.Http.Headers;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using System.IO;
using System.Net.NetworkInformation;

namespace BackgroundRest
{
    public sealed class RestfulAPI : IBackgroundTask
    {
        #region CLASS PARAMETERS


        const string LAST_RUN_TIME_SETTING = "lsr";
        const string LAST_RUN_TIME_DEFAULT = "not run";
        const int TIME_DELAY = 3000;

        const String API_APP_ID = "1";
        const String API_HOST_URL = "http://frink-dev.smartfactory.ch:1337/api";
        const String API_ETAG = "ETag";
        const String API_METHOD_MENU = "/app";

        private const String API_ETAG_CACHE_DELIMITER = " =#=! ";
        private const String LOCALE_PASSWORD = "thisisanawasomecoolpassword";
        private const String LOCAL_FILE_APPLICATION_THEME = "apptheme.txt";


        #endregion
        #region BACKGROUND TASK



        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            bool cancelled = false;

            BackgroundTaskCanceledEventHandler handler = (s, e) =>
            {
                cancelled = true;
            };

            for (uint i = 0; !cancelled; i++)
            {
                taskInstance.Progress = 0;

                // Values to be used in the task
                string eTag = null;

                HttpClient request = new HttpClient();
                Uri connectionUri = new Uri(API_HOST_URL + API_METHOD_MENU + "/" + API_APP_ID);

                bool fileExists = await ValidateFile(ApplicationData.Current.TemporaryFolder, LOCAL_FILE_APPLICATION_THEME);
                if (fileExists)
                {
                    string[] read = await readHttpFromFile(LOCAL_FILE_APPLICATION_THEME);
                    if (read.Length > 0)
                        eTag = read[1];
                }

                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    taskInstance.Progress = 1;
                    if (eTag != null)
                        request.DefaultRequestHeaders.Add(new KeyValuePair<string, string>(API_ETAG, eTag));

                    // Extract the data
                    HttpResponseMessage response = await request.GetAsync(connectionUri);
                    IBuffer stream = await response.Content.ReadAsBufferAsync();
                    taskInstance.Progress = 2;

                    if (response.Headers.ContainsKey(API_ETAG) && response.Headers[API_ETAG] != eTag)
                    {
                        taskInstance.Progress = 3;
#if DEBUG
                        Debug.WriteLine("[TheTask][Run] etags are different: {0}, {1}", eTag, response.Headers[API_ETAG]);
#endif
                        eTag = response.Headers[API_ETAG];
#if DEBUG
                        Debug.WriteLine("[TheTask][Run] ¸new etag {0}", eTag);
#endif
                        if (stream.Length > 10)
                        {
                            taskInstance.Progress = 4;
#if DEBUG
                            Debug.WriteLine("[TheTask][Run] response was not null");
#endif
                            byte[] readstream = new byte[stream.Length];
                            using (var reader = DataReader.FromBuffer(stream))
                                reader.ReadBytes(readstream);

                            request.Dispose();
                            response.Dispose();

                            // Prepare data
                            String responseFormatted = System.Text.Encoding.UTF8.GetString(readstream, 0, readstream.Length);
                            string bodyToSave = responseFormatted + API_ETAG_CACHE_DELIMITER + eTag;
                            var encrypted = AES_Encrypt(bodyToSave, LOCALE_PASSWORD);
                            taskInstance.Progress = 5;
#if DEBUG
                            Debug.WriteLine("[TheTask][Run] data prepared decyrpted: {0}, encrypted: {1}",
                                bodyToSave, encrypted);
#endif

                            // Write to file
                            taskInstance.Progress = 6;
                            var folder = ApplicationData.Current.TemporaryFolder;
                            var file = await folder.CreateFileAsync(LOCAL_FILE_APPLICATION_THEME, CreationCollisionOption.ReplaceExisting);
                            using (var fs = await file.OpenAsync(FileAccessMode.ReadWrite))
                            {
                                using (var outStream = fs.GetOutputStreamAt(0))
                                {
                                    using (var dataWriter = new DataWriter(outStream))
                                    {
                                        if (encrypted != null)
                                            dataWriter.WriteString(encrypted);

                                        await dataWriter.StoreAsync();
                                        dataWriter.DetachStream();
                                    }

                                    await outStream.FlushAsync();
                                }
                            }
#if DEBUG
                            Debug.WriteLine("[TheTask][Run] Data written to file");
#endif
                        }
                    }
                }

                taskInstance.Progress = 7;
                await Task.Delay(TIME_DELAY);
            }

            ApplicationData.Current.LocalSettings.Values[LAST_RUN_TIME_SETTING] = DateTimeOffset.Now;
            deferral.Complete();
        }



        #endregion
        #region COMMUNICATION METHODS



        public static string LastRunTime
        {
            get
            {
                object outValue = null;
                string lastRunTime = LAST_RUN_TIME_DEFAULT;

                if (ApplicationData.Current.LocalSettings.Values.TryGetValue(
                  LAST_RUN_TIME_SETTING, out outValue))
                {
                    DateTimeOffset dateTime = (DateTimeOffset)outValue;
                    lastRunTime = dateTime.ToString("f");
                }
                return (lastRunTime);
            }
        }



        public static void OnDestroy()
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(LAST_RUN_TIME_SETTING))
            {
                ApplicationData.Current.LocalSettings.Values.Remove(LAST_RUN_TIME_SETTING);
            }
        }



        #endregion
        #region FILE HELPER METHODS


        /// <summary>
        /// Reads a string from a text file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
        private static async Task<string> ReadFromFile(string fileName, StorageFolder folder = null)
        {
            folder = folder ?? ApplicationData.Current.TemporaryFolder;
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



        #endregion
        #region HTTP FILE HELPERS



        /// <summary>
        ///     Check to see if the file exists or not
        /// </summary>
        /// <param name="storagefolder">StorageFolder</param>
        /// <param name="filename">string</param>
        /// <returns>StorageFile or null</returns>
        async private static Task<bool> ValidateFile(StorageFolder storagefolder, string filename)
        {
            try
            {
                await storagefolder.GetFileAsync(filename);
                return true;
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("[Task][ValidateFile] file not found");
                return false;
            }
        }


        /// <summary>
        ///     Reads http response and etag, if any, from a file
        /// </summary>
        /// <param name="file">file to read from</param>
        /// <returns>array of decrypted data from the file. if there's any, 0 index is the body, 1 index is the etag</returns>
        async private static Task<String[]> readHttpFromFile(string file)
        {
            String readfile = await ReadFromFile(file);
            readfile = AES_Decrypt(readfile, LOCALE_PASSWORD);

            String[] separators = new String[1];
            separators[0] = API_ETAG_CACHE_DELIMITER;
            String[] readfilesplit = readfile.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            return readfilesplit;
        }



        #endregion
        #region ENCRYPT HELPERS


        /// <summary>
        ///     Basic AES encryption function
        /// </summary>
        /// <param name="input">string to be encrypted</param>
        /// <param name="pass">encryption key</param>
        /// <returns></returns>
        private static string AES_Encrypt(string input, string pass)
        {
            var sap = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
            var hap = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            var hashaes = hap.CreateHash();

            try
            {
                var hash = new byte[32];
                hashaes.Append(CryptographicBuffer.CreateFromByteArray(Encoding.UTF8.GetBytes(pass)));
                byte[] temp;
                CryptographicBuffer.CopyToByteArray(hashaes.GetValueAndReset(), out temp);

                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);

                var aes = sap.CreateSymmetricKey(CryptographicBuffer.CreateFromByteArray(hash));

                var buffer = CryptographicBuffer.CreateFromByteArray(Encoding.UTF8.GetBytes(input));
                return CryptographicBuffer.EncodeToBase64String(CryptographicEngine.Encrypt(aes, buffer, null));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }


        /// <summary>
        ///     Decryption function
        /// </summary>
        /// <param name="input">string to be decrypted</param>
        /// <param name="pass">decryption key</param>
        /// <returns></returns>
        private static string AES_Decrypt(string input, string pass)
        {
            var sap = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
            var hap = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            var hashaes = hap.CreateHash();

            try
            {
                var hash = new byte[32];
                hashaes.Append(CryptographicBuffer.CreateFromByteArray(Encoding.UTF8.GetBytes(pass)));
                byte[] temp;
                CryptographicBuffer.CopyToByteArray(hashaes.GetValueAndReset(), out temp);

                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);

                var aes = sap.CreateSymmetricKey(CryptographicBuffer.CreateFromByteArray(hash));

                var buffer = CryptographicBuffer.DecodeFromBase64String(input);
                byte[] decryptedByteArray;
                CryptographicBuffer.CopyToByteArray(CryptographicEngine.Decrypt(aes, buffer, null),
                    out decryptedByteArray);
                return Encoding.UTF8.GetString(decryptedByteArray, 0, decryptedByteArray.Length);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }


        #endregion
    }
}
