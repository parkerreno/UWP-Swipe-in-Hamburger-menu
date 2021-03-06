﻿using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Hamburger__menu_swipe_in
{

	public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
			InitializeHamburgerMenu();
			trnslttrnsfrmHamburgerBackground.X = -stckpnlMenuWidth;
			trnslttrnsfrmMenuTop.X = -stckpnlMenuWidth;
			trnslttrnsfrmMenuBottom.X = -stckpnlMenuWidth;
			pgMainPage.ManipulationMode = ManipulationModes.TranslateX;
		}

		#region "Hamburger menu"

		double stckpnlMenuWidth = 210;
		double InitialManipulationPoint;
		bool HamburgerMenuOpen = false;
		bool TriggerManipulation = false;

		private void InitializeHamburgerMenu()
		{
			shMenu0.From = -stckpnlMenuWidth;
			shMenu1.From = -stckpnlMenuWidth;
			shMenu2.From = -stckpnlMenuWidth;
			hdMenu0.To = -stckpnlMenuWidth;
			hdMenu1.To = -stckpnlMenuWidth;
			hdMenu2.To = -stckpnlMenuWidth;
			grdHamburgerMenu.Width = stckpnlMenuWidth;
			stckpnlMenuTop.Width = stckpnlMenuWidth;
			stckpnlMenuBottom.Width = stckpnlMenuWidth;
		}

		private void bttnHamburgerMenu_Tapped(object sender, TappedRoutedEventArgs e)
		{
			if (HamburgerMenuOpen)
			{
				HideMenu();
			}
			else
			{
				ShowMenu();
			}
		}

		private void MenuHome_Tapped(object sender, TappedRoutedEventArgs e)
		{
			txtblckMenuTapped.Text = "Home";
			HideMenu();
		}

		private void MenuAdd_Tapped(object sender, TappedRoutedEventArgs e)
		{
			txtblckMenuTapped.Text = "Add";
			HideMenu();
		}

		private void MenuPrivacy_Tapped(object sender, TappedRoutedEventArgs e)
		{
			txtblckMenuTapped.Text = "Privacy";
			HideMenu();
		}

		private void MenuTerms_Tapped(object sender, TappedRoutedEventArgs e)
		{
			txtblckMenuTapped.Text = "Terms";
			HideMenu();
		}

		private void MenuAbout_Tapped(object sender, TappedRoutedEventArgs e)
		{
			txtblckMenuTapped.Text = "About";
			HideMenu();
		}

		private void grdManagementOverlay_Tapped(object sender, TappedRoutedEventArgs e)
		{
			HideMenu();
		}

		private async void ShowMenu()
		{
			grdManagementOverlay.Visibility = Visibility.Visible;
			await strbrdShowMenu.BeginAsync();
			HamburgerMenuOpen = true;
		}

		private async void HideMenu()
		{
			HamburgerMenuOpen = false;
			grdManagementOverlay.Visibility = Visibility.Collapsed;
			await strbrdHideMenu.BeginAsync();
		}

		private void grdManagement_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
		{
			InitialManipulationPoint = e.Position.X;
			TriggerManipulation = HamburgerMenuOpen ? false : InitialManipulationPoint < 30 ? true : false;
		}

		private void grdManagement_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
		{

			if ((TriggerManipulation || HamburgerMenuOpen) && (!(HamburgerMenuOpen && InitialManipulationPoint < e.Position.X) || TriggerManipulation))
			{
				if (e.Position.X < stckpnlMenuWidth + 1)
				{
					HamburgerMenuOpen = true;
					Point currentpoint = e.Position;
					trnslttrnsfrmHamburgerBackground.X = e.Position.X < stckpnlMenuWidth ? -stckpnlMenuWidth + e.Position.X : 0;
					trnslttrnsfrmMenuTop.X = e.Position.X < stckpnlMenuWidth ? -stckpnlMenuWidth + e.Position.X : 0;
					trnslttrnsfrmMenuBottom.X = e.Position.X < stckpnlMenuWidth ? -stckpnlMenuWidth + e.Position.X : 0;
				}
			}
		}

		private async void grdManagement_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			if (!(HamburgerMenuOpen && InitialManipulationPoint < e.Position.X) || TriggerManipulation)
			{
				double X = e.Position.X > stckpnlMenuWidth ? stckpnlMenuWidth : e.Position.X;
				if (X > stckpnlMenuWidth / 2)
				{
					shMenu0.From = -stckpnlMenuWidth + X;
					shMenu1.From = -stckpnlMenuWidth + X;
					shMenu2.From = -stckpnlMenuWidth + X;
					await strbrdShowMenu.BeginAsync();
					shMenu0.From = -stckpnlMenuWidth;
					shMenu1.From = -stckpnlMenuWidth;
					shMenu2.From = -stckpnlMenuWidth;
					grdManagementOverlay.Visibility = Visibility.Visible;
				}
				else
				{
					grdManagementOverlay.Visibility = Visibility.Collapsed;
					hdMenu0.From = X - stckpnlMenuWidth;
					hdMenu1.From = X - stckpnlMenuWidth;
					hdMenu2.From = X - stckpnlMenuWidth;
					await strbrdHideMenu.BeginAsync();
					hdMenu0.From = 0;
					hdMenu1.From = 0;
					hdMenu2.From = 0;
					HamburgerMenuOpen = false;
				}
			}
			TriggerManipulation = false;
		}

		#endregion
	}

	public static class StoryboardExtensions
	{
		public static Task BeginAsync(this Storyboard storyboard)
		{
			try
			{
				TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
				if (storyboard == null)
					tcs.SetException(new ArgumentNullException());
				else
				{
					EventHandler<object> onComplete = null;
					onComplete = (s, e) => {
						storyboard.Completed -= onComplete;
						tcs.SetResult(true);
					};
					storyboard.Completed += onComplete;
					storyboard.Begin();
				}
				return tcs.Task;
			}
			catch
			{
				return null;
			}
		}

	}                                                                   // Extension for an asynchrone storyboard animation

}
