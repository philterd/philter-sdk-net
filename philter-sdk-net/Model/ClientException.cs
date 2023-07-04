﻿/*******************************************************************************
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

namespace Philter.Model
{
    /// <summary>
    /// An exception thrown during the execution of a Philter API request.
    /// </summary>
    public class ClientException : Exception
    {
        /// <summary>
        /// Creates a new exception.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        public ClientException(string message) : base(message)
        {

        }

        /// <summary>
        /// Creates a new exception.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <param name="ex">The underlying exception.</param>
        public ClientException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}
