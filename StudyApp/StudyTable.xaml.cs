using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

namespace StudyApp
{
    public partial class StudyTable : ContentPage
    {
        public StudyTable()
        {
            InitializeComponent();
        }
		async void load_records(object sender, System.EventArgs e)
		{
			loading.IsRunning = true;
			List<renzoinformation> renzosapp = await AzureManager.AzureManagerInstance.getrenzoinformation();
			StudyList.ItemsSource = renzosapp;
			loading.IsRunning = false;
		}
		private async void open_cameramenu(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new CameraMenu());
		}


	}
}
