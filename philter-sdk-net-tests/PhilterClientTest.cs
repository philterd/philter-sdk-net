using FluentSim;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Philter.Model;
using RestSharp;
using System;

namespace Philter
{
    [TestClass]
    public class PhilterClientTest
    {

        FluentSimulator simulator = new FluentSimulator("http://localhost:18081/");

        [TestInitialize()]
        public void Initialize()
        {
            simulator.Start();
            simulator.Post("/api/filter").Responds("His SSN was {{{REDACTED-ssn}}}.").WithCode(200);
            simulator.Get("/api/status").Responds("{\"Status\": \"Healthy\", \"Version\": \"1.0.0\"}").WithCode(200);
        }

        [TestCleanup]
        public void CleanUp()
        {
            simulator.Stop();
        }

        [TestMethod]
        public void FilterTest()
        {

            PhilterClient philterClient = new PhilterClient(GetClient());
            string filteredText = philterClient.Filter("His SSN was 123-45-6789.", "context", "default");

            Assert.AreEqual("His SSN was {{{REDACTED-ssn}}}.", filteredText);

        }

        [TestMethod]
        public void StatusText()
        {

            PhilterClient philterClient = new PhilterClient(GetClient());
            StatusResponse statusResponse = philterClient.GetStatus();

            Assert.AreEqual("Healthy", statusResponse.Status);

        }

        private RestClient GetClient()
        {
            RestClient restClient = new RestClient();
            restClient.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            restClient.BaseUrl = new Uri("http://localhost:18081/");

            return restClient;
        }

    }

}
