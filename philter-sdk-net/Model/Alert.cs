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

using Newtonsoft.Json;

namespace Philter.Model
{
    public class Alert
    {
        [JsonProperty("id")]
        public string id { get; set; }
        
        [JsonProperty("filterProfile")]
        public string filterProfile { get; set; }
        
        [JsonProperty("strategyId")]
        public string strategyId { get; set; }
        
        [JsonProperty("context")]
        public string context { get; set; }
        
        [JsonProperty("documentId")]
        public string documentId { get; set; }
        
        [JsonProperty("filterType")]
        public string filterType { get; set; }
        
        [JsonProperty("date")]
        public string date { get; set; }
    }
}