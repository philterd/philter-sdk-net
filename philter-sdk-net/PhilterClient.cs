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

using System;
using Philter.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using System.Net;

namespace Philter
{
    /// <summary>
    /// Philter API client.
    /// </summary>
    public class PhilterClient
    {

        private readonly RestClient _client;
        private readonly string _token;
        
        /// <summary>
        /// Creates a new Philter client.
        /// </summary>
        /// <param name="endpoint">The Philter API endpoint, e.g. https://localhost:8080.</param>
        public PhilterClient(string endpoint)
        {
            _client = new RestClient(endpoint);
        }
        
        /// <summary>
        /// Creates a new Philter client.
        /// </summary>
        /// <param name="endpoint">The Philter API endpoint, e.g. https://localhost:8080.</param>
        /// <<param name="token">The authentication token or <code>null</code>.</param>
        public PhilterClient(string endpoint, string token)
        {
            _client = new RestClient(endpoint);
            _token = token;
        }
        
        /// <summary>
        /// Creates a new Philter client.
        /// </summary>
        /// <param name="restClient">A custom RestClient.</param>
        public PhilterClient(RestClient restClient)
        {
            _client = restClient;
        }

        /// <summary>
        /// Creates a new Philter client.
        /// </summary>
        /// <param name="restClient">A custom RestClient.</param>
        /// <<param name="token">The authentication token or <code>null</code>.</param>
        public PhilterClient(RestClient restClient, string token)
        {
            _client = restClient;
            _token = token;
        }
        
        /// <summary>
        /// Sends text to Philter for filtering.
        /// </summary>
        /// <param name="text">The text to be filtered.</param>
        /// <param name="context">The context.</param>
        /// <param name="filterProfileName">The name of the filter profile to apply.</param>
        /// <returns>The filtered text.</returns>
        /// <exception cref="ClientException"></exception>
        public string Filter(string text, string context, string filterProfileName)
        {
            return Filter(text, context, String.Empty, filterProfileName);
        }

        /// <summary>
        /// Sends text to Philter for filtering.
        /// </summary>
        /// <param name="text">The text to be filtered.</param>
        /// <param name="context">The context.</param>
        /// <param name="documentId">The document ID.</param>
        /// <param name="filterProfileName">The name of the filter profile to apply.</param>
        /// <returns>The filtered text.</returns>
        /// <exception cref="ClientException"></exception>
        public string Filter(string text, string context, string documentId, string filterProfileName)
        {

            var request = new RestRequest("api/filter", Method.POST);
            request.AddParameter("c", context);
            request.AddParameter("p", filterProfileName);
            request.AddHeader("accept", "text/plain");
            request.AddParameter("text/plain", text, ParameterType.RequestBody);

            if (_token != null)
            {
                request.AddHeader("Authorization", Base64Encode("token:" + _token));
            }
            
            if (documentId != String.Empty)
            {
                request.AddParameter("d", documentId);
            }

            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                return response.Content;
            }

            throw new ClientException("Unable to filter text. Check Philter's status.", response.ErrorException);

        }

        /// <summary>
        /// Sends text to Philter for filtering with an explanation response.
        /// </summary>
        /// <param name="text">The text to be filtered.</param>
        /// <param name="context">The context.</param>
        /// <param name="filterProfileName">The name of the filter profile to apply.</param>
        /// <returns>The filtered text with the filtering explanation.</returns>
        /// <exception cref="ClientException"></exception>
        public ExplainResponse Explain(string text, string context, string filterProfileName)
        {
            return Explain(text, context, String.Empty, filterProfileName);
        }

        /// <summary>
        /// Sends text to Philter for filtering with an explanation response.
        /// </summary>
        /// <param name="text">The text to be filtered.</param>
        /// <param name="context">The context.</param>
        /// <param name="documentId">The document ID.</param>
        /// <param name="filterProfileName">The name of the filter profile to apply.</param>
        /// <returns>The filtered text with the filtering explanation.</returns>
        /// <exception cref="ClientException"></exception>
        public ExplainResponse Explain(string text, string context, string documentId, string filterProfileName)
        {

            var request = new RestRequest("api/explain", Method.POST);
            request.AddParameter("c", context);
            request.AddParameter("p", filterProfileName);
            request.AddHeader("accept", "application/json");
            request.AddParameter("text/plain", text, ParameterType.RequestBody);

            if (documentId != String.Empty)
            {
                request.AddParameter("d", documentId);
            }
            
            if (_token != null)
            {
                request.AddHeader("Authorization", Base64Encode("token:" + _token));
            }

            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<ExplainResponse>(response.Content);
            }

            throw new ClientException("Unable to filter text. Check Philter's status.", response.ErrorException);

        }

        /// <summary>
        /// Gets the replacements made by Philter in a document.
        /// </summary>
        /// <param name="documentId">The document ID.</param>
        /// <returns>A list of spans that were identified as sensitive information in the given document.</returns>
        /// <exception cref="ClientException"></exception>
        public List<Span> GetReplacements(string documentId)
        {

            var request = new RestRequest("api/replacements", Method.GET);
            request.AddParameter("d", documentId);
            request.AddHeader("accept", "application/json");

            if (_token != null)
            {
                request.AddHeader("Authorization", Base64Encode("token:" + _token));
            }
            
            var response = _client.Execute(request);
            
            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                throw new ClientException("Philter's replacement store is not enabled.");
            }

            if (response.IsSuccessful)
            {
                Span[] spans = JsonConvert.DeserializeObject<Span[]>(response.Content);
                if (spans != null)
                {
                    return spans.ToList();
                }

                return new List<Span>();

            }

            throw new ClientException("Unable to get spans. Check Philter's status.", response.ErrorException);

        }

        /// <summary>
        /// Gets the status of Philter.
        /// </summary>
        /// <returns>The status of Philter.</returns>
        /// <exception cref="ClientException"></exception>
        public StatusResponse GetStatus()
        {

            var request = new RestRequest("api/status", Method.GET);

            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<StatusResponse>(response.Content);
            }

            throw new ClientException("Unable to get status.", response.ErrorException);

        }
        
                /// <summary>
        /// Get the names of available filter profiles.
        /// </summary>
        /// <returns>A list of filter profile names.</returns>
        /// <exception cref="ClientException"></exception>
        public List<string> GetFilterProfiles()
        {

            var request = new RestRequest("api/profiles", Method.GET);
            request.AddHeader("accept", "application/json");

            if (_token != null)
            {
                request.AddHeader("Authorization", Base64Encode("token:" + _token));
            }
            
            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<List<string>>(response.Content);
            }
            
            throw new ClientException("Unable to get filter profiles.", response.ErrorException);

        }

        /// <summary>
        /// Get a filter profile.
        /// </summary>
        /// <param name="filterProfileName">The name of a filter profile.</param>
        /// <returns>The filter profile.</returns>
        /// <exception cref="ClientException"></exception>
        public string GetFilterProfile(string filterProfileName)
        {

            var request = new RestRequest("api/profiles/{filterProfileName}", Method.GET);
            request.AddParameter("filterProfileName", filterProfileName, ParameterType.UrlSegment);
            request.AddHeader("accept", "application/json");
            
            if (_token != null)
            {
                request.AddHeader("Authorization", Base64Encode("token:" + _token));
            }

            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                return response.Content;
            }

            throw new ClientException("Unable to get filter profile.", response.ErrorException);

        }

        /// <summary>
        /// Upload a filter profile. If a filter profile with the same name already exists it will be overwritten.
        /// </summary>
        /// <param name="filterProfile">The filter profile.</param>
        /// <exception cref="ClientException"></exception>
        public void SaveFilterProfile(string filterProfile)
        {

            var request = new RestRequest("api/profiles", Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", filterProfile, ParameterType.RequestBody);
            
            if (_token != null)
            {
                request.AddHeader("Authorization", Base64Encode("token:" + _token));
            }

            var response = _client.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new ClientException("Unable to save filter profile.", response.ErrorException);
            }

        }

        /// <summary>
        /// Delete a filter profile.
        /// </summary>
        /// <param name="filterProfileName">The name of the filter profile to delete.</param>
        /// <exception cref="ClientException"></exception>
        public void DeleteFilterProfile(string filterProfileName)
        {

            var request = new RestRequest("api/profiles/{filterProfileName}", Method.DELETE);
            request.AddParameter("filterProfileName", filterProfileName, ParameterType.UrlSegment);
            
            if (_token != null)
            {
                request.AddHeader("Authorization", Base64Encode("token:" + _token));
            }

            var response = _client.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new ClientException("Unable to delete filter profile.", response.ErrorException);
            }

        }
        
        private string Base64Encode(string plainText)
        {
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(plainText));
        }

    }

}