using System;
using System.Diagnostics;
using System.Windows;
using DevExpress.Mvvm.POCO;
using SimpleInjector;
using WpfApp.ViewModels;
using WpfApp.Views;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        #region | Fields |

        private static Container _container;
        private static bool _isVerified;

        #endregion

        #region | Methods |

        [DebuggerStepThrough]
        public static TService GetInstance<TService>() where TService : class
        {
            return _container.GetInstance<TService>();
        }

        [DebuggerStepThrough]
        public static object GetInstance(Type type)
        {
            return _container.GetInstance(type);
        }

        [DebuggerStepThrough]
        public static bool IsVerified()
        {
            return _isVerified; 
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Bootstrap();
        }

        private static void Bootstrap()
        {
            // Create the container as usual.
            var container = new Container();

            // Register ViewModels using POCO ViewModel style from DevExpress
            //  https://documentation.devexpress.com/#WPF/CustomDocument17352
            //
            container.Register(ViewModelSource.Create<ViewModel1>);
            container.Register(ViewModelSource.Create<ViewModel2>);

            //  Register 2nd window
            container.Register<Window, NextWindow>();

            // Optionally verify the container.
            container.Verify();
            _isVerified = true;

            // Store the container for use by the application.
            _container = container;

            var win = new MainWindow();

            var next = container.GetInstance<Window>();

            win.Closed += (s, a) => next.Show();

            next.Closed += (s, a) => Current.Shutdown();

            win.Show();
        }

        #endregion
    }
}
