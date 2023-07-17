using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using Philter.Model;
using RestSharp;

namespace Philter
{
    [TestClass]
    public class PhilterClientIntegrationTest
    {
    
        [Ignore]
        [TestMethod]
        public void FilterTextTest()
        {
            string certificate = "c:\\client-test.pfx";
            SecureString password =  new NetworkCredential("", "changeit").SecurePassword;

            PhilterClient philterClient = new PhilterClient(GetClient(certificate, password));
            string filteredText = philterClient.Filter("His SSN was 123-45-6789.", "context", "default");

            Assert.AreEqual("His SSN was {{{REDACTED-ssn}}}.", filteredText);

            Console.WriteLine(filteredText);
        }
        
        [Ignore]
        [TestMethod]
        public void FilterPdfTest()
        {
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Input\12-12110 K.pdf");
            PhilterClient philterClient = new PhilterClient(GetClient());
            byte[] response = philterClient.Filter(fileName, "context", "docid", "default", ResponseFormat.Pdf);

            string outputFileName = Path.GetTempPath() + Guid.NewGuid() + ".pdf";
            File.WriteAllBytes(outputFileName, response);
            
            Console.WriteLine("Output written to: " + outputFileName);
        }

        private RestClient GetClient()
        {
            RestClient restClient = new RestClient
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true,
                BaseUrl = new Uri("https://10.0.2.51:8080")
            };

            return restClient;
        }
        
        private RestClient GetClient(string certificateFile, SecureString password)
        {
            RestClient restClient = new RestClient
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true,
                BaseUrl = new Uri("https://10.0.2.51:8080")
            };

            X509Certificate2 certificate = new X509Certificate2(certificateFile, password,
                X509KeyStorageFlags.MachineKeySet);

            restClient.ClientCertificates = new X509CertificateCollection() { certificate };

            return restClient;
        }

    }

}
