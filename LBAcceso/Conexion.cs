using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBAcceso
{
     public class Conexion
    {
        //static String cadenaConexion = @"Data Source=0048-08\SQL2014;Initial Catalog=Siprofi; Integrated Security=SSPI;";
        static String cadenaConexion = @"Data Source = 0136-08; Initial Catalog=sil; Integrated Security = SSPI;";
        
        //static String cadenaConexion = System.Configuration.ConfigurationManager.AppSettings["conn"];
        //string userName = System.Configuration.ConfigurationManager.AppSettings["conn"];

        public static string CadenaConexion
        {
            //get { return ReadSetting("llave1"); }
            get { return cadenaConexion; }
        }

        static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
                return result;
            }
            catch (Exception e)
            {
                return "Error reading app settings";
            }
        }


    }
}