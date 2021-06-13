using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
    /// Klasa <c>Archiwum</c> zapis i przywrócenie archiwum bazy danych.
    /// </summary>
    public partial class Archiwum : UserControl
    {
        /// <summary>
        /// Konstruktor klasy Archiwum.
        /// </summary>
        public Archiwum()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Wywołanie zapisu do archiwum.
        /// </summary>
        /// <param name="sender">Źródło</param>
        /// <param name="e">Dodatkowe argumenty</param>
        private void Archiwizuj(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "SQL|*.sql";
            dialog.ShowDialog();
            if (dialog.FileName != null)
            {
                using (MySqlCommand Cmd = new MySqlCommand())
                {
                    using (MySqlBackup Mb = new MySqlBackup(Cmd))
                    {
                        try
                        {
                            Cmd.Connection = BazaDanych.Connection;
                            Mb.ExportToFile(dialog.FileName);
                        }
                        catch (MySqlException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Wywołanie przywrócenia z archiwum.
        /// </summary>
        /// <param name="sender">Źródło</param>
        /// <param name="e">Dodatkowe argumenty</param>
        private void Przywroc(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "SQL|*.sql";
            dialog.ShowDialog();
            if (dialog.FileName != null)
            {
                using (MySqlCommand Cmd = new MySqlCommand())
                {
                    using (MySqlBackup Mb = new MySqlBackup(Cmd))
                    {
                        try
                        {
                            Cmd.Connection = BazaDanych.Connection;
                            Mb.ImportFromFile(dialog.FileName);
                        }
                        catch (MySqlException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }
    }
}
