using OfficeDogs.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OfficeDogs.Mobile
{
	public partial class MainPage : ContentPage
	{
		public DogViewModel ViewModel { get; set; } = new DogViewModel();

		public MainPage()
		{
			BindingContext = ViewModel;
			InitializeComponent();
		}

		async void SaveClicked(object sender, EventArgs e)
		{
			var success = await ViewModel.SaveDog();

			if(!success)
			{
				await DisplayAlert("There was an error saving the dog.", "", "OK");
			}
		}
	}
}
