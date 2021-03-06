using AresNews.Models;
using AresNews.ViewModels;
using AresNews.Views;
using CustardApi.Objects;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
[assembly: ExportFont("FontAwesome5Free-Regular-400.otf", Alias = "FaRegular")]
[assembly: ExportFont("FontAwesome5Brands-Regular-400.otf", Alias = "FaBrand")]
[assembly: ExportFont("FontAwesome5Free-Solid-900.otf", Alias = "FaSolid")]
[assembly: ExportFont("Ubuntu-Regular.ttf", Alias = "P-Regular")]
[assembly: ExportFont("Ubuntu-Bold.ttf", Alias = "P-Bold")]
[assembly: ExportFont("Ubuntu-Medium.otf", Alias = "P-SemiBold")]
namespace AresNews
{
    public partial class App : Application
    {
        public static Collection<Source> Sources { get; private set; }
        // Property SqlLite Connection
        public static SQLiteConnection SqLiteConn { get; set; }
        public static SQLiteConnection BackUpConn { get; set; }
        public static Service WService { get; set; }


        public enum PageType
        {
            about,
            article,
            bookmark,
            news,
            source


        }
        public App()
        {
#if __LOCAL__
            // Set webservice
            WService = new Service(host: "192.168.1.12",
                                    port: 3000,
                                   sslCertificate: false);
#else
        // Set webservice
            WService = new Service(host: "pinnate-beautiful-marigold.glitch.me",
                                   sslCertificate: true);
#endif


            Sources = new Collection<Source>();

            InitializeComponent();

            

            // Start the db
            StartDb();

            SqLiteConn.CreateTable<Source>();
            SqLiteConn.CreateTable<Article>();
            BackUpConn.CreateTable<Source>();
            BackUpConn.CreateTable<Article>();

            // Close the db
            //CloseDb();

            MainPage = new AppShell();

            
        }
        /// <summary>
        ///  Function to close the database 
        /// </summary>
        public static void CloseDb()
        {
            SqLiteConn.Dispose();
            //SqLiteConn.Close();
        }
        /// <summary>
        /// Function to start the data base
        /// </summary>
        public static void StartDb()
        {
            // Just use whatever directory SpecialFolder.Personal returns
            string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            var path = Path.Combine(libraryPath, "ares.db3");
            var pathBackUp = Path.Combine(libraryPath, "aresBackup.db3");

            // Verify if a data base already exist
            if (!File.Exists(path))
                // Create the folder path.
                File.Create(path);

            // Verify if a data base already exist
            if (!File.Exists(pathBackUp))
                // Create the folder path.
                File.Create(pathBackUp);
            
            // Sqlite connection
            SqLiteConn = new SQLiteConnection(path);
            BackUpConn = new SQLiteConnection(pathBackUp);

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            base.OnSleep();

            AppShell mainPage = ((AppShell)MainPage);
            Page currentPage = mainPage.CurrentPage;

            if (currentPage.ToString() == "AresNews.Views.ArticlePage")
            {

                ((ArticleViewModel)((ArticlePage)currentPage).BindingContext).TimeSpent.Stop();
            }
            //SqLiteConn.Dispose();
        }
        

        protected override void OnResume()
        {
            AppShell mainPage = ((AppShell)MainPage);
            Page currentPage = mainPage.CurrentPage;

            if (currentPage.ToString() == "AresNews.Views.ArticlePage")
            {

                ((ArticleViewModel)((ArticlePage)currentPage).BindingContext).TimeSpent.Start();
            }
            else if (currentPage.ToString() == "AresNews.Views.NewPage")
            {
                ((NewsViewModel)((ArticlePage)currentPage).BindingContext).FetchArticles();
            }

            StartDb();
        }
    }
}
