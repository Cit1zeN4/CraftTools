using CraftTools.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Npgsql;

namespace CraftTools.Helpers
{
    public class Tools
    {
        public static byte[] ImageToByteArrayFromFilePath(string imagefilePath)
        {
            byte[] imageArray = File.ReadAllBytes(imagefilePath);
            return imageArray;
        }

        public static WareMaterial ConvertMaterialToWareMaterial(Material material)
        {
            WareMaterial wareMaterial = new WareMaterial
            {
                Name = material.Name,
                Description = material.Description,
                HaveSize = material.HaveSize,
                Length = material.Length,
                Width = material.Width,
                Price = material.Price
            };
            return wareMaterial;
        }

        public static void WareMaterialApplyChanges(ref WareMaterial wareMaterial, ChangerModel model)
        {
            wareMaterial.Length = model.WareMaterial.Length;
            wareMaterial.Width = model.WareMaterial.Width;
            wareMaterial.CustomPrice = model.WareMaterial.CustomPrice;
        }

        public static string GetConnectionString()
        {
            string connectionString;
            var DataSource = Properties.Settings.Default.DBServerName;
            var Port = Properties.Settings.Default.DBProt;
            var InitialCatalog = Properties.Settings.Default.DBDatabaseName;
            var IntegratedSecurity = Properties.Settings.Default.DBUseIntegratedSecurity;
            var UserID = Properties.Settings.Default.DBUserName;
            var Password = Properties.Settings.Default.DBPassword;

            connectionString = $"Server={DataSource};Port={Port};Database={InitialCatalog};";

            if (IntegratedSecurity)
            {
                connectionString += $"User Id={UserID};Password={Password}";
            }
            else
            {
                connectionString += $"Integrated Security={IntegratedSecurity};";
            }
            return connectionString;
        }

        public static void SetConnectionString(string serverName, string port, string dbName, bool integratedSecurity = true, string userId = "", string password = "" )
        {
            Properties.Settings.Default.DBServerName = serverName;
            Properties.Settings.Default.DBProt = port;
            Properties.Settings.Default.DBDatabaseName = dbName;
            Properties.Settings.Default.DBUseIntegratedSecurity = integratedSecurity;
            Properties.Settings.Default.DBUserName = userId;
            Properties.Settings.Default.DBPassword = password;
        }
    }
}
