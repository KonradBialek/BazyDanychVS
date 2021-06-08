using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Newtonsoft.Json;
using System.Windows;

namespace AplikacjaDostepowa
{
    /// <summary>
    /// Klasa odpowiedzialna za komunikację z bazą danych
    /// </summary>
    static class BazaDanych
    {
        public static MySqlConnection Connection { get; set; } = null;
        /// <summary>
        /// Wykonuje zapytanie sql bez zwracania danych (np. Insert, Update)
        /// </summary>
        public static void Execute(string query, params MySqlParameter[] parameters)
        {
            try
            {
                MySqlCommand command = new MySqlCommand(query, Connection);
                command.Parameters.AddRange(parameters);
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        internal static void SetPassword(string password)
        {
            byte[] pass = Encoding.UTF8.GetBytes(password);
            MD5 md5Provider = new MD5CryptoServiceProvider();
            byte[] md5Hash = md5Provider.ComputeHash(pass);
            password = BitConverter.ToString(md5Hash).Replace("-", string.Empty).ToLower();
            var ConnString = "server=localhost;uid=dziennikszkolny;pwd=" + password + ";database=dziennikszkolny;";
            Connection = new MySqlConnection(ConnString);
            try
            {
                Connection.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Wykonuje zapytanie typu Select zwracając dane
        /// </summary>
        /// <param name="query"></param>
        public static List<object[]> ReadAsArray(string query, params MySqlParameter[] parameters)
        {
            try
            {
                MySqlCommand command = new MySqlCommand(query, Connection);
                command.Parameters.AddRange(parameters);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    var list = new List<object[]>();
                    while (reader.Read())
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);
                        list.Add(values);
                    }
                    return list;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return new List<object[]>();
            }
        }
        public static List<Dictionary<string, object>> ReadAsDictionary(string query, params MySqlParameter[] parameters)
        {
            try
            {
                MySqlCommand command = new MySqlCommand(query, Connection);
                command.Parameters.AddRange(parameters);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    var list = new List<Dictionary<string, object>>();
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var name = reader.GetName(i);
                            var value = reader.GetValue(i);
                            row.Add(name, value);
                        }
                        list.Add(row);
                    }
                    return list;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return new List<Dictionary<string, object>>();
            }
        }
        public static DataTable GetTable(string query, params MySqlParameter[] parameters)
        {
            try
            {
                MySqlCommand command = new MySqlCommand(query, Connection);
                command.Parameters.AddRange(parameters);
                DataTable dataTable = new DataTable();
                (new MySqlDataAdapter(command)).Fill(dataTable);
                return dataTable;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return new DataTable();
            }
        }

        public static List<T> ReadAsClass<T>(string query, params MySqlParameter[] parameters)
        {
            return JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(ReadAsDictionary(query, parameters)));
        }
    }
}
