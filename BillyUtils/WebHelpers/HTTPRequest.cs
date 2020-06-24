using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BillyUtils.WebHelpers {
    public static class HTTPRequest {

        public static async Task<string> GETAsync(Uri uri) {
            HttpWebRequest request = WebRequest.CreateHttp(uri);
            request.Method = "GET";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using(HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync())
            using(Stream stream = response.GetResponseStream())
            using(StreamReader reader = new StreamReader(stream)) {
                return await reader.ReadToEndAsync();
            }
        }
        public static async Task<string> GETAsync(string uri) {
            return await GETAsync(new Uri(uri));
        }

        public static string GET(Uri uri) {
            HttpWebRequest request = WebRequest.CreateHttp(uri);
            request.Method = "GET";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using(HttpWebResponse response = (HttpWebResponse) request.GetResponse())
            using(Stream stream = response.GetResponseStream())
            using(StreamReader reader = new StreamReader(stream)) {
                return reader.ReadToEnd();
            }
        }
        public static string GET(string uri) {
            return GET(new Uri(uri));
        }

        public static async Task<string> POSTAsync(Uri uri, string data) {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            HttpWebRequest request = WebRequest.CreateHttp(uri);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = dataBytes.Length;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using(Stream stream = await request.GetRequestStreamAsync()) {
                await stream.WriteAsync(dataBytes, 0, dataBytes.Length);
            }

            using(HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync())
            using(Stream stream = response.GetResponseStream())
            using(StreamReader reader = new StreamReader(stream)) {
                return await reader.ReadToEndAsync();
            }

        }
        public static async Task<string> POSTAsync(string uri, string data) {
            return await POSTAsync(new Uri(uri), data);
        }
        public static async Task<string> POSTAsync(Uri uri, object data) {
            return await POSTAsync(uri, JsonSerializer.Serialize(data));
        }
        public static async Task<string> POSTAsync(string uri, object data) {
            return await POSTAsync(new Uri(uri), JsonSerializer.Serialize(data));
        }

        public static string POST(Uri uri, string data) {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            HttpWebRequest request = WebRequest.CreateHttp(uri);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = dataBytes.Length;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using(Stream stream = request.GetRequestStream()) {
                stream.Write(dataBytes, 0, dataBytes.Length);
            }

            using(HttpWebResponse response = (HttpWebResponse) request.GetResponse())
            using(Stream stream = response.GetResponseStream())
            using(StreamReader reader = new StreamReader(stream)) {
                return reader.ReadToEnd();
            }

        }
        public static string POST(string uri, string data) {
            return POST(new Uri(uri), data);
        }
        public static string POST(Uri uri, object data) {
            return POST(uri, JsonSerializer.Serialize(data));
        }
        public static string POST(string uri, object data) {
            return POST(new Uri(uri), JsonSerializer.Serialize(data));
        }
    }
}