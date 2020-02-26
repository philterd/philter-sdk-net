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
    /// <summary>
    /// A span of text identified as sensitive information.
    /// </summary>
    public class Span
    {
        /// <summary>
        /// The character index of the start of the span.
        /// </summary>
        [JsonProperty("characterStart")]
        public int CharacterStart { get; set; }

        /// <summary>
        /// The character index of the end of the span.
        /// </summary>
        [JsonProperty("characterEnd")]
        public int CharacterEnd { get; set; }

        /// <summary>
        /// The type of sensitive information.
        /// </summary>
        [JsonProperty("filterType")]
        public string FilterType { get; set; }
        
        /// <summary>
        /// The text of the span.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }
        
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
        /// The confidence of the span.
        /// </summary>
        [JsonProperty("confidence")]
        public double Confidence { get; set; }

        /// <summary>
        /// The value used to replace the span's text.
        /// </summary>
        [JsonProperty("replacement")]
        public string Replacement { get; set; }
    }
}
