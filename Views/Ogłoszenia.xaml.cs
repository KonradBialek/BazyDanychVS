using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    /// Logika interakcji dla klasy Ogłoszenia.xaml
    /// </summary>
    public partial class Ogłoszenia : UserControl
    {
        public Ogłoszenia()
        {
            DataContext = this;
            InitializeComponent();
            var user = ((MainWindow)Application.Current.MainWindow).LoggedUser;
            DodajButton.Visibility = user.Typ == UserType.Nauczyciel ? Visibility.Visible : Visibility.Collapsed;
        }
        public DataTable Data => BazaDanych.GetTable("SELECT tresc from informacje");

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).DataContext = new DodawanieOgłoszenia();
        }
    }
}
