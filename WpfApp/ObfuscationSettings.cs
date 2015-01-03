using System.Reflection;

[assembly: Obfuscation(Feature = "encrypt symbol names with password 'TestPassword'", Exclude = false)]
[assembly: Obfuscation(Feature = "string encryption", Exclude = false)]
[assembly: Obfuscation(Feature = "Apply to type WpfApp.*: renaming", Exclude = true, ApplyToMembers = true)]
[assembly: Obfuscation(Feature = "encrypt resources [compress]", Exclude = false)]
[assembly: Obfuscation(Feature = "embed SimpleInjector.dll", Exclude = false)]
[assembly: Obfuscation(Feature = "embed DevExpress.Mvvm.v14.2.dll", Exclude = false)]

