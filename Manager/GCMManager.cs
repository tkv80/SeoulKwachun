using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;

namespace SeoulKwachun.Manager
{
    static class GcmManager
    {
        private const string DeviceId = "APA91bEjnv2-SfcFU441y__4LZycOmHNUFN9hBjsi24B3QRyTXT6sQ4TcL8ehpvX9Sn74B9UkOiIFI-Amj-mMOJDhjOL5mvhzohW-n41bbR8BDVO9ChuQjG7VXJ2tv-O5fWeAq8I21_AgF5_2DHMAKV1CuIYfrMtcA";
        private const string ApiKey = "AIzaSyCcFDylElWuJkVPvG688WXPxYttV8GHoGM";

        private const string TickerText = "cmaping GCM";

        public static void SendNotification(string message, string contentTitle)
        {
            var postData =
        "{ \"registration_ids\": [ \"" + DeviceId + "\" ], " +
          "\"data\": {\"GameName\":\"" + TickerText + "\", " +
                     "\"FTitle\":\"" + contentTitle + "\", " +
                     "\"FContent\":\"" + contentTitle + "\", " +
                     "\"PromotionCopy\": \"" + message + "\"}}";
            SendGcmNotification(ApiKey, postData);
        }


        /// <summary>
        /// Send a Google Cloud Message. Uses the GCM service and your provided api key.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="postData"></param>
        /// <param name="postDataContentType"></param>
        /// <returns>The response string from the google servers</returns>
        private static void SendGcmNotification(string apiKey, string postData, string postDataContentType = "application/json")
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateServerCertificate);

            //
            //  MESSAGE CONTENT
            var byteArray = Encoding.UTF8.GetBytes(postData);

            //
            //  CREATE REQUEST
            var request = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
            request.Method = "POST";
            request.KeepAlive = false;
            request.ContentType = postDataContentType;
            request.Headers.Add(string.Format("Authorization: key={0}", apiKey));
            request.ContentLength = byteArray.Length;

            var dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            //
            //  SEND MESSAGE
            try
            {
                var response = request.GetResponse();
                var responseCode = ((HttpWebResponse)response).StatusCode;
                if (responseCode.Equals(HttpStatusCode.Unauthorized) || responseCode.Equals(HttpStatusCode.Forbidden))
                {
                    var text = "Unauthorized - need new token";
                }
                else if (!responseCode.Equals(HttpStatusCode.OK))
                {
                    var text = "Response from web service isn't OK";
                }

                var reader = new StreamReader(response.GetResponseStream());
                var responseLine = reader.ReadToEnd();
                reader.Close();

                return;
            }
            catch (Exception ex)
            {
                Util.ErrorLog(System.Reflection.MethodBase.GetCurrentMethod(), ex, "에러");
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private static bool ValidateServerCertificate(
                                                    object sender,
                                                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                                                    SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

    }
}