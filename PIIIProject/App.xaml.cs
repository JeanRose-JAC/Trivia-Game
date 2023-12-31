﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PIIIProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Exception not handles: " + e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //what action to
            e.Handled = true;// to  stop app from crashing set true
        }
    }
}
