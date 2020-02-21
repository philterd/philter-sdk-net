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

using System.Collections.Generic;

namespace Philter.Model
{
    public class Explanation
    {

        private List<Span> _appliedSpans;
        private List<Span> _identifiedSpans;

        public List<Span> appliedSpans
        {
            get
            {
                return this._appliedSpans;
            }
            set
            {
                this._appliedSpans = value;
            }
        }

        public List<Span> identifiedSpans
        {
            get
            {
                return this._identifiedSpans;
            }
            set
            {
                this._identifiedSpans = value;
            }
        }

    }
}
