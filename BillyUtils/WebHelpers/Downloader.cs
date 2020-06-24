using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BillyUtils.WebHelpers {
    public static class Downloader {

        public static Stream DownloadStream(string url) {
            WebRequest request = WebRequest.Create(url);
            using(WebResponse response = request.GetResponse()) {
                return response.GetResponseStream();
            }
        }

        public static async Task<Stream> DownloadStreamAsync(string url) {
            WebRequest request = WebRequest.Create(url);
            using(var response = await request.GetResponseAsync()) {
                return response.GetResponseStream();
            }
        }

        public static byte[] DownloadBytes(string url) {
            using(var wc = new WebClient()) {
                return wc.DownloadData(url);
            }
        }

        public static async Task<byte[]> DownloadBytesAsync(string url) {
            using(var wc = new WebClient()) {
                return await wc.DownloadDataTaskAsync(new Uri(url));
            }
        }

        public static void DownloadToFile(string url, string downloadPath) {
            using(var wc = new WebClient()) {
                File.Create(downloadPath).Close();
                wc.DownloadFile(url, downloadPath);
            }
        }

        public static async void DownloadToFileAsync(string url, string downloadPath) {
            using(var wc = new WebClient()) {
                File.Create(downloadPath).Close();
                await wc.DownloadFileTaskAsync(url, downloadPath);
            }
        }
    }
}