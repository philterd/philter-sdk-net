using FluentSim;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Philter.Model;
using RestSharp;
using System;
using System.Collections.Generic;

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
        }

        [TestCleanup]
        public void CleanUp()
        {
            simulator.Stop();
        }

        [TestMethod]
        public void FilterTest()
        {

            simulator.Post("/api/filter").Responds("His SSN was {{{REDACTED-ssn}}}.").WithCode(200);

            PhilterClient philterClient = new PhilterClient(GetClient());
            string filteredText = philterClient.Filter("His SSN was 123-45-6789.", "context", "default");

            Assert.AreEqual("His SSN was {{{REDACTED-ssn}}}.", filteredText);

        }

        [TestMethod]
        public void GetReplacementsBadRequestTest()
        {

            simulator.Get("/api/replacements").WithParameter("d", "").Responds("").WithCode(400);

            PhilterClient philterClient = new PhilterClient(GetClient());
            Assert.ThrowsException<ClientException>(() => philterClient.Filter("His SSN was 123-45-6789.", "context", "default"));

        }

        [TestMethod]
        public void GetReplacementsTest()
        {

            Span span = new Span();
            span.CharacterStart = 1;
            span.CharacterEnd = 2;
            span.Text = "A";

            List<Span> spans = new List<Span>();
            spans.Add(span);

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(spans);

            simulator.Get("/api/replacements").WithParameter("d", "1234").Responds(json).WithCode(200);

            PhilterClient philterClient = new PhilterClient(GetClient());
            List<Span> replacementSpans = philterClient.GetReplacements("1234");

            Assert.IsNotNull(replacementSpans);
            Assert.AreEqual(1, replacementSpans.Count);

        }

        [TestMethod]
        public void ExplainTest()
        {

            Span span = new Span();
            span.CharacterStart = 1;
            span.CharacterEnd = 2;
            span.Text = "A";

            List<Span> spans = new List<Span>();
            spans.Add(span);

            Explanation explanation = new Explanation();
            explanation.AppliedSpans = spans;
            explanation.IgnoredSpans = new List<Span>();

            ExplainResponse mockResponse = new ExplainResponse();
            mockResponse.FilteredText = "His SSN was {{{REDACTED-ssn}}}.";
            mockResponse.Explanation = explanation;

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(mockResponse);

            simulator.Post("/api/explain").Responds(json).WithCode(200);

            PhilterClient philterClient = new PhilterClient(GetClient());
            ExplainResponse explainResponse = philterClient.Explain("His SSN was 123-45-6789.", "context", "invalid");

            Assert.IsNotNull(explainResponse);
            Assert.AreEqual(1, explainResponse.Explanation.AppliedSpans.Count);
            Assert.AreEqual(0, explainResponse.Explanation.IgnoredSpans.Count);

        }

        [TestMethod]
        public void FilterBadRequestTest()
        {

            simulator.Post("/api/filter").Responds("His SSN was {{{REDACTED-ssn}}}.").WithCode(400);
            PhilterClient philterClient = new PhilterClient(GetClient());
            Assert.ThrowsException<ClientException>(() => philterClient.Filter("His SSN was 123-45-6789.", "context", "invalid"));

        }

        [TestMethod]
        public void StatusText()
        {

            simulator.Get("/api/status").Responds("{\"Status\": \"Healthy\", \"Version\": \"1.0.0\"}").WithCode(200);

            PhilterClient philterClient = new PhilterClient(GetClient());
            StatusResponse statusResponse = philterClient.GetStatus();

            Assert.AreEqual("Healthy", statusResponse.Status);
            Assert.AreEqual("1.0.0", statusResponse.Version);

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
