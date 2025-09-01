using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MCS.UI.TraysUISettings
{
    public class TraysConfig
    {
        private static readonly Dictionary<int, Tray> Elements;

        static TraysConfig()
        {
            Elements = new Dictionary<int, Tray>();

            var section = (TrayConfigSection)ConfigurationManager.GetSection("trays");

            if (section == null)
            {
                throw new ConfigurationErrorsException("trays section is not Configurated");
            }

            foreach (Tray system in section.Trays)
            {
                Elements.Add(system.Id, system);
            }
        }

        public static Tray GetTray(int trayId)
        {
            return Elements[trayId];
        }

        public static List<Tray> Trays
        {
            get { return Elements.Values.ToList(); }
        }


    }
}