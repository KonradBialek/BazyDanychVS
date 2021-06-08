using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AplikacjaDostepowa.Views
{
    /// <summary>
    /// Logika interakcji dla klasy Logowanie.xaml
    /// </summary>
    public partial class Logowanie : UserControl
    {
        private string Login;
        private string Haslo;

        public string Login1 { get => Login; set => Login = value; }
        public string Haslo1 { get => Haslo; set => Haslo = value; }

        //String Login;
        //String Haslo;
        public Logowanie()
        {
            InitializeComponent();
        }


        public void Zaloguj(object sender, RoutedEventArgs e)
        {
            BazaDanych.SetPassword(hasloDB.Password);
            Login1 = login.Text;
            //Haslo1 = haslo.Text; 
            byte[] pass = Encoding.UTF8.GetBytes(haslo.Password);
            MD5 md5Provider = new MD5CryptoServiceProvider();
            byte[] md5Hash = md5Provider.ComputeHash(pass);
            string strPassword = BitConverter.ToString(md5Hash).Replace("-", string.Empty).ToLower();
            try
            {
                var qwe = ((MainWindow)Application.Current.MainWindow).UserInBase(Login1, strPassword);
                if (qwe != null)
                    ((MainWindow)Application.Current.MainWindow).Zaloguj(qwe);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
