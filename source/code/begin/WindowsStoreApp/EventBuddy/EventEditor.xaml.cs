using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace EventBuddy
{
    public sealed partial class EventEditor : UserControl
    {
        public EventEditor()
        {
            this.InitializeComponent();
            //this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
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
  
        public bool Shown
        {
            get { return (bool)GetValue(ShownProperty); }
            set { SetValue(ShownProperty, (bool)value); }
        }
       
        public static readonly DependencyProperty ShownProperty =
            DependencyProperty.Register("Shown", typeof(bool), typeof(EventEditor), new PropertyMetadata(false, ShownChanged));

        private static void ShownChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = d as EventEditor;
            if (editor != null)
            {
                if ((bool)e.NewValue)
                    editor.Show();
                else
                    editor.Hide();
            }
        }

        public event CancelledEditor Cancelled;
        public event SaveEditor Save;

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
            //this.Visibility = Windows.UI.Xaml.Visibility.Visible;
            VisualStateManager.GoToState(this, "shown", true);
            txtTitle.Focus(Windows.UI.Xaml.FocusState.Programmatic);
        }

        public void Hide()
        {
            VisualStateManager.GoToState(this, "hidden", true);
            //this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Shown = false; //note shown property does not seem to be working with the dep prop
            //this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            InvokeCancelled();
            Hide();
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (txtTitle.IsValid() && endDate.IsValid() && startDate.IsValid())
            {
                Hide();
                Shown = false; // note shown property does not seem to be working with the dep prop
                
                // this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                InvokeSave();
            }
        }
    }
}
