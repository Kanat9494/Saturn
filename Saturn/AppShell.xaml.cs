namespace Saturn
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutingPages();
        }

        private void RegisterRoutingPages()
        {
            Routing.RegisterRoute(nameof(ChatPage), typeof(ChatPage));
            Routing.RegisterRoute("CustomBottomBarPage", typeof(CustomBottomBarPage));
        }
    }
}
