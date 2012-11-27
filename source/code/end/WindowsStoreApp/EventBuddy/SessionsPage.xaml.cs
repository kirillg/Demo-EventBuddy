﻿using Microsoft.WindowsAzure.MobileServices;
using EventBuddy.DataModel;
using EventBuddy.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace EventBuddy
{
    public sealed partial class SessionsPage : EventBuddy.Common.LayoutAwarePage
    {
        private async void SaveSession(Session item)
        {
            await App.MobileService.GetTable<Session>().InsertAsync(item);

            Sessions.Add(item);
        }

        private async Task LoadSessions(Event eventItem)
        {
            Sessions = await App.MobileService.GetTable<Session>().Where(e => e.EventId == Event.Id).ToEnumerableAsync();
        }

        private async void UpdateSessionWithDeck(Session session, string deckSource)
        {
            session.DeckSource = deckSource;
            
            await App.MobileService.GetTable<Session>().UpdateAsync(session);            
        }

        #region 
        private ObservableCollection<Session> _sessions = new ObservableCollection<Session>();
        public dynamic Sessions
        {
            get { return _sessions; }
            set
            {
                _sessions.Clear();
                foreach (var item in value)
                {
                    _sessions.Add(item);
                }
            }
        }

        public Event Event { get; set; }

        public SessionsPage()
        {
            this.InitializeComponent();
            User.Current.PropertyChanged += this.OnUserPropertyChanged;
            this.SetLoggedUser();
            this.sessionEditor.UploadCallback = UpdateSessionWithDeck;
        }

        private void OnUserPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "UserId")
            {
                this.SetLoggedUser();
            }
        }

        private void SetLoggedUser()
        {
            this.loggedUser.Text = LoginMessageHelper.GetLoginMessage();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Event = e.Parameter as Event;

            await LoadSessions(Event);
            
            itemList.ItemsSource = _sessions;

            gridEvent.DataContext = Event;
        }

        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.editButtonCommands.Visibility = Visibility.Visible;
            this.BottomAppBar.IsOpen = true;

            sessionEditor.DataContext = (Session)e.ClickedItem;
        }

        private void btnAddSession(object sender, RoutedEventArgs e)
        {
            sessionEditor.DataContext = new Session(Event);
            sessionEditor.Show();
        }

        private void btnEditSession(object sender, RoutedEventArgs e)
        {
            sessionEditor.Show();
        }

        private async void btnSaveSession(object sender, EventBuddy.SessionEditor.SaveEditorEventArgs args)
        {
            var item = sessionEditor.DataContext as Session;

            if (item.Id == 0)
            {
                SaveSession(item);
            }
            else
            {
                await GetPrivateClient().GetTable<Session>().UpdateAsync(item);
                
                await LoadSessions(this.Event);
            }

            sessionEditor.Hide();
            this.editButtonCommands.Visibility = Visibility.Collapsed;
        }

        MobileServiceClient privateClient = null;

        private MobileServiceClient GetPrivateClient()
        {
            if (privateClient == null)
            {
                var field = typeof(App).GetRuntimeFields().SingleOrDefault(pi => pi.Name == "MobileService");
                if (field == null)
                {
                    return null;
                }
                privateClient = field.GetValue(null) as MobileServiceClient;
            }
            return privateClient;
        }
    #endregion  

    }
}
