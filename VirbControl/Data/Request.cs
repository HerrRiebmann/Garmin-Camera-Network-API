using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace VirbControl.Data
{
    public enum Commands
    {
        status,
        deleteFile,
        deviceInfo,
        features,
        found,
        livePreview,
        locate,
        mediaDirList,
        mediaList,
        sensors,
        snapPicture,
        startRecording,
        stopRecording,
        stopStillRecording,
        updateFeature
    }
    public static class Request
    {
        private static string _lastErrorMessage = string.Empty;
        public static string LastErrorMessage
        {
            get { return _lastErrorMessage; }
            set { _lastErrorMessage = value; }
        }

        public static bool Submit(IPAddress ip, Commands command)
        {
            var jobj = new JObject {{"command", command.ToString()}};
            var adress = String.Format("http://{0}/virb",ip);
            var response = Send(adress, jobj.ToString());

            if (response == null)
                return false;

            foreach (var p in response.Properties())
            {
                var name = p.Name;
                var value = p.Value.ToString();
                Console.WriteLine(name + ": " + value);
                if (p.Name.Equals("result") && p.Value.ToString().Equals("1"))
                    return true;
            }
            return false;
        }

        public static JObject Send(string url, string json)
        {
            var content = string.Empty;
            try
            {
                var request = WebRequest.Create(url);
                request.ContentType = "Content-type: application/x-www-form-urlencoded";
                request.Method = "POST";

                //string credentials = "xxxxxxxxxxxxxxxxxxxxxxxx:yyyyyyyyyyyyyyyyyyyyyyyyyyyyyy";
                //CredentialCache mycache = new CredentialCache();
                //myReq.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));

                var buffer = Encoding.GetEncoding("UTF-8").GetBytes(json);
                var requestStream = request.GetRequestStream();
                requestStream.Write(buffer, 0, buffer.Length);
                requestStream.Close();


                var webRequest = request.GetResponse();
                var receiveStream = webRequest.GetResponseStream();
                var reader = new StreamReader(receiveStream, Encoding.UTF8);
                content = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                _lastErrorMessage = ex.Message;
            }
            if (string.IsNullOrEmpty(content))
                return null;

            return JObject.Parse(content);

            //Console.WriteLine(content);
            //var jsonResponse = "[" + content + "]"; // change this to array
            //var objects = JArray.Parse(jsonResponse); // parse as array  
            //foreach (var o in objects.Children<JObject>())
            //{
            //    foreach (var p in o.Properties())
            //    {
            //        var name = p.Name;
            //        var value = p.Value.ToString();
            //        Console.WriteLine(name + ": " + value);
            //    }
            //}
        }
    }
}
