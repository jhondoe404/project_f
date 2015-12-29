using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.Diagnostics;
using Windows.Web.Http;
using Windows.Storage.Streams;
using Windows.Web.Http.Headers;
using Frink.Helpers;

namespace Frink.Rest
{
    class AsyncTask
    {
        #region PROPERTIES





        public string                       _url;
        string                              _method;
        string                              _contentType;
        string                              _etag;   
        Dictionary<string, string>          _body;
        public HttpResponseHeaderCollection _header;
        public const string                 Post = "POST";
        public const string                 Get = "GET";
        public const string                 Put = "PUT";
        public const string                 Delete = "DELETE";





        #endregion
        #region CLASS CONSTRUCTION





        public AsyncTask ()
        {
            _method = Get;
            _contentType = "text/*";
        }


        /// <summary>
        ///     Sets the URL to which the HTTP request will be executed
        /// </summary>
        /// <param name="url">string</param>
        /// <returns>this</returns>
        public AsyncTask setUrl(string url)
        {
            if (url != null && !url.Equals(""))
                _url = url;
            return this;
        }


        /// <summary>
        ///     Will set the type of the HTTP request (POST, GET..).
        ///     Use constants from the class: Request.POST...
        ///     If the _method is not invoked or empty parameter is passed,
        ///     the default will be used (GET)
        /// </summary>
        /// <param name="method">string</param>
        /// <returns>this</returns>
        public AsyncTask setMethod(string method)
        {
            if (method != null && !method.Equals(""))
            {
                _method = method;
                if (!_method.Equals(Get)) setDefaultContentType();
            }

            return this;
        }


        /// <summary>
        ///     Sets the default content type header for the specified header
        /// </summary>
        void setDefaultContentType()
        {
            if (_method.Equals(Get))
                _contentType = "text/*";
            if (_method.Equals(Post) || _method.Equals(Put))
                _contentType = "application/x-www-form-urlencoded";
            if (_method.Equals(Delete))
                _contentType = "X-HTTP-Method-Override: DELETE";
        }


        /// <summary>
        ///     Set a custom Content Type for the header of the request.
        ///     If the _method is not invoked or empty parameters were
        ///     passed, it will use default content types based on the
        ///     set _method of communication.
        /// </summary>
        /// <param name="contenttype">string</param>
        /// <returns></returns>
        public AsyncTask setContentType(string contenttype)
        {
            if (contenttype != null && !contenttype.Equals(""))
                _contentType = contenttype;
            return this;
        }


        /// <summary>
        ///     Sets the _body of the request
        /// </summary>
        /// <param name="body">Dictionary</param>
        /// <returns></returns>
        public AsyncTask setBody(Dictionary<string, string> body)
        {
            if (body != null)
                _body = body;
            return this;
        }


        /// <summary>
        ///     Sets the etag, if any to be used for the server validation 
        /// </summary>
        /// <param name="etag">string value to be used for the server validation</param>
        /// <returns></returns>
        public AsyncTask setETag(string etag)
        {
            if (etag != null)
                _etag = etag;
            return this;
        }





        #endregion
        #region HTTP EXECUTORS




        /// <summary>
        ///     Executes the HTTP request and gets the result in byte array
        /// </summary>
        /// <returns>byte[]</returns>
        public async Task<byte[]> execute()
        {
            try
            {               
                return await getFromWeb(ValidateBody());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[AsyncTask][GetFromWeb] Error executing HTTP request: {0}", ex.Message);
                return null;
            }
        }


        /// <summary>
        ///     Validates the _method of the HTTP request,
        ///     whether or not we're writing _body of the
        ///     request to stream
        /// </summary>
        /// <returns>byte[]</returns>
        byte[] ValidateBody()
        {
            try
            {
                if (_body != null && _body.Count > 0)
                {
                    var body = ConvertBody();

                    if (_method.Equals(Get))
                    {
                        _url += "?" + body;
                        return null;
                    }
                    else
                    {
                        return Encoding.UTF8.GetBytes(body);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[AsyncTask][ValidateBody] Error validating body of the request: {0}", ex.Message);
                return null;
            }
        }


        /// <summary>
        ///     Converts the passed dictionary to a string
        ///     usable in the HTTP request
        /// </summary>
        /// <returns>string</returns>
        string ConvertBody()
        {
            try
            {
                var body = "";
                if (_body != null && _body.Count > 0)
                {
                    foreach (var item in _body)
                        body += item.Key + "=" + item.Value + "&";

                    char[] charsToTrim = { ' ', '\'', '&' };
                    return body.TrimEnd(charsToTrim);
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[AsyncTask][ConvertBody] Error converting body arguments: {0}", ex.Message);
                return null;
            }
        }



        #endregion
        #region HELPER METHODS





        /// <summary>
        ///     Executes a HTTP request
        /// </summary>
        /// <param name="body">body of the request</param>
        /// <returns>Array of bytes</returns>
        public async Task<byte[]> getFromWeb(byte[] body)
        {
            Debug.WriteLine("[AsyncTask][getFromWeb] executing http request {0}", _url);

            HttpClient request = new HttpClient();            
            Uri connectionUri = new Uri(_url);

            HttpFormUrlEncodedContent formContent = null;
            if (body != null && body.Length > 0)
                formContent = new HttpFormUrlEncodedContent(_body);
                        
            if (_etag != null)
                request.DefaultRequestHeaders.Add(new KeyValuePair<string,string>(ConstantsHelper.API_ETAG, _etag));

            HttpResponseMessage response = null;
            if (_method.Equals(AsyncTask.Get))
                response = await request.GetAsync(connectionUri);

            if (_method.Equals(AsyncTask.Post))
                response = await request.PostAsync(connectionUri, formContent);

            if (_method.Equals(AsyncTask.Put))
                response = await request.PutAsync(connectionUri, formContent);

            if (_method.Equals(AsyncTask.Delete))
                response = await request.DeleteAsync(connectionUri);

            IBuffer stream = await response.Content.ReadAsBufferAsync();
            byte[] readstream = new byte[stream.Length];
            using (var reader = DataReader.FromBuffer(stream))
                reader.ReadBytes(readstream);

#if DEBUG
            Debug.WriteLine("[AsyncTask][getFromWeb] it contains an etag: " + response.Headers["ETag"]);
#endif
            _header = response.Headers;
            request.Dispose();
            response.Dispose();            
            return readstream;
        }



        #endregion
    }
}
