using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Notifications;
using Windows.UI.Notifications.Management;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
namespace UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static ObservableCollection<string> items = new ObservableCollection<string>();
        public static UserNotificationListener listener = UserNotificationListener.Current;
        

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            Debug.Write("button pressed");
            MediaElement mediaElement = new MediaElement();

            // Get the listener

            UserNotificationListenerAccessStatus accessStatus =
                await listener.RequestAccessAsync();
            if (accessStatus != UserNotificationListenerAccessStatus.Allowed)
            {
                return;
            }

            // Get the toast notifications
            NotificationsProcessor.GetNotificationsAndProcessThem();
            //Request / check background task access via BackgroundExecutionManager.RequestAccessAsync
            var requestStatus =
                await Windows.ApplicationModel.Background.BackgroundExecutionManager.RequestAccessAsync();
            if (requestStatus != BackgroundAccessStatus.AlwaysAllowed)
            {

            }


            ApplicationData.Current.LocalSettings.Values["python"] = this.PythonPath.Text;
            ApplicationData.Current.LocalSettings.Values["command"] = this.NotificationPath.Text;
            // If background task isn't registered yet
            if (!BackgroundTaskRegistration.AllTasks.
                   Any(i => i.Value.Name.Equals("UserNotificationChanged")))
            {
                // Specify the background task
                var builder = new BackgroundTaskBuilder()
                {
                    Name = "UserNotificationChanged"
                };

                // Set the trigger for Listener, listening to Toast Notifications
                builder.SetTrigger(new UserNotificationChangedTrigger(NotificationKinds.Toast));

                // Register the task
                builder.Register();
            }
            button.IsEnabled = false;
            Python.IsEnabled = false;
            NotificationPythonFile.IsEnabled = false;
        }

        private async void Python_Click(object sender, RoutedEventArgs e)
        {
            var folderPicker = new Windows.Storage.Pickers.FileOpenPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            Windows.Storage.StorageFile file = await folderPicker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to all contents in the picked folder
                // (including other sub-folder contents)
                this.PythonPath.Text = file.Path;
            }
            else
            {
                this.PythonPath.Text = "Operation cancelled.";
            }
        }

        private async void NotificationPythonFile_Click(object sender, RoutedEventArgs e)
        {
            var folderPicker = new Windows.Storage.Pickers.FileOpenPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            Windows.Storage.StorageFile file = await folderPicker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to all contents in the picked folder
                // (including other sub-folder contents)
                this.NotificationPath.Text = file.Path;
            }
            else
            {
                this.NotificationPath.Text = "Operation cancelled.";
            }
        }
    }

    public class NotificationsProcessor
    {
        internal static async Task SyncNotifications()
        {
            NotificationsProcessor.GetNotificationsAndProcessThem();
        }

        public static async void GetNotificationsAndProcessThem()
        {
            IReadOnlyList<UserNotification> notifs =
                    await MainPage.listener.GetNotificationsAsync(NotificationKinds.Toast);

            UserNotification n = notifs[notifs.Count - 1];
            
            // Get the toast binding, if present
            NotificationBinding toastBinding = n.Notification.Visual.GetBinding(KnownNotificationBindings.ToastGeneric);

            if (toastBinding != null)
            {
                // And then get the text elements from the toast binding
                IReadOnlyList<AdaptiveNotificationText> textElements = toastBinding.GetTextElements();
                String appName = n.AppInfo.DisplayInfo.DisplayName;
                string bodyText =
                    string.Join(" | ", textElements.Select(t => t.Text));
                Debug.Write(bodyText);
                MainPage.items.Add(bodyText);


                if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
                {
                    ApplicationData.Current.LocalSettings.Values["parameters"] = appName + " | " + bodyText;
                    await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
                }
            }
            

        }

    }
}
