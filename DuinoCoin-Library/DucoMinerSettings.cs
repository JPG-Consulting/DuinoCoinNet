using DuinoCoin.Resources;
using Newtonsoft.Json;
using System;
using System.IO;

namespace DuinoCoin
{
    /// <summary>
    /// Represents the settings of a duino-coin miner.
    /// </summary>
    public sealed class DucoMinerSettings
    {
        /// <summary>
        /// Local variable to store the duino-coin username.
        /// </summary>
        private string _username = null;

        /// <summary>
        /// Gets or sets the Duino-Coin username.
        /// </summary>
        /// <value>Duino-coin username.</value>
        public string Username 
        {
            get { return this._username; }
            set 
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new System.ArgumentException(StringResources.Exception_UsernameRequired);

                this._username = value.Trim();
            }
        }

        /// <summary>
        /// Gets or sets the name of this rig.
        /// </summary>
        /// <value>The name of this rig.</value>
        public string RigName { get; set; }

        public static DucoMinerSettings Create()
        {
            DucoMinerSettings settings = new DucoMinerSettings();

            string path = Path.Combine(AppContext.BaseDirectory, "minersettings.json");

            if (File.Exists(path))
            {
                try
                {
                    string json = File.ReadAllText(path);

                    settings = JsonConvert.DeserializeObject(json, typeof(DucoMinerSettings)) as DucoMinerSettings;
                }
                catch
                {
                    // Silence exceptions as if the file does not exist.
                }
            }

            return (settings != null) ? settings : new DucoMinerSettings();
        }

        public void Save()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "minersettings.json");

            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                StreamWriter writer = new StreamWriter(stream);

                string json = JsonConvert.SerializeObject(this);

                writer.Write(json);

                writer.Flush();
            }
        }

    }
}
