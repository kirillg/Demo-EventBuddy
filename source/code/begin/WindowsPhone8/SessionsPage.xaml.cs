using EventBuddy.WindowsPhone.Model;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace EventBuddy.WindowsPhone
{
    public partial class SessionsPage : PhoneApplicationPage, INotifyPropertyChanged 
    {

        public SessionsPage()
        {
            InitializeComponent();

            this.DataContext = this;
            this.Loaded += Sessions_Loaded;
        }

        void Sessions_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private async void LoadData()
        {
            string id = this.NavigationContext.QueryString["id"];
            EventName = this.NavigationContext.QueryString["name"];
            int eventId = int.Parse(id);
            var sessions = await App.MobileService.GetTable<Session>().Where(s => s.EventId == eventId).ToEnumerableAsync();
            EventSessions.Clear();
            foreach (var item in sessions)
            {
                EventSessions.Add(item);
            }
        }

        private readonly ObservableCollection<Session> _sessions = new ObservableCollection<Session>();

        public ObservableCollection<Session> EventSessions
        {
            get { return _sessions; }
        }

        private string _eventName;
        public string EventName
        {
            get { return _eventName; }
            set { SetProperty(ref _eventName, value); }
        }
   
        private void OnGridTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            
        }

        private void OnListboxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Session session = null;

            if (e.AddedItems.Count > 0)
                session = e.AddedItems[0] as Session;

            if (session != null)
            {
                PhoneApplicationService.Current.State["Session"] = session;
                string uri = string.Format("/SessionDetail.xaml?id={0}", session.Id);
                this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
            }

            //Session session = (Session)e.DataContext;
            //PhoneApplicationService.Current.State["Session"] = session;
            //string value = b.Content.ToString();
            //var rating = new Rating()
            //{
            //    ImageUrl = "https://twimg0-a.akamaihd.net/profile_images/1349731834/profile2_reasonably_small.png",
            //    Score = int.Parse(value),
            //    SessionId = s.Id,
            //    RaterName = "Nick Harris",
            //};
            //await App.MobileService.GetTable<Rating>().InsertAsync(rating);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}