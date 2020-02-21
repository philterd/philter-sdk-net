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
    public class Span
    { 

        private int _characterStart;
        private int _characterEnd;
        private string _context;
        private string _documentId;
        private double _confidence;
        private string _replacement;
        private FilterType _filterType;

        public int characterStart
        {
            get
            {
                return this._characterStart;
            }
            set
            {
                this._characterStart = value;
            }
        }

        public int characterEnd
        {
            get
            {
                return this._characterEnd;
            }
            set
            {
                this._characterEnd = value;
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

        public double confidence
        {
            get
            {
                return this._confidence;
            }
            set
            {
                this._confidence = value;
            }
        }

        public string replacement
        {
            get
            {
                return this._replacement;
            }
            set
            {
                this._replacement = value;
            }
        }

        public FilterType filterType
        {
            get
            {
                return this._filterType;
            }
            set
            {
                this._filterType = value;
            }
        }

    }
}
