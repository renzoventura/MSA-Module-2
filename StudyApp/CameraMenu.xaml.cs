using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Diagnostics.Contracts;

namespace StudyApp
{
    public partial class CameraMenu : ContentPage
    {
        public CameraMenu()
        {
            InitializeComponent();
        }

		string x = DateTime.Now.DayOfWeek.ToString();
		//        string[] array_probability = new string[2];
		//      string[] array_tags = new string[2];
		//   int a;
		//  string current_course;


		private async void loadCamera(object sender, EventArgs e)
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{
				await DisplayAlert("Camera unavailable", "Camera cannot be accessed in device.", "OK");
				return;
			}
			MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
			{
				PhotoSize = PhotoSize.Medium,
				Directory = "Sample",
				Name = $"{DateTime.UtcNow}.jpg"
			});
			if (file == null)
				return;
			image.Source = ImageSource.FromStream(() =>
			{
				return file.GetStream();
			});
			await MakePredictionRequest(file);
		}
		static byte[] GetImageAsByteArray(MediaFile file)
		{
			var stream = file.GetStream();
			BinaryReader binaryReader = new BinaryReader(stream);
			return binaryReader.ReadBytes((int)stream.Length);
		}
		//put cognigtive, sends photo custom api

		async Task MakePredictionRequest(MediaFile file)
		{
			var client = new HttpClient();
			client.DefaultRequestHeaders.Add("Prediction-Key", "c6b2b330bd2c43eea73185db8a0b2a10");
			string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/453f231c-df1c-4729-bdae-e8f4e7a61e95/image?iterationId=225aaf85-99dc-49c6-87db-c16facb3ee88";
			HttpResponseMessage response;
			byte[] byteData = GetImageAsByteArray(file);
			using (var content = new ByteArrayContent(byteData))
			{
				content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
				response = await client.PostAsync(url, content);
				if (response.IsSuccessStatusCode)
				{
					var responseString = await response.Content.ReadAsStringAsync();
					JObject rss = JObject.Parse(responseString);

					var Probability = from p in rss["Predictions"] select (string)p["Probability"];
					var Tag = from p in rss["Predictions"] select (string)p["Tag"];
					foreach (var item in Tag)
					{
						TagLabel.Text += item + ": \n";
					}
					foreach (var item in Probability)
					{
						PredictionLabel.Text += item + "\n";
					}

					//  foreach (String item in Tag)
					//  {
					//      a = 0;
					//      array_tags[a] = item;
					//       a = a + 1;
					//  }

					//                    foreach (String item in Probability)
					//                  {
					//                    a = 0;
					//                  array_probability[0] = item;
					//                  a = a + 1;
					//            }
					//  foreach (var item in Probability)
					//  {
					//      PredictionLabel.Text += item + "\n";
					//Test
					//         if (int.Parse(item) > 0.5)
					//          {
					//              PredictionLabel.Text += "You Studied cs210!\n";
					//          }
					//          else
					//          {
					//              PredictionLabel.Text += "You did not study cs 210\n";
					//          }
					//      }

					await senddata();
				}
				//Get rid of file once we have finished using it
				file.Dispose();
			}
		}

		async Task senddata()
		{
			//    if (Int32.Parse(array_probability[0]) > Int32.Parse(array_probability[1]))
			//      current_course = array_tags[0];
			//  else
			//     current_course = array_tags[1];
			renzoinformation model = new renzoinformation()
			{




				Day = x + " " + DateTime.Today.ToString("dd-MM-yyyy"),
				Time = DateTime.Now.ToString("h:mm tt"),
				//  Course = current_course


			};
			await AzureManager.AzureManagerInstance.postrenzoinformation(model);
		}




	}
}
