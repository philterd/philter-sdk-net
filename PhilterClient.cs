/*******************************************************************************
 * Copyright 2020 Mountain Fog, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License.  You may obtain a copy
 * of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the
 * License for the specific language governing permissions and limitations under
 * the License.
 ******************************************************************************/

using Philter.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using System;
using System.Net;

namespace Philter
{
    /// <summary>
    /// Philter API client.
    /// </summary>
    public class PhilterClient
    {

        private readonly RestClient _client;

        public PhilterClient(string endpoint, bool ignoreSelfSignedCertificates)
        {

            _client = new RestClient(endpoint);

            if (ignoreSelfSignedCertificates)
            {
                _client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            }

        }

        public string Filter(string text, string context, string filterProfileName)
        {

            var request = new RestRequest("api/filter", Method.POST);
            request.AddParameter("c", context);
            request.AddParameter("p", filterProfileName);
            request.AddHeader("accept", "text/plain");
            request.AddParameter("text/plain", text, ParameterType.RequestBody);

            IRestResponse response = _client.Execute(request);

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

            IRestResponse response = _client.Execute(request);

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

            IRestResponse response = _client.Execute(request);

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

            IRestResponse response = _client.Execute(request);

            return response.IsSuccessful;

        }

    }

}
