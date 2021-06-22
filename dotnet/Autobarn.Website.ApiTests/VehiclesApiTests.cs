using System.Net.Http;
using System.Text;
using Autobarn.Data.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Autobarn.Website.ApiTests {
	public class VehiclesApiTests : IClassFixture<WebApplicationFactory<Startup>> {
		private readonly WebApplicationFactory<Startup> factory;

		public VehiclesApiTests(WebApplicationFactory<Startup> factory) {
			this.factory = factory;
		}

		[Fact]
		public async void WebsiteWorks() {
			var client = factory.CreateClient();
			var response = await client.GetAsync("/");
			response.EnsureSuccessStatusCode();
		}

		[Fact]
		public async void ApiWorks() {
			var client = factory.CreateClient();
			var response = await client.GetAsync("/api");
			response.EnsureSuccessStatusCode();
		}

		[Fact]
		public async void CarWorks() {
			var client = factory.CreateClient();
			var putData = new {
				modelCode = "volkswagen-beetle",
				registration = "TEST1234",
				color = "Green",
				year = "1985"
			};
			await client.PutAsync("/api/vehicles/test1234",
				new StringContent(JsonConvert.SerializeObject(putData), Encoding.UTF8, "application/json"));
			var response = await client.GetAsync("/api/vehicles/test1234");
			Assert.True(response.IsSuccessStatusCode);
			var json = response.Content.ReadAsStringAsync().Result;
			var data = JsonConvert.DeserializeObject<Vehicle>(json);
			Assert.Equal("Green", data.Color);
		}
	}
}