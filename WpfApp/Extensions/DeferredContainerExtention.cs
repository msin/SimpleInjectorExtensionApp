using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;

namespace WpfApp.Extensions
{
    //
    //  Markup extension that resolces an instance from a IoC container.
    //  Based on Mike Hillberg's Blog on Wpf and Silverlight
    //  http://blogs.msdn.com/b/mikehillberg/archive/2006/10/06/limitedgenericssupportinxaml.aspx
    //
    //  And mostly on Ifeanyi Echeruo's blog WPF Recipe - Deferred Markup Extension 
    //  http://blogs.msdn.com/b/ifeanyie/archive/2010/03/27/9986217.aspx
    //

    public class DeferredContainerExtension : MarkupExtension
    {
        public Type Type { get; set; }

        private static bool TryGetFromProvideValueTarget(IProvideValueTarget target, out IProvideValueTargetSetter result)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            do
            {
                var dependencyObject = target.TargetObject as DependencyObject;
                var dependencyProperty = target.TargetProperty as DependencyProperty;

                if (dependencyObject != null && dependencyProperty != null)
                {
                    result = new DependencyPropertyProvideValueTargetSetter(dependencyObject, dependencyProperty);
                    break;
                }

                // We have no idea how to deal with the object and its property
                // this is probably template voodoo
                result = null;
            } while (false);

            return result != null;
        }

        // ProvideValue, which returns a defered object instance from the container
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");

            var provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            if (provideValueTarget == null)
                throw new NullReferenceException("provideValueTarget");

            IProvideValueTargetSetter setter;

            if (TryGetFromProvideValueTarget(provideValueTarget, out setter))
                Task.Factory.StartNew(() => SetDataContext(serviceProvider, setter));

            return null;
        }

        private void SetDataContext(IServiceProvider serviceProvider, IProvideValueTargetSetter setter)
        {
            //  Wiat until the container is verified - all instances are registered
            while (!App.IsVerified())
                Thread.Sleep(50);

            //  Set Velue of DependencyProperty of DependencyObject (mostly value of FrameworkElement.DataContext)
            setter.SetValue(App.GetInstance(Type));
            setter.Dispose();
        }
    }


    public interface IProvideValueTargetSetter : IDisposable
    {
        void SetValue(object value);
    }

    /// <summary>
    /// IProvideValueTargetSetter implementation for DependencyObject DependencyProperty pairs
    /// </summary>
    public sealed class DependencyPropertyProvideValueTargetSetter : IProvideValueTargetSetter
    {
        private DependencyObject targetObject;
        private DependencyProperty targetProperty;

        public DependencyPropertyProvideValueTargetSetter(DependencyObject targetObject, DependencyProperty targetProperty)
        {
            if (targetObject == null)
            {
                throw new ArgumentNullException("targetObject");
            }

            if (targetProperty == null)
            {
                throw new ArgumentNullException("targetProperty");
            }

            this.targetObject = targetObject;
            this.targetProperty = targetProperty;
        }

        public void SetValue(object value)
        {
            if (targetObject == null || targetProperty == null)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            // Marshal the call back to the UI thread
            targetObject.Dispatcher.Invoke(DispatcherPriority.Normal, (DispatcherOperationCallback)SetValueCallback, value);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            targetObject = null;
            targetProperty = null;
        }

        private object SetValueCallback(object arg)
        {
            targetObject.SetValue(targetProperty, arg);
            return null;
        }
    }
}
