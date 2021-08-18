using Microsoft.PowerPlatform.Dataverse.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RMC.DTO.CRM.Singleton
{
    public sealed class DataverseSingleton
    {
        private static DataverseSingleton instance;
        public static string ConnectionString { get; set; }
        public ServiceClient service { get; set; }
        private DataverseSingleton()
        {
            service = new ServiceClient(ConnectionString);
        }
        public static DataverseSingleton Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(DataverseSingleton))
                        if (instance == null) instance = new DataverseSingleton();

                return instance;
            }
        }

    }
}
