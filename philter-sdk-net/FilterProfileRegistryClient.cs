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

using Newtonsoft.Json;
using System.Collections.Generic;
using RestSharp;
using Philter.Model;

namespace Philter
{
    /// <summary>
    /// A client for the Filter Profile Registry.
    /// </summary>
    public class FilterProfileRegistryClient
    {

        private readonly RestClient _client;

        /// <summary>
        /// Creates a new client.
        /// </summary>
        /// <param name="endpoint">The Philter or Filter Profile Registry endpoint, e.g. https://localhost:8080.</param>
        public FilterProfileRegistryClient(string endpoint)
        {
            _client = new RestClient(endpoint);
        }
        
        /// <summary>
        /// Creates a new Filter Registry client client.
        /// </summary>
        /// <param name="restClient">A custom RestClient.</param>
        public FilterProfileRegistryClient(RestClient restClient)
        {
            _client = restClient;
        }

        /// <summary>
        /// Get the names of available filter profiles.
        /// </summary>
        /// <returns>A list of filter profile names.</returns>
        /// <exception cref="ClientException"></exception>
        public List<string> Get()
        {

            var request = new RestRequest("api/profiles", Method.GET);
            request.AddHeader("accept", "application/json");

            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<List<string>>(response.Content);
            }
            else
            {
                throw new ClientException("Unable to get filter profiles.", response.ErrorException);
            }

        }

        /// <summary>
        /// Get a filter profile.
        /// </summary>
        /// <param name="filterProfileName">The name of a filter profile.</param>
        /// <returns>The filter profile.</returns>
        /// <exception cref="ClientException"></exception>
        public string Get(string filterProfileName)
        {

            var request = new RestRequest("api/profiles/{filterProfileName}", Method.GET);
            request.AddParameter("filterProfileName", filterProfileName, ParameterType.UrlSegment);
            request.AddHeader("accept", "application/json");

            var response = _client.Execute(request);

            if (response.IsSuccessful)
            {
                return response.Content;
            }
            else
            {
                throw new ClientException("Unable to get filter profile.", response.ErrorException);
            }

        }

        /// <summary>
        /// Upload a filter profile. If a filter profile with the same name already exists it will be overwritten.
        /// </summary>
        /// <param name="filterProfile">The filter profile.</param>
        /// <exception cref="ClientException"></exception>
        public void Save(string filterProfile)
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
        public void Delete(string filterProfileName)
        {

            var request = new RestRequest("api/profiles/{filterProfileName}", Method.DELETE);
            request.AddParameter("filterProfileName", filterProfileName, ParameterType.UrlSegment);

            var response = _client.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new ClientException("Unable to delete filter profile.", response.ErrorException);
            }

        }

    }
}
