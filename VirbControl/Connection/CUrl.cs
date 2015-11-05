using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace VirbControl.Connection
{
    public class CUrl
    {
        public void Send(string url, string json)
        {
            WebRequest myReq = WebRequest.Create(url);
            myReq.ContentType = "Content-type: application/x-www-form-urlencoded";
            myReq.Method = "POST";

            //string credentials = "xxxxxxxxxxxxxxxxxxxxxxxx:yyyyyyyyyyyyyyyyyyyyyyyyyyyyyy";
            //CredentialCache mycache = new CredentialCache();
            //myReq.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
            
            byte[] buffer = Encoding.GetEncoding("UTF-8").GetBytes(json);
            Stream reqstr = myReq.GetRequestStream();
            reqstr.Write(buffer, 0, buffer.Length);
            reqstr.Close();


            WebResponse wr = myReq.GetResponse();
            Stream receiveStream = wr.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            string content = reader.ReadToEnd();
            Console.WriteLine(content);
            var jsonResponse = "[" + content + "]"; // change this to array
            var objects = JArray.Parse(jsonResponse); // parse as array  
            foreach (JObject o in objects.Children<JObject>())
            {
                foreach (JProperty p in o.Properties())
                {
                    string name = p.Name;
                    string value = p.Value.ToString();
                    Console.WriteLine(name + ": " + value);
                }
            }
        }
    }
}
