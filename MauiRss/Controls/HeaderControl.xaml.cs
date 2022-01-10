namespace MauiRss.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeaderControl : ContentView
    {
        public HeaderControl()
        {
            InitializeComponent();
        }

        public event EventHandler? OnNewItemClickedEvent;

        private void OnNewItemClicked(object sender, EventArgs e)
        {
            this.OnNewItemClickedEvent?.Invoke(this, e);
        }
    }
}
