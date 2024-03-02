using DescartesAssignment.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace DescartesAssignment.IntegrationTests
{
    [TestClass]
    public class DifferentiatingApiTests
    {
        [TestMethod]
        public async Task GetDifferencesAsync_ReturnsNotFound_WhenNothingIsPutInDb()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var response = await GetDifferencesAsync("v1/diff/1", httpClient);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }
        [TestMethod]
        public async Task SaveData_LeftSide_ReturnsCreated()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var response = await PutValueToDbAsync("AAAAAA==", "/v1/diff/1/left", httpClient);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
        }
        [TestMethod]
        public async Task GetDifferencesAsync_ReturnsNotFound_WhenOnlyOneSideIsPutInDb()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();
            await PutValueToDbAsync("AAAAAA==", "/v1/diff/1/left", httpClient);
            var response = await GetDifferencesAsync("v1/diff/1", httpClient);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }
        [TestMethod]
        public async Task SaveData_RightSide_ReturnsCreated()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var response = await PutValueToDbAsync("AAAAAA==", "/v1/diff/1/right", httpClient);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
        }
        [TestMethod]
        public async Task GetDifferencesAsync_ReturnsEquals()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();
            await PutValueToDbAsync("AAAAAA==", "/v1/diff/1/left", httpClient);
            await PutValueToDbAsync("AAAAAA==", "/v1/diff/1/right", httpClient);
            var response = await GetDifferencesAsync("v1/diff/1", httpClient);

            DifferencesResponse differencesResponse = JsonConvert.DeserializeObject<DifferencesResponse>(response.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(differencesResponse.DiffResultType, "Equals");
        }
        [TestMethod]
        public async Task GetDifferencesAsync_ReturnsContentDoNotMatchAndSpecifiedDifferences()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();
            await PutValueToDbAsync("AAAAAA==", "/v1/diff/1/left", httpClient);
            await PutValueToDbAsync("AQABAQ==", "/v1/diff/1/right", httpClient);
            var response = await GetDifferencesAsync("v1/diff/1", httpClient);

            DifferencesResponse differencesResponse = JsonConvert.DeserializeObject<DifferencesResponse>(response.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(differencesResponse.DiffResultType, "ContentDoNotMatch");
            Assert.AreEqual(differencesResponse.Diffs.Count, 2);
            Assert.AreEqual(differencesResponse.Diffs[0].Offset, 0);
            Assert.AreEqual(differencesResponse.Diffs[0].Length, 1);
            Assert.AreEqual(differencesResponse.Diffs[1].Offset, 2);
            Assert.AreEqual(differencesResponse.Diffs[1].Length, 2);
        }
        [TestMethod]
        public async Task GetDifferencesAsync_ReturnsSizeDoNotMatch()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();
            await PutValueToDbAsync("AAA=", "/v1/diff/1/left", httpClient);
            await PutValueToDbAsync("AQABAQ==", "/v1/diff/1/right", httpClient);
            var response = await GetDifferencesAsync("v1/diff/1", httpClient);

            DifferencesResponse differencesResponse = JsonConvert.DeserializeObject<DifferencesResponse>(response.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(differencesResponse.DiffResultType, "SizeDoNotMatch");
        }
        [TestMethod]
        public async Task SaveData_LeftSide_ReturnsBadRequest()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var response = await PutValueToDbAsync(null, "/v1/diff/1/left", httpClient);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }
        
        private async Task<HttpResponseMessage> GetDifferencesAsync(string requestUri, HttpClient client)
        {
            return await client.GetAsync(requestUri);
        }
        private async Task<HttpResponseMessage> PutValueToDbAsync(string data, string requestUri, HttpClient client)
        {
            var cm = new ReceivedData();
            cm.Data = data;
            var stringContent = new StringContent(JsonConvert.SerializeObject(cm), Encoding.UTF8, "application/json");
            return await client.PutAsync(requestUri, stringContent);
        }
    }
}