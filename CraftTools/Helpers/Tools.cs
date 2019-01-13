using CraftTools.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
            SqlConnectionStringBuilder sql = new SqlConnectionStringBuilder();
            sql.DataSource = Properties.Settings.Default.DBServerName;
            sql.InitialCatalog = Properties.Settings.Default.DBDatabaseName;
            sql.IntegratedSecurity = Properties.Settings.Default.DBUseIntegratedSecurity;
            if (!sql.IntegratedSecurity)
            {
                sql.UserID = Properties.Settings.Default.DBUserName;
                sql.Password = Properties.Settings.Default.DBPassword;
            }
            return sql.ConnectionString;
        }

        public static void SetConnectionString(string serverName, string dbName, bool integratedSecurity = true, string userId = "", string password = "" )
        {
            Properties.Settings.Default.DBServerName = serverName;
            Properties.Settings.Default.DBDatabaseName = dbName;
            Properties.Settings.Default.DBUseIntegratedSecurity = integratedSecurity;
            Properties.Settings.Default.DBUserName = userId;
            Properties.Settings.Default.DBPassword = password;
        }
    }
}
