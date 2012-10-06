using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Threading;
using System.ComponentModel;
using System.Windows.Data;

namespace SynchronousWebServiceCalling
{
    public partial class MainPage : UserControl
    {
        private  AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        public MainPage()
        {
            InitializeComponent();
            StartServerTimeUpdatingSyncronosly();
        }
        private void StartServerTimeUpdatingSyncronosly()
        {
            try
            {
                Thread thread = new Thread(new ThreadStart(UpdateTimeSyncronosly));
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SynchronousWebServiceCalling", MessageBoxButton.OK);
            }
        }     
        private void UpdateTimeSyncronosly()
        {
            SynchronousWebServiceCalling.ContentServiceReference.ContentServiceClient contentServiceClient = new SynchronousWebServiceCalling.ContentServiceReference.ContentServiceClient();
            contentServiceClient.ReturnServerTimeCompleted += new EventHandler<SynchronousWebServiceCalling.ContentServiceReference.ReturnServerTimeCompletedEventArgs>(ReturnServerTimeCompleted);
            string message = "Retrieving new server time...";
            while (true)
            {
                this.Dispatcher.BeginInvoke(new Action<string>(DisplayBusyMessage), message);
                contentServiceClient.ReturnServerTimeAsync();
                autoResetEvent.WaitOne();
            }
        }
        void ReturnServerTimeCompleted(object sender, SynchronousWebServiceCalling.ContentServiceReference.ReturnServerTimeCompletedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action<string>(DisplayTime), e.Result);         
            string message = "New server time is retrieved";
            this.Dispatcher.BeginInvoke(new Action<string>(DisplayBusyMessage), message);
            System.Threading.Thread.Sleep(5000);
            autoResetEvent.Set();
        }
        private void DisplayTime(string time)
        {
            tblTime.Text = time;
        }
        private void DisplayBusyMessage(string message)
        {
            StatusLabel.Text = message;
        }
    }
}
