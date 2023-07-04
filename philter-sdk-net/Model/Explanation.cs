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

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Philter.Model
{
    /// <summary>
    /// An explanation of filtering.
    /// </summary>
    public class Explanation
    {
        /// <summary>
        /// Text spans identified as sensitive information.
        /// </summary>
        [JsonProperty("appliedSpans")]
        public List<Span> AppliedSpans { get; set; }

        /// <summary>
        /// Text spans identified but ignored due to conditions or other factors.
        /// </summary>
        [JsonProperty("ignoredSpans")]
        public List<Span> IgnoredSpans { get; set; }
    }
}
