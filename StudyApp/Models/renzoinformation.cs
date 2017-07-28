using System;
using Newtonsoft.Json;

namespace StudyApp
{
    public class renzoinformation
    {
		[JsonProperty(PropertyName = "Id")]
		public string ID { get; set; }

		[JsonProperty(PropertyName = "Study")]
		public string Study { get; set; }

		[JsonProperty(PropertyName = "Time")]
		public string Time { get; set; }

		[JsonProperty(PropertyName = "Day")]
		public string Day { get; set; }

		//      [JsonProperty(PropertyName = "Course")]
		//      public string Course { get; set; }
	}
}
