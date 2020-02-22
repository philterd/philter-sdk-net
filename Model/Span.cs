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

namespace Philter.Model
{
    public class Span
    {
        [JsonProperty("characterStart")]
        public int characterStart { get; set; }

        [JsonProperty("characterEnd")]
        public int characterEnd { get; set; }

        [JsonProperty("context")]
        public string context { get; set; }

        [JsonProperty("documentId")]
        public string documentId { get; set; }

        [JsonProperty("confidence")]
        public double confidence { get; set; }

        [JsonProperty("replacement")]
        public string replacement { get; set; }

        [JsonProperty("filterType")]
        public FilterType filterType { get; set; }
    }
}
