using System;

using System.Windows;

using System.Timers;
using System.Net;
using System.Threading;
using System.IO;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static int am_i_connected=1;
        private static int re_connected = 0;
        private int is_thread_is_running = 0;
        public static int is_proxy_enabled=0;
        public static string proxy_url;
        public static int port;
        public static string username;
        public static string password;
        public static string config_file_path= @"C:\Users\ansh\Documents\config.txt";
        public static  int is_file_exist=0;
        public static string status;
   Thread thread;

        public MainWindow()
        {
            InitializeComponent();
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(is_net_connected);
            timer.Interval = 15000;
            string msg;
            if (File.Exists(config_file_path))
            {
                is_proxy_enabled = 1;
                is_file_exist = 1;
                var lines = File.ReadLines(config_file_path);
                string[] temp = new string[5];
                int i = 0;
                foreach (var line in lines)
                {
                    temp[i++] = line;
                }

           status = temp[0];
                proxy_url = temp[1];
                port = Convert.ToInt32(temp[2]);
                username = temp[3];
                password = temp[4];
            }
            timer.Enabled = true;

            if (is_proxy_enabled == 0)
            listBox.Items.Add("monitoring started , configure the proxy detail\n if you are behind proxy server");
            else

                listBox.Items.Add("monitoring started ,proxy is configured");



        }


        public void is_net_connected(object source, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(CheckForInternetConnection, System.Windows.Threading.DispatcherPriority.ContextIdle);
        }

        public void make_alarm()
        {
            is_thread_is_running = 1;
            int i = 0;
            for(i=0;i<500;i++)
            {
                Console.Beep();
            }
        }

        public void CheckForInternetConnection()
        {
           
            

            

                try
                {
                    using (var client = new WebClient())
                    {
                    if (is_proxy_enabled == 1)
                    {

                        WebProxy proxy = new WebProxy(proxy_url+":"+port, true);
                        proxy.Credentials = new NetworkCredential(username, password);  //These can be replaced by user input, if wanted.
                                                                                                   // WebRequest.DefaultWebProxy = proxy;
                        client.Proxy = proxy;
                    }
                        using (var stream = client.OpenRead("http://www.google.com"))
                        {
                        //listBox.Items.Add("connected at" + " " + DateTime.Now.ToString());
                        if (am_i_connected == 0 && re_connected == 0)
                        {
                            if (is_thread_is_running == 1)
                            {
                                is_thread_is_running = 0;
                                thread.Abort();
                            }
                            listBox.Items.Add("connected again at" + " " + DateTime.Now.ToString());

                            am_i_connected = 1;
                            re_connected = 1;
                        }
                        }
                    }
                }
                catch
                {
                if (am_i_connected == 1)
                {
                    if (is_thread_is_running == 0)
                    {
                        thread = new Thread(make_alarm);
                        thread.Start();
                    }
                    listBox.Items.Add("disconnected at" + " " + DateTime.Now.ToString()+"\n"+"waiting for connection.....");
                    am_i_connected = 0;
                    re_connected = 0;
                }

                }



            



            }

        private void proxy_clicked(object sender, RoutedEventArgs e)
        {
            var w = new Window
            {
                Content = new UserControl1(),
                Title = "proxy",
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.CanResize

            };
            w.Show();
        }

        private void stopalarm_Click(object sender, RoutedEventArgs e)
        {
            if (is_thread_is_running == 1)
            {
                is_thread_is_running = 0;
                thread.Abort();
            }
            else
            {
                MessageBox.Show("alarm is not started yet\n");
            }
        }
    }


}
