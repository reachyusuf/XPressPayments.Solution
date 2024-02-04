using Flurl;
using Flurl.Http;

namespace XPressPayments.Common.Helpers
{
    public class FlurlHttpClientUtil
    {
        public const int ConfigsX_REST_REQUEST_TIMEOUT = 60;

        public static async Task<T> GetJSON<T>(string path, object queryParams = null, object headers = null, object cookies = null)
        {
            try
            {
                var result = await new Url(path)
                   .SetQueryParams(queryParams ?? new { })
                   .WithCookies(cookies ?? new { })
                   //.WithTimeout(ConfigsX_REST_REQUEST_TIMEOUT)
                   .WithHeaders(headers ?? new { })
                   .GetAsync().ReceiveJson<T>();
                return result;
            }
            catch (FlurlHttpException ex)
            {
                var response = ex.GetResponseJsonAsync<T>().Result;
                return response;
            }
            catch (TaskCanceledException)
            {
                return default(T);
            }
            catch (Exception ex)
            {
                var errorMsg = ex.Message;
                return default(T);
            }
        }
        public static async Task<string> GetString(string path, object queryParams = null, object headers = null,
            object cookies = null)
        {
            try
            {
                var result = await new Url(path)
                    .SetQueryParams(queryParams ?? new { })
                    .WithCookies(cookies ?? new { })
                    .WithTimeout(ConfigsX_REST_REQUEST_TIMEOUT)
                    .WithHeaders(headers ?? new { })
                    .GetStringAsync();
                return result;
            }
            catch (FlurlHttpException ex)
            {
                var response = ex.GetResponseJsonAsync<string>().Result;
                return response;
            }
            catch (TaskCanceledException)
            {
                return string.Empty;
            }
            catch (IOException)
            {
                return string.Empty;
            }
        }
        public static async Task<T> PostJSON<T>(string path, object payload = null, object headers = null, object cookies = null)
        {
            try
            {
                var result = await new Url(path)
                    .WithTimeout(ConfigsX_REST_REQUEST_TIMEOUT)
                    .WithCookies(cookies ?? new { })
                    .WithHeaders(headers ?? new { })
                    .PostJsonAsync(payload ?? new object()).ReceiveJson<T>();
                return result;
            }
            catch (FlurlHttpException ex)
            {
                var statuscode = ex.StatusCode;
                var response = ex.GetResponseJsonAsync<T>().Result;
                return response;
            }
            catch (TaskCanceledException)
            {
                return default(T);
            }
            catch (IOException)
            {
                return default(T);
            }
        }
        public static async Task<string> PostJSONForString(string path, object payload = null, object headers = null, object cookies = null)
        {
            try
            {
                var result = await new Url(path)
                    .WithTimeout(ConfigsX_REST_REQUEST_TIMEOUT)
                    .WithCookies(cookies ?? new { })
                    .WithHeaders(headers ?? new { })
                    .PostJsonAsync(payload ?? new object()).ReceiveString();
                return result;
            }
            catch (FlurlHttpException ex)
            {
                //var response = ex.GetResponseJsonAsync<string>().Result;
                var response = ex.GetResponseStringAsync().Result;
                return response;
            }
            catch (TaskCanceledException)
            {
                return string.Empty;
            }
            catch (IOException)
            {
                return string.Empty;
            }
        }
        public static async Task PostJSONAsync(string path, object payload = null, object headers = null,
                                                  object cookies = null)
        {
            try
            {
                await new Url(path).WithCookies(cookies ?? new { }).WithTimeout(ConfigsX_REST_REQUEST_TIMEOUT)
                                  .WithHeaders(headers ?? new { })
                                   .PostJsonAsync(payload ?? new object());
            }
            catch (TaskCanceledException)
            {

            }
            catch (IOException)
            {

            }
        }
        public static async Task<String> UploadByteArrayAsync(string path, byte[] imageBytes, byte[] secondaryImageBytes, string token, ICollection<KeyValuePair<String, String>> payload = null)
        {
            try
            {
                using (Stream primaryFileStream = new MemoryStream(imageBytes))
                {
                    using (Stream secondaryFileStream = secondaryImageBytes == null ? new MemoryStream() : new MemoryStream(secondaryImageBytes))
                        return await new Url(path).WithTimeout(ConfigsX_REST_REQUEST_TIMEOUT).PostMultipartAsync((mp) =>
                        {
                            mp.AddFile("File", primaryFileStream, "my_uploaded_image.jpg");
                            if (secondaryImageBytes is not null)
                            {
                                mp.AddFile("BackFile", secondaryFileStream, "my_secondary_uploaded_image.jpg");
                            }
                            if (payload is not null)
                            {
                                foreach (var item in payload)
                                {
                                    mp.AddString(item.Key, item.Value);
                                }
                            }
                        }).ReceiveJson<String>();
                }
            }
            catch (FlurlHttpException ex)
            {
                var response = ex.GetResponseJsonAsync<string>().Result;
                return response;
            }
            catch (TaskCanceledException)
            {
                return string.Empty;
            }
            catch (IOException)
            {
                return string.Empty;
            }
        }
        public static async Task<T> PostUrlEncodedAsync<T>(string path, object payload = null, object headers = null,
                                                  object cookies = null)
        {
            try
            {
                return await new Url(path).WithCookies(cookies ?? new { }).WithTimeout(ConfigsX_REST_REQUEST_TIMEOUT)
                                   .WithHeaders(headers ?? new { })
                                          .PostUrlEncodedAsync(payload ?? new object()).ReceiveJson<T>();
            }
            catch (FlurlHttpException ex)
            {
                var response = ex.GetResponseJsonAsync<T>().Result;
                return response;
            }
            catch (TaskCanceledException)
            {
                return default(T);
            }
            catch (IOException)
            {
                return default(T);
            }
        }
        public async Task PostUrlEncodedAsync(string path, object payload = null, object headers = null,
                                                  object cookies = null)
        {
            try
            {
                await new Url(path).WithCookies(cookies ?? new { }).WithTimeout(ConfigsX_REST_REQUEST_TIMEOUT)
                                  .WithHeaders(headers ?? new { })
                                   .PostUrlEncodedAsync(payload ?? new object());
            }
            catch (TaskCanceledException)
            {

            }
            catch (IOException)
            {

            }
        }
        public T PutJSONAsync<T>(string path, object payload = null, object headers = null, object cookies = null)
        {
            try
            {
                return new Url(path).WithCookies(cookies ?? new { }).WithTimeout(ConfigsX_REST_REQUEST_TIMEOUT)
                                   .WithHeaders(headers ?? new { })
                                          .PutJsonAsync(payload ?? new object()).ReceiveJson<T>().Result;
            }
            catch (FlurlHttpException ex)
            {
                var response = ex.GetResponseJsonAsync<T>().Result;
                return response;
            }
            catch (TaskCanceledException)
            {
                return default(T);
            }
        }

        public async Task PutJSONAsync(string path, object payload = null, object headers = null,
                                                  object cookies = null)
        {
            try
            {
                await new Url(path).WithCookies(cookies ?? new { }).WithTimeout(ConfigsX_REST_REQUEST_TIMEOUT)
                                  .WithHeaders(headers ?? new { })
                                   .PutJsonAsync(payload ?? new object());
            }
            catch (TaskCanceledException)
            {

            }
            catch (IOException)
            {

            }
        }
        public T DeleteAsync<T>(string path,
                               object queryParams = null, object headers = null,
                              object cookies = null)
        {
            try
            {
                return new Url(path)
                         .SetQueryParams(queryParams ?? new { })
                    .WithCookies(cookies ?? new { }).WithTimeout(ConfigsX_REST_REQUEST_TIMEOUT)
                                   .WithHeaders(headers ?? new { })
                    .DeleteAsync().ReceiveJson<T>().Result;
            }
            catch (TaskCanceledException)
            {
                return default(T);
            }
            catch (IOException)
            {
                return default(T);
            }
        }
        public async Task DeleteAsync(string path,
                               object queryParams = null, object headers = null,
                              object cookies = null)
        {
            try
            {
                await new Url(path)
                   .SetQueryParams(queryParams ?? new { }).WithTimeout(ConfigsX_REST_REQUEST_TIMEOUT)
                   .WithCookies(cookies ?? new { })
                                  .WithHeaders(headers ?? new { })
                                         .DeleteAsync();
            }
            catch (TaskCanceledException)
            {

            }
            catch (IOException)
            {

            }
        }
        public async Task<byte[]> GetBytesAsync(string path,
                               object queryParams = null, object headers = null,
                              object cookies = null)
        {
            try
            {
                return await new Url(path)
                   .SetQueryParams(queryParams ?? new { }).WithTimeout(ConfigsX_REST_REQUEST_TIMEOUT)
                   .WithCookies(cookies ?? new { })
                    .WithHeaders(headers ?? new { })
                    .GetBytesAsync();
            }
            catch (TaskCanceledException)
            {
                return default(byte[]);
            }
            catch (IOException)
            {
                return default(byte[]);
            }
        }

    }

}
