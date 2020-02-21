using Philter.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using System;
using System.Net;

namespace Philter
{
    public class PhilterClient
    {

        private readonly RestClient client;

        public PhilterClient(string endpoint, bool ignoreSelfSignedCertificates)
        {

            this.client = new RestClient(endpoint);

            if (ignoreSelfSignedCertificates)
            {
                client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            }

        }

        public string Filter(string text, string context, string filterProfileName)
        {

            var request = new RestRequest("api/filter", Method.POST);
            request.AddParameter("c", context);
            request.AddParameter("p", filterProfileName);
            request.AddHeader("accept", "text/plain");
            request.AddParameter("text/plain", text, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return response.Content;
            }
            else
            {
                throw new PhilterException("Unable to filter text. Check Philter's status.", response.ErrorException);
            }

        }

        public FilterResponse Explain(string text, string context, string filterProfileName)
        {

            var request = new RestRequest("api/explain", Method.POST);
            request.AddParameter("c", context);
            request.AddParameter("p", filterProfileName);
            request.AddHeader("accept", "application/json");
            request.AddParameter("text/plain", text, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<FilterResponse>(response.Content);
            }
            else
            {
                throw new PhilterException("Unable to filter text. Check Philter's status.", response.ErrorException);
            }

        }

        public List<ResponseSpan> GetReplacements(string documentId)
        {

            var request = new RestRequest("api/replacements", Method.GET);
            request.AddParameter("d", documentId);
            request.AddHeader("accept", "application/json");

            IRestResponse response = client.Execute(request);

            Console.WriteLine(response.StatusCode);

            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                throw new ReplacementStoreDisabledException("Philter's replacement store is not enabled.");
            }
            else
            {
                if (response.IsSuccessful)
                {
                    ResponseSpan[] spans = JsonConvert.DeserializeObject<ResponseSpan[]>(response.Content);
                    if (spans != null)
                    {
                        return spans.ToList();
                    }
                    else
                    {
                        return new List<ResponseSpan>();
                    }
                }
                else
                {
                    throw new PhilterException("Unable to get replacements. Check Philter's status.", response.ErrorException);
                }
            }

        }

        public bool GetStatus()
        {

            var request = new RestRequest("api/status", Method.GET);

            IRestResponse response = client.Execute(request);

            return response.IsSuccessful;

        }

    }

}
