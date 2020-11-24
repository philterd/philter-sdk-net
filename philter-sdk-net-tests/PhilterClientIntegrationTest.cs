using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using RestSharp;

namespace Philter
{
    //[TestClass]
    public class PhilterClientIntegrationTest
    {
    
        //[TestMethod]
        public void FilterTest()
        {

            string certificate = "c:\\client-test.pfx";
            SecureString password =  new NetworkCredential("", "changeit").SecurePassword;

            PhilterClient philterClient = new PhilterClient(GetClient(certificate, password));
            string filteredText = philterClient.Filter("His SSN was 123-45-6789.", "context", "default");

            Assert.AreEqual("His SSN was {{{REDACTED-ssn}}}.", filteredText);

            Console.WriteLine(filteredText);

        }

        private RestClient GetClient(string certificateFile, SecureString password)
        {
            RestClient restClient = new RestClient
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true,
                BaseUrl = new Uri("https://philter:8080")
            };

            X509Certificate2 certificate = new X509Certificate2(certificateFile, password,
                X509KeyStorageFlags.MachineKeySet);

            restClient.ClientCertificates = new X509CertificateCollection() { certificate };

            return restClient;
        }

    }

}
