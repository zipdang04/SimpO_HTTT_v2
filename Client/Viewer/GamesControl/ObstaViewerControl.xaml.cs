﻿using Server.Information;
using System;
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

using Client.Viewer.GamesControl.Components;

namespace Client.Viewer.GamesControl
{
	/// <summary>
	/// Interaction logic for ObstaViewerControl.xaml
	/// </summary>
	public partial class ObstaViewerControl : UserControl
	{
		List<Image> images;
		WordControl[] wordControls = new WordControl[4];
		int keyLength = 0;
		int curIndex;
		MediaPlayer mediaStartGame = new MediaPlayer(),
					mediaOpenQuestion = new MediaPlayer(),
					mediaOpening = new MediaPlayer(),
					mediaOpenPiece = new MediaPlayer(),
					mediaBell = new MediaPlayer(),
					mediaBellCorrect = new MediaPlayer(),
					mediaBellWrong = new MediaPlayer(),
					media = new MediaPlayer(),
					mediaLast15 = new MediaPlayer();
		bool foundWinner = false;
		public ObstaViewerControl()
		{
			InitializeComponent();
			images = new List<Image>();
			image.BeginInit();
			images.Add(rectUL); images.Add(rectUR); images.Add(rectDL); images.Add(rectDR); images.Add(rectTT);
			for (int i = 0; i < 4; i++) {
				wordControls[i] = new WordControl();
				grid4W.Children.Add(wordControls[i]);
				Grid.SetColumn(wordControls[i], 1);
				Grid.SetRow(wordControls[i], i * 2 + 1);
			}
			lblKey.Visibility = Visibility.Hidden;
			mediaStartGame.Open(new Uri(HelperClass.PathString("Effects", "VCNV_StartGame.mp3")));
			mediaStartGame.Play(); mediaStartGame.Stop();
			mediaOpenQuestion.Open(new Uri(HelperClass.PathString("Effects", "VCNV_OpenQuestion.m4a")));
			mediaOpenQuestion.Play(); mediaOpenQuestion.Stop();
			mediaOpening.Open(new Uri(HelperClass.PathString("Effects", "VCNV_Opening.mpeg")));
			mediaOpening.Play(); mediaOpening.Stop();
			mediaOpenPiece.Open(new Uri(HelperClass.PathString("Effects", "VCNV_OpenPiece.mpeg")));
			mediaOpenPiece.Play(); mediaOpenPiece.Stop();
			mediaBell.Open(new Uri(HelperClass.PathString("Effects", "VCNV_Bell.m4a")));
			mediaBell.Play(); mediaBell.Stop();
			mediaBellCorrect.Open(new Uri(HelperClass.PathString("Effects", "VCNV_BellCorrect.m4a")));
			mediaBellCorrect.Play(); mediaBellCorrect.Stop();
			mediaBellWrong.Open(new Uri(HelperClass.PathString("Effects", "PlayerAnswerWrong.mpeg")));
			mediaBellWrong.Play(); mediaBellWrong.Stop();
			mediaShow.Source = new Uri(HelperClass.PathString("Effects", "VCNV_ShowQuestion.mp4")); mediaShow.BeginInit();
			mediaShow.Play(); mediaShow.Stop();
			media15s.Source = new Uri(HelperClass.PathString("Effects", "VCNV_15s.mp4")); media15s.BeginInit();
			media15s.Play(); media15s.Stop();
			mediaLast15.Open(new Uri(HelperClass.PathString("Effects", "VCNV_Last15s.mpeg")));
			mediaLast15.Play(); mediaLast15.Stop();

			mediaOpenQuestion.MediaEnded += mediaOpenQuestion_MediaEnded;
		}

        public void WTF()
        {
            _ = Dispatcher.Invoke(async () =>
            {
                while (image.RenderSize.Width == 0 || image.RenderSize.Height == 0)
                {
                    await Task.Delay(1000);
                    int i = 0; i += 1;
                }
                double width = image.RenderSize.Width;
                double height = image.RenderSize.Height;
                for (int i = 0; i < 5; i++)
                {
                    images[i].Visibility = Visibility.Visible;
                    images[i].Width = width;
                    images[i].Height = height;
                }

            });
        }
        public void GetKey(int length) { keyLength = length; }
		public void ResetGame(string attach, int[] cntLetter)
		{
			foundWinner = false;
			Dispatcher.Invoke(() => {
				SceneReset();
				image.Source = new BitmapImage(new Uri(HelperClass.PathString("Media", attach)));
				
				media15s.Visibility = Visibility.Hidden;
				mediaShow.Visibility = Visibility.Hidden;
				gridWord.Visibility = Visibility.Visible;
				gridPic.Visibility = Visibility.Hidden;
				qBox.Visibility = Visibility.Hidden;
				mediaStartGame.Position = TimeSpan.Zero; mediaStartGame.Play();

				for (int i = 0; i < 4; i++)
					wordControls[i].SetWord(cntLetter[i]);
				lblKey.Content = string.Format("CNV có {0} chữ cái", keyLength);
				lblKey.Visibility = Visibility.Visible;
			});
			WTF();
			WTF();
		}

		public void Opening() { Dispatcher.Invoke(() => { mediaOpening.Position = TimeSpan.Zero; mediaOpening.Play(); }); }
		public void ChangeScene(string s)
		{
			Dispatcher.Invoke(() => { 
				gridPic.Visibility = Visibility.Hidden;
				gridWord.Visibility = Visibility.Hidden;
				if (s == "PIC") gridPic.Visibility = Visibility.Visible;
				else if (s == "WORD") gridWord.Visibility = Visibility.Visible;
			});
		}

		public void ShowQuestion(int index, string question, string attach)
		{
			bool loadOke = false;
			Dispatcher.Invoke(() => {
				mediaOpenQuestion.Position = TimeSpan.Zero;
				mediaOpenQuestion.Play();
				qBox.SetQuestion(question);
				qBox.Visibility = Visibility.Hidden;
				media15s.Visibility = Visibility.Hidden; 
				mediaShow.Visibility = Visibility.Hidden; mediaShow.Position = TimeSpan.Zero;
				mediaShow.Stop();
				if (index < 4) wordControls[index].SetChoosing();
				this.curIndex = index;
				try {
					media.Open(new Uri(HelperClass.PathString("Media", attach)));
					media.Position = TimeSpan.Zero;
					loadOke = true;
				} catch { }
			});
			Dispatcher.Invoke(() => { if (loadOke) media.Play(); });
		}
		private void mediaOpenQuestion_MediaEnded(object? sender, EventArgs e)
		{
			mediaShow.Visibility = Visibility.Visible;
			mediaShow.Position = TimeSpan.Zero;
			mediaShow.Play();
		}
		private void mediaShow_MediaEnded(object sender, RoutedEventArgs e)
		{
			qBox.Visibility = Visibility.Visible;
		}

		public void StartTimer()
		{
			Dispatcher.Invoke(() => { 
				mediaShow.Visibility = Visibility.Hidden;
				media15s.Play();
				media15s.Visibility = Visibility.Visible;
			});
		}

		private void image_SourceUpdated(object sender, DataTransferEventArgs e)
		{
			WTF();
		}

		public void Open(int index) {
			// media Mở
			Dispatcher.Invoke(() => { 
				mediaOpenPiece.Position = TimeSpan.Zero; mediaOpenPiece.Play();
				images[index].Visibility = Visibility.Collapsed;
			});
		}
		public void OpenWord(int index, string word)
		{
			if (index < 4) wordControls[index].ShowAnswer(index, word);
		}
		public void CloseWord(int index)
		{
			if (index < 4) wordControls[index].DisAnswer(index);
		}

		public void SceneReset()
		{
			Dispatcher.Invoke(() => {	
				mediaShow.Visibility = Visibility.Hidden;
				media15s.Visibility = Visibility.Hidden;
				media15s.Position = TimeSpan.FromMilliseconds(2); media15s.Play();  media15s.Pause();
				qBox.Visibility = Visibility.Hidden;
			});
		}

		public void PlayerBelling(int player, string name)
		{
			Dispatcher.Invoke(() => {
				stackBell.Children.Add(new ObstaPlayerBelling(player, name));
				mediaBell.Position = TimeSpan.Zero; mediaBell.Play();
			});
		}
		public void FoundWinner()
		{
			foundWinner = true;
			Dispatcher.Invoke(() => {
				mediaBellCorrect.Position = TimeSpan.Zero; mediaBellCorrect.Play();
			});
		}
		public void RemoveStack(int player)
		{
			Dispatcher.Invoke(() => { 
				int len = stackBell.Children.Count;
				for (int i = 0; i < len; i++) {
					ObstaPlayerBelling tmp = (ObstaPlayerBelling)stackBell.Children[i];
					if (tmp.player == player) {
						stackBell.Children.RemoveAt(i); 
						if (!foundWinner) {
							mediaBellWrong.Position = TimeSpan.Zero; mediaBellWrong.Play();
						}
						break;
					}
				}
			});
		}
		public void Last15s()
		{
			Dispatcher.Invoke(() => {
				mediaLast15.Position = TimeSpan.Zero; mediaLast15.Play();
			});
		}
	}
}
