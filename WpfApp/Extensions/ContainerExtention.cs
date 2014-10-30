using System;
using System.Windows.Markup;

namespace WpfApp.Extensions
{
    //
    //  Markup extension that resolces an instance from a IoC container.
    //  Based on Mike Hillberg's Blog on Wpf and Silverlight
    //  http://blogs.msdn.com/b/mikehillberg/archive/2006/10/06/limitedgenericssupportinxaml.aspx
    //

    public class ContainerExtension : MarkupExtension
    {
        public Type Type { get; set; }

        // ProvideValue, which returns an object instance from the container
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");

            if (Type == null)
                throw new NullReferenceException("Type");

            return App.GetInstance(Type);
        }
    }
}
