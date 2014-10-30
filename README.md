SimpleInjectorExtensionApp
==========================

Demo applicarion with integrated the IoC Simple Injector in XAML code

Final ViewModel is emitted by free DevExpress MVVM Framework, this greatly simplfies the ViewModel code:
- POCO property instead of property with INPC implementation
- simple method instead of DelegateCommand / RelayCommand

Complete documentation is here:
https://documentation.devexpress.com/#WPF/CustomDocument15112

This is the best MVVM Framework I've ever seen.

But there is a limitation - a ViewModel emits with the default constructor, so there is no possibility for constructor injection (auto-wire)
https://www.devexpress.com/Support/Center/Question/Details/T152613

So you have to resolve dependencies explicitely:
IMainVM _main = App.container.GetInstance<IMainVM>();
