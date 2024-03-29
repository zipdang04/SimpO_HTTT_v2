﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using SimpleSockets.Client;
using SimpleSockets;
using System.Net;

namespace Client.Pointer
{
	/// <summary>
	/// Interaction logic for LogInWindow.xaml
	/// </summary>
	public partial class LogInWindow : Window
	{
		MainWindow main;
		SimpleSocketClient client = new SimpleSocketTcpClient();
		PointerWindow? pointerWindow;
        public LogInWindow(MainWindow main)
		{
			InitializeComponent();
			this.main = main;

			client.ConnectedToServer += ConnectedToServer;
		}
		private void ConnectedToServer(SimpleSocket a)
		{
			client.ConnectedToServer -= ConnectedToServer;
			MessageBox.Show("Đã kết nối", "đã nối", MessageBoxButton.OK);
			Dispatcher.Invoke(() => {
				pointerWindow = new PointerWindow(this, client);
				pointerWindow.Show(); Hide();
			});
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			string serverIP;
			int serverPort;
			
			try
			{
				serverIP = txtIP.Text;
				serverPort = Convert.ToInt32(txtPort.Text);

				IPAddress testIP = IPAddress.Parse(serverIP);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Đầu vào không hợp lệ!");
				return;
			}

			try
			{
				client.StartClient(serverIP, serverPort);
			}
			catch
			{
				MessageBox.Show("Có lỗi trong khi kết nối");
				client.Close();
				return;
			}
		}

		private void Window_Unloaded(object sender, RoutedEventArgs e)
		{
			main.Show();
		}
	}
}
