using EventBuddy.DataModel;
using System;
using System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace EventBuddy
{
    public sealed partial class SessionEditor : UserControl
    {
        public SessionEditor()
        {
            this.InitializeComponent();
            this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            VisualStateManager.GoToState(this, "hidden", false);
        }

        public class CancelledEditorEventArgs : EventArgs
        {
        }

        public class SaveEditorEventArgs : EventArgs
        {
        }

        public delegate void CancelledEditor(object sender, CancelledEditorEventArgs args);
        public delegate void SaveEditor(object sender, SaveEditorEventArgs args);

        public event CancelledEditor Cancelled;
        public event SaveEditor Save;
        public Action<Session, string> UploadCallback;

        public void InvokeCancelled()
        {
            var cncl = Cancelled;
            if (cncl != null)
            {
                cncl.Invoke(this, new CancelledEditorEventArgs());
            }
        }

        public void InvokeSave()
        {
            var accpt = Save;
            if (accpt != null)
            {
                accpt.Invoke(this, new SaveEditorEventArgs());
            }
        }

        public void Show()
        {
            this.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.FileNameTextBlock.Text = string.Empty;
            
            var session = this.DataContext as Session;
            if(session != null)
                lblDialogTitle.Text = (session.Id > 0) ? "Edit Session" : "Add Session";                

            VisualStateManager.GoToState(this, "shown", false);

            txtName.Focus(Windows.UI.Xaml.FocusState.Programmatic);
        }

        public void Hide()
        {
            VisualStateManager.GoToState(this, "hidden", false);
            this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            InvokeCancelled();
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (txtName.IsValid() && startDate.IsValid() && endDate.IsValid())
            {
                this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                InvokeSave();    
            }
        }
    }
}
