using System;
using System.Text;
using System.Windows;

namespace SoftwareKobo.WPFDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnClick(object sender, RoutedEventArgs e)
        {
            VisualTreeLooper looper = new VisualTreeLooper(this);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("层级|控件名");
            looper.Loop((manager, obj) => sb.AppendFormat("{0}层级{1}|{2}{3}", new string(' ', manager.Deep * 2), manager.Deep, obj, Environment.NewLine), null);
            MessageBox.Show(sb.ToString());
        }
    }
}