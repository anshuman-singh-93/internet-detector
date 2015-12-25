using System;

using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        
        public UserControl1()
        {
            InitializeComponent();
            if (File.Exists(MainWindow.config_file_path))
            {
                
                var lines = File.ReadLines(MainWindow.config_file_path);
                string[] temp = new string[5];
                int i = 0;
                foreach (var line in lines)
                {
                    temp[i++] = line;
                }

                string status = temp[0];
                textBox1.Text = temp[1];
                textBox2.Text = temp[2];
                textBox3.Text = temp[3];
                textBox4.Text = temp[4];
                if (status == "enabled")
                    checkBox.IsChecked = true;
            }
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (File.Exists(MainWindow.config_file_path))
            {
                File.Delete(MainWindow.config_file_path);
                string path = MainWindow.config_file_path;
                File.AppendAllText(path, "disabled" + Environment.NewLine);

                File.AppendAllText(path, textBox1.Text + Environment.NewLine);
                File.AppendAllText(path, textBox2.Text + Environment.NewLine);
                File.AppendAllText(path, textBox3.Text + Environment.NewLine);
                File.AppendAllText(path, textBox4.Text + Environment.NewLine);

            }
           
            MainWindow.is_proxy_enabled = 0;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();

        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            string url,u,p;
            
            url = textBox1.Text;
       
            try
            {
                int.TryParse(textBox2.Text,out MainWindow.port);
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
            u = textBox3.Text;
            p = textBox4.Text;

            if(url.Length==0 || u.Length==0 || p.Length==0)
            {
                MessageBox.Show("some field are empty");

            }
            else
            {
               MainWindow.proxy_url = url;
                MainWindow.username = u;
                string path = MainWindow.config_file_path;
               MainWindow.password = p;
                if (File.Exists(MainWindow.config_file_path))
                {
                    File.Delete(MainWindow.config_file_path);

                }
                File.AppendAllText(path, "enabled" + Environment.NewLine);

                File.AppendAllText(path,url + Environment.NewLine);
                File.AppendAllText(path, textBox2.Text + Environment.NewLine);
                File.AppendAllText(path, u + Environment.NewLine);
                File.AppendAllText(path, p + Environment.NewLine);
                MainWindow.is_proxy_enabled = 1;

                Window.GetWindow(this).Close();

            }
        }
    }
}
