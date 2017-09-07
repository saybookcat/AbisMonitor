using System.Threading;
using AbisMonitor.UI.ViewModels;
using Common;
using Framework;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows;
using System.ComponentModel;
using AbisMonitor.ViewModels;
using AbisMonitor.Views;
using GalaSoft.MvvmLight.Threading;

namespace AbisMonitor
{
    public partial class App : Application
    {

        private Mutex _mutex;
        private static MainWindow app;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            CheckDuplicateOperation();

            Log.Init();
         
            Log.Info("Application Startup");
            
            // For catching Global uncaught exception
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionOccured);

            Init();

            Log.Info("Starting App");
            LogMachineDetails();
            app = new MainWindow();
            var context = new MainViewModel();
            app.DataContext = context;
            app.Show();

        }

        static void UnhandledExceptionOccured(object sender, UnhandledExceptionEventArgs args)
        {

            Exception e = (Exception)args.ExceptionObject;
            var dialogService = new MvvmDialogs.DialogService();
            dialogService.ShowMessageBox((INotifyPropertyChanged)(app.DataContext),
                string.Format("系统发生未知错误，错误信息：{0}。",e),
                Pub.AppName,
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.OK);

   
            Log.Fatal("Application has crashed", e);
        }

        private void LogMachineDetails()
        {
            var computer = new Microsoft.VisualBasic.Devices.ComputerInfo();

            string text = "OS: " + computer.OSPlatform + " v" + computer.OSVersion + Environment.NewLine +
                          computer.OSFullName + Environment.NewLine +
                          "RAM: " + computer.TotalPhysicalMemory.ToString() + Environment.NewLine +
                          "Language: " + computer.InstalledUICulture.EnglishName;
            Log.Info(text);
        }

        public static bool IsLogin { get; set; }


        private void CheckDuplicateOperation()
        {
            bool ret;
            _mutex = new Mutex(true, "AbisMonitor", out ret);

            if (!ret)
            {
                MessageBox.Show(string.Format("{0}已经启动。", Pub.AppName), Pub.AppName,
                    MessageBoxButton.OK, MessageBoxImage.Information);

                Environment.Exit(0);
            }
        }

        private void Init()
        {
            DispatcherHelper.Initialize();
        }
    }
}
