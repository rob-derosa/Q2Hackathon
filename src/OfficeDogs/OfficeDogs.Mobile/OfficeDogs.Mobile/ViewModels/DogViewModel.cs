using OfficeDogs.Common.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using OfficeDogs.Mobile;

namespace OfficeDogs.Mobile.ViewModels
{
	public class DogViewModel : BindableObject
	{
		HttpClient _client;

		Dog _dog = new Dog();
		public Dog Dog
		{
			get { return _dog; }
			set { _dog = value; OnPropertyChanged(); }
		}

		bool _isBusy;
		public bool IsBusy
		{
			get { return _isBusy; }
			set { _isBusy = value; OnPropertyChanged(); }
		}

		public int DogAge
		{
			get { return Dog.Age; }
			set { Dog.Age = value; OnPropertyChanged(); }
		}

		public async Task<bool> SaveDog()
		{
			try
			{
				IsBusy = true;
				Debug.WriteLine(Dog.Name);

				if(_client == null)
					_client = new HttpClient();

				var saved = await _client.SendPostRequest<Dog>("SaveDog", Dog);
				Dog = saved ?? new Dog();
				return saved != null;
			}
			catch (Exception e)
			{
				return false;
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}
