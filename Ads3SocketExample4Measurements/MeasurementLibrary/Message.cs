using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasurementLibrary
{
    public class Message
    {
        public string Command { get; set; }
        public Measurements Data { get; set; }

        public Message()
        {

        }

        public Message(string command)
        {
            Command = command;
        }

        public Message(string command, Measurements data)
        {
            Command = command;
            Data = data;
        }

        public static Message CreateMessageFromJson(string jsonData)
        {
            Message message =
                JsonConvert.DeserializeObject<Message>(jsonData);

            return message;
        }

        public string ToJSON()
        {
            string s = JsonConvert.SerializeObject(this);

            return s;
        }

    }
}
