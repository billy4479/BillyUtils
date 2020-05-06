using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using System;
using System.Text.Json;

namespace BillyUtils
{
    public static class HTTPRequest
    {
        public static async Task<string> GET(Uri uri)
        {
            HttpWebRequest request = WebRequest.CreateHttp(uri);
            request.Method = "GET";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
        public static async Task<string> GET(string uri)
        {
            return await GET(new Uri(uri));
        }
        public static async Task<string> POST(Uri uri, string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            HttpWebRequest request = WebRequest.CreateHttp(uri);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = dataBytes.Length;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using(Stream stream = await request.GetRequestStreamAsync())
            {
                await stream.WriteAsync(dataBytes, 0, dataBytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }

        }
        public static async Task<string> POST(string uri, string data)
        {
            return await POST(new Uri(uri), data);
        }
        public static async Task<string> POST(Uri uri, object data)
        {
            return await POST(uri, JsonSerializer.Serialize(data));
        }
        public static async Task<string> POST(string uri, object data)
        {
            return await POST(new Uri(uri), JsonSerializer.Serialize(data));
        }
    }
}