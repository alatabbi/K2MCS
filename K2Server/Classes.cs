using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace K2Server
{
    public enum CommandNames { PUT, GET };

    public enum Results { OK, FAILD };

    public class Message
    {
        public string CommandName { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Result { get; set; }
        public string Text { get; set; }

        public Message(string text)
        {
            Text = text;
            Init();
        }

        public Message()
        {
        }

        void Init()
        {
            if (!IsValid())
                return;
            string[] a = Text.Trim().Split('\n');

            if (a.Length == 2 && a[0] == CommandNames.GET.ToString())
            {
                this.CommandName = a[0];
                this.Key = a[1];
            }

            else if (a.Length == 3 && a[0] == CommandNames.PUT.ToString())
            {
                this.CommandName = a[0];
                this.Key = a[1];
                this.Value = a[2];
            }
        }

        public bool IsValid()
        {
            try
            {
                Text = Text.Replace("\r", "");
                if (string.IsNullOrEmpty(Text))
                    return false;

                string[] a = Text.Trim().Split('\n');
                return (a.Length == 2 && a[0] == "GET") || (a.Length == 3 && a[0] == "PUT");

            }
            catch (Exception)
            {
                return false;
            }

        }

        public string Serialize()
        {
            try
            {
                return new JavaScriptSerializer().Serialize(this);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static Message Deserialize(string json)
        {
            try
            {
                var m = new JavaScriptSerializer().Deserialize<Message>(json);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(m.CommandName);
                sb.AppendLine(m.Key);
                sb.AppendLine(m.Value);
                m.Text = sb.ToString();
                return m;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public override string ToString()
        {
            if (this.CommandName == CommandNames.PUT.ToString())
            {
                return this.Result;
            }
            else if (this.CommandName == CommandNames.GET.ToString())
            {
                return this.Value;
            }
            return "Error";

        }

    }
}
