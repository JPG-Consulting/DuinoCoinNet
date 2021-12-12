using System;

namespace DuinoCoin
{
    public class DucoPoolInformation
    {
        [Newtonsoft.Json.JsonProperty("name")]
        public string Name { get; private set; }

        [Newtonsoft.Json.JsonProperty("ip")]
        public String Ip { get; private set; }

        [Newtonsoft.Json.JsonProperty("port")]
        public int Port { get; private set; }

        [Newtonsoft.Json.JsonProperty("connections")]
        public int Connections { get; private set; }

        public static DucoPoolInformation GetPool()
        {
            DucoPoolInformation poolInformation = null;

            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                string json = client.DownloadString("http://51.15.127.80:4242/getPool");

                poolInformation = Newtonsoft.Json.JsonConvert.DeserializeObject<DucoPoolInformation>(json);
            }

            return poolInformation;
        }
    }
}
