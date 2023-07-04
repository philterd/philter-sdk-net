/*******************************************************************************
 * Copyright 2023 Philterd, LLC
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
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace Philter
{
    /// <summary>
    /// Philter API client.
    /// </summary>
    public class PhilterClient
    {

        private readonly RestClient _client;

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
        /// <param name="certificatePfx">The certificate PFX file name.</param>
        /// <param name="privateKeyPassword">The private key's password.</param>
        public PhilterClient(string endpoint, string certificatePfx, SecureString privateKeyPassword)
        {
            X509Certificate2 certificate = new X509Certificate2(certificatePfx, privateKeyPassword, X509KeyStorageFlags.MachineKeySet);

            _client = new RestClient(endpoint);
            _client.ClientCertificates = new X509CertificateCollection() { certificate };
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

            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<ExplainResponse>(response.Content);
            }

            throw new ClientException("Unable to filter text. Check Philter's status.", response.ErrorException);

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
        /// Gets all alerts.
        /// </summary>
        /// <returns>A list of alerts.</returns>
        /// <exception cref="ClientException"></exception>
        public List<Alert> GetAlerts()
        {

            var request = new RestRequest("api/alerts", Method.GET);
            request.AddHeader("accept", "application/json");
            
            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                Alert[] alerts = JsonConvert.DeserializeObject<Alert[]>(response.Content);
                if (alerts != null)
                {
                    return alerts.ToList();
                }

                return new List<Alert>();

            }

            throw new ClientException("Unable to get alerts. Check Philter's status.", response.ErrorException);

        }
        
        /// <summary>
        /// Deletes an alert.
        /// </summary>
        /// <param name="alertId">The ID of the alert to delete.</param>
        /// <exception cref="ClientException"></exception>
        public void DeleteAlert(string alertId)
        {

            var request = new RestRequest("api/alerts/{alertId}", Method.DELETE).AddParameter("alertId", alertId);
            
            var response = _client.Execute(request);
            
            if (!response.IsSuccessful)
            {
                throw new ClientException("Unable to delete alert. Check Philter's status.", response.ErrorException);
            }

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