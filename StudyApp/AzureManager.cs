using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace StudyApp
{
    public class AzureManager
    {

		private static AzureManager instance;
		private MobileServiceClient client;
		private IMobileServiceTable<renzoinformation> renzotable;

        public AzureManager()
        {
			this.client = new MobileServiceClient("https://renzosapp.azurewebsites.net");
			this.renzotable = this.client.GetTable<renzoinformation>();
        }

		public MobileServiceClient AzureClient
		{
			get { return client; }
		}

		public static AzureManager AzureManagerInstance
		{
			get
			{
				if (instance == null)
				{
					instance = new AzureManager();
				}

				return instance;
			}
		}

		public async Task<List<renzoinformation>> getrenzoinformation()
		{
			return await this.renzotable.ToListAsync();
		}

		public async Task postrenzoinformation(renzoinformation renzoinformation)
		{
			await this.renzotable.InsertAsync(renzoinformation);
		}
    }
}
