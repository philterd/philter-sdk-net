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
    /// <summary>
    /// Response to an explain API request.
    /// </summary>
    public class ExplainResponse
    {
        /// <summary>
        /// The filtered text.
        /// </summary>
        [JsonProperty("filteredText")]
        public string FilteredText { get; set; }

        /// <summary>
        /// The context.
        /// </summary>
        [JsonProperty("context")]
        public string Context { get; set; }

        /// <summary>
        /// The document ID.
        /// </summary>
        [JsonProperty("documentId")]
        public string DocumentId { get; set; }

        /// <summary>
        /// The explanation.
        /// </summary>
        [JsonProperty("explanation")]
        public Explanation Explanation { get; set; }
    }
}
