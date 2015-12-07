using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AINT354_Mobile_API.BusinessLogic
{
    public class AndroidGCMPushNotification
    {
        public AndroidGCMPushNotification()
        {
            
        }
        public string SendNotification(string deviceId, string message)
        {
            string SERVER_API_KEY = "AIzaSyCPn-39zi5pljrmhQJgj2PLHUnT_CRJCwo";
            var SENDER_ID = "197149327166";
            var value = message;
            var tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
            tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_API_KEY));

            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

            string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + value + "&data.time=" + DateTime.Now + "&registration_id=" + deviceId + "";
            Console.WriteLine(postData);
            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            tRequest.ContentLength = byteArray.Length;

            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();

            dataStream = tResponse.GetResponseStream();

            StreamReader tReader = new StreamReader(dataStream);

            String sResponseFromServer = tReader.ReadToEnd();


            tReader.Close();
            dataStream.Close();
            tResponse.Close();
            return sResponseFromServer;
        }
    }
}
