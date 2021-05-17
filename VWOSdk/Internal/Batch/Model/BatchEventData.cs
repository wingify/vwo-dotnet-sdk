#pragma warning disable 1587
/**
 * Copyright 2019-2021 Wingify Software Pvt. Ltd.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#pragma warning restore 1587

namespace VWOSdk
{
    /// <summary>
    /// EVENT TYPES ENUM , TRACK USER=1,TRACK GOAL=2,PUSH=3.
    /// </summary>
    internal enum EVENT_TYPES
    {
        TRACK_USER = 1,
        TRACK_GOAL = 2,
        PUSH = 3

    }
    /// <summary>
    /// Event Batching Data.
    /// </summary>
    public class BatchEventData
    {
        internal int? eventsPerRequest;
        internal int? requestTimeInterval;

        internal IFlushInterface flushCallback;

        /// <summary>
        ///Events Per Request . Default 100
        /// </summary>
        public int? EventsPerRequest
        {
            get
            {
                return eventsPerRequest;
            }
            set
            {
                this.eventsPerRequest = value;
            }
        }

        /// <summary>
        ///Request Time Interval . Default 600 seconds
        /// </summary>
        public int? RequestTimeInterval
        {
            get
            {
                return requestTimeInterval;
            }
            set
            {
                this.requestTimeInterval = value;
            }
        }

        /// <summary>
        /// Flush Callback Method.
        /// </summary>
        public IFlushInterface FlushCallback
        {
            get
            {
                return flushCallback;
            }
            set
            {
                this.flushCallback = value;
            }
        }

        internal EVENT_TYPES _Eventtype { get; private set; }

    }
}
