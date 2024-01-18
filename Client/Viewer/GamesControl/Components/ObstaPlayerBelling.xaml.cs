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

namespace Client.Viewer.GamesControl.Components
{
	/// <summary>
	/// Interaction logic for ObstaPlayerBelling.xaml
	/// </summary>
	public partial class ObstaPlayerBelling : UserControl
	{
		public int player;
		public ObstaPlayerBelling(int player, string name)
		{
			InitializeComponent();
			lblName.Content = name;
			this.player = player;
		}
	}
}
