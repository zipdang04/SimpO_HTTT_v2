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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Server.HostServer.Components
{
	/// <summary>
	/// Interaction logic for PlayerVCNVBelling.xaml
	/// </summary>
	public partial class PlayerVCNVBelling : UserControl
	{
		public int playerIndex;
		public PlayerVCNVBelling(int index, string name)
		{
			InitializeComponent();
			playerIndex = index;
			lblPlayer.Content = name;
			radioCorrect.GroupName = radioWrong.GroupName = name;
		}
	}
}
