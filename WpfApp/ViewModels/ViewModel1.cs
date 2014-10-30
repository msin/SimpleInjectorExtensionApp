namespace WpfApp.ViewModels
{
    public class ViewModel1
    {
        public virtual string Name { get; set; }

        public ViewModel1()
        {
            Name = "ViewModel1 is attached";
        }

        public void Hello()
        {
            Name = "Hello from ViewModel 1";
        }
    }
}
