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
using System.IO;
using System.Linq;
using RestSharp;
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

            var options = new RestClientOptions(endpoint)
            {
                ClientCertificates = new X509CertificateCollection() { certificate }
            };
            _client = new RestClient(options);

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
        /// <param name="policyName">The name of the policy to apply.</param>
        /// <returns>The filtered text.</returns>
        /// <exception cref="ClientException"></exception>
        public string Filter(string text, string context, string policyName)
        {
            return Filter(text, context, string.Empty, policyName);
        }

        /// <summary>
        /// Sends text to Philter for filtering.
        /// </summary>
        /// <param name="text">The text to be filtered.</param>
        /// <param name="context">The context.</param>
        /// <param name="documentId">The document ID.</param>
        /// <param name="policyName">The name of the policy to apply.</param>
        /// <returns>The filtered text.</returns>
        /// <exception cref="ClientException"></exception>
        public string Filter(string text, string context, string documentId, string policyName)
        {

            var request = new RestRequest("api/filter", Method.Post);
            request.AddParameter("c", context);
            request.AddParameter("p", policyName);
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
        /// Sends a PDF document to Philter for filtering.
        /// </summary>
        /// <param name="fileName">The filename of the PDF document.</param>
        /// <param name="context">The context.</param>
        /// <param name="documentId">The document ID.</param>
        /// <param name="policyName">The name of the policy to apply.</param>
        /// <param name="responseFormat">The desired format of the returned filtered document.</param>
        /// <returns>The filtered document.</returns>
        /// <exception cref="ClientException"></exception>
        public byte[] Filter(String fileName, string context, string documentId, string policyName, ResponseFormat responseFormat)
        {
            var request = new RestRequest("api/filter", Method.Post);

            var bytes = File.ReadAllBytes(fileName);

            request.AddParameter("application/pdf", bytes, ParameterType.RequestBody);
            request.AddParameter("c", context);
            request.AddParameter("p", policyName);

            request.AddHeader("Accept", responseFormat == ResponseFormat.Pdf ? "application/pdf" : "image/jpeg");

            if (documentId != string.Empty)
            {
                request.AddParameter("d", documentId);
            }
            
            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                return response.RawBytes;
            }
            
            throw new ClientException("Unable to filter document. Check Philter's status.", response.ErrorException);

        }

        /// <summary>
        /// Sends text to Philter for filtering with an explanation response.
        /// </summary>
        /// <param name="text">The text to be filtered.</param>
        /// <param name="context">The context.</param>
        /// <param name="policyName">The name of the policy to apply.</param>
        /// <returns>The filtered text with the filtering explanation.</returns>
        /// <exception cref="ClientException"></exception>
        public ExplainResponse Explain(string text, string context, string policyName)
        {
            return Explain(text, context, String.Empty, policyName);
        }

        /// <summary>
        /// Sends text to Philter for filtering with an explanation response.
        /// </summary>
        /// <param name="text">The text to be filtered.</param>
        /// <param name="context">The context.</param>
        /// <param name="documentId">The document ID.</param>
        /// <param name="policyName">The name of the policy to apply.</param>
        /// <returns>The filtered text with the filtering explanation.</returns>
        /// <exception cref="ClientException"></exception>
        public ExplainResponse Explain(string text, string context, string documentId, string policyName)
        {

            var request = new RestRequest("api/explain", Method.Post);
            request.AddParameter("c", context);
            request.AddParameter("p", policyName);
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

            var request = new RestRequest("api/status", Method.Get);

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

            var request = new RestRequest("api/alerts", Method.Get);
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

            var request = new RestRequest("api/alerts/{alertId}", Method.Delete).AddParameter("alertId", alertId);
            
            var response = _client.Execute(request);
            
            if (!response.IsSuccessful)
            {
                throw new ClientException("Unable to delete alert. Check Philter's status.", response.ErrorException);
            }

        }

        /// <summary>
        /// Get the names of available policies.
        /// </summary>
        /// <returns>A list of policy names.</returns>
        /// <exception cref="ClientException"></exception>
        public List<string> GetPolicies()
        {

            var request = new RestRequest("api/profiles", Method.Get);
            request.AddHeader("accept", "application/json");
            
            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<List<string>>(response.Content);
            }

            throw new ClientException("Unable to get policies.", response.ErrorException);

        }

        /// <summary>
        /// Get a policy.
        /// </summary>
        /// <param name="policyName">The name of a policy.</param>
        /// <returns>The policy.</returns>
        /// <exception cref="ClientException"></exception>
        public string GetPolicy(string policyName)
        {

            var request = new RestRequest("api/profiles/{policyName}", Method.Get);
            request.AddParameter("policyName", policyName, ParameterType.UrlSegment);
            request.AddHeader("accept", "application/json");
            
            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                return response.Content;
            }

            throw new ClientException("Unable to get policy.", response.ErrorException);

        }

        /// <summary>
        /// Upload a policy. If a policy with the same name already exists it will be overwritten.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <exception cref="ClientException"></exception>
        public void SavePolicy(string policy)
        {

            var request = new RestRequest("api/profiles", Method.Post);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", policy, ParameterType.RequestBody);
            
            var response = _client.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new ClientException("Unable to save policy.", response.ErrorException);
            }

        }

        /// <summary>
        /// Delete a policy.
        /// </summary>
        /// <param name="policyName">The name of the policy to delete.</param>
        /// <exception cref="ClientException"></exception>
        public void DeletePolicy(string policyName)
        {

            var request = new RestRequest("api/profiles/{policyName}", Method.Delete);
            request.AddParameter("policyName", policyName, ParameterType.UrlSegment);

            var response = _client.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new ClientException("Unable to delete policy.", response.ErrorException);
            }

        }

        private string Base64Encode(string plainText)
        {
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(plainText));
        }

    }

}