using AresNews.Models;
using AresNews.ViewModels;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms.Xaml;

namespace AresNews.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsPage : ContentPage
    {
        private NewsViewModel _vm;
        public NewsPage()
        {
            InitializeComponent();

            BindingContext = _vm = new NewsViewModel ();

            _vm.IsRefreshing = true;

            // For the auto scroll
            MessagingCenter.Subscribe<MessageItem>(this._vm, "ScrollTop", (sender) =>
            {
                // Scroll to the top of the collection view
                newsCollectionView.ScrollTo(0);
            });

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Is the app connected to the internet
            //if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            //{
            //    this.DisplayToastAsync("You're offline", 60000);
            //}
        }
        public void DisplayOfflineMessage ()
        {
            this.DisplayToastAsync("You're offline", 60000);
        }
    }
}