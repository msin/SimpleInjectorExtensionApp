namespace WpfApp.ViewModels
{
    public class ViewModel2
    {
        public virtual string Name { get; set; }

        public ViewModel2()
        {
            Name = "ViewModel2 is attached";
        }

        public void Hello()
        {
            Name = "Hello from ViewModel 2";
        }
    }
}
