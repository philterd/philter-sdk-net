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

namespace Philter.Model
{
    public class FilterResponse
    { 

        private string _filteredText;
        private string _context;
        private string _documentId;
        private Explanation _explanation;

        public string filteredText
        {
            get
            {
                return this._filteredText;
            }
            set
            {
                this._filteredText = value;
            }
        }

        public string context
        {
            get
            {
                return this._context;
            }
            set
            {
                this._context = value;
            }
        }

        public string documentId
        {
            get
            {
                return this._documentId;
            }
            set
            {
                this._documentId = value;
            }
        }

        public Explanation explanation
        {
            get
            {
                return this._explanation;
            }
            set
            {
                this._explanation = value;
            }
        }

    }
}
