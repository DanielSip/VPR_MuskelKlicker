﻿using System;
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
using System.Windows.Shapes;

namespace MuskelKlicker
{
    /// <summary>
    /// Interaktionslogik für Credits.xaml
    /// </summary>
    public partial class Credits : Window
    {
        public Credits()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Daniel Sippel
        /// Öffnet das Main menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_menu_Click(object sender, RoutedEventArgs e)
        {
            MainMenu menu = new MainMenu();
            Close();
            menu.Show();
        }
    }
}
