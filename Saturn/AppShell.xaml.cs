﻿namespace Saturn
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
            Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
            Routing.RegisterRoute(nameof(DetailsPageFromDeeplink), typeof(DetailsPageFromDeeplink));
        }
    }
}
