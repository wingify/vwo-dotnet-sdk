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

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace VWOSdk
{
    /// <summary>
    /// Used for Event Batching .
    /// </summary>
    public class BatchEventQueue
    {
        internal const int MAX_EVENTS_PER_REQUEST = 5000;
        internal Queue<IDictionary<string, dynamic>> batchQueue = new Queue<IDictionary<string, dynamic>>();
        private static readonly Dictionary<string, int> queueMetaData = new Dictionary<string, int>();
        internal int requestTimeInterval = 600; //default:- 10 * 60(secs) = 600 secs i.e. 10 minutes
        internal int eventsPerRequest = 100; //default
        internal IFlushInterface flushCallback;
        internal Timer timer;
        private readonly int accountId;
        private readonly string apikey;
        private bool isDevelopmentMode;
        private bool isBatchProcessing = false;
        private static readonly string file = typeof(BatchEventQueue).FullName;

        /// <summary>
        /// Init variables in BatchEventQueue.
        /// </summary>
        /// <param name="batchEvents">BatchEventsData instance</param>
        /// <param name="apikey">VWO application apikey</param>
        /// <param name="accountId">VWO application accountId</param>
        /// <param name="isDevelopmentMode">isDevelopmentMode Boolean value specifying development mode is on ir off</param>
        internal BatchEventQueue(BatchEventData batchEvents, string apikey, int accountId, bool isDevelopmentMode)
        {
            if (batchEvents != null)
            {
                if (batchEvents.RequestTimeInterval != null)
                {
                    if (batchEvents.RequestTimeInterval > 1)
                    {
                        this.requestTimeInterval = (int)batchEvents.RequestTimeInterval;
                    }
                    else
                    {
                        LogDebugMessage.RequestTimeIntervalOutOfBound(file, 1, requestTimeInterval);

                    }
                }
                else
                {
                    LogDebugMessage.RequestTimeIntervalOutOfBound(file, 1, requestTimeInterval);

                }
                if (batchEvents.EventsPerRequest != null)
                {

                    if (batchEvents.EventsPerRequest > 0 && batchEvents.EventsPerRequest <= MAX_EVENTS_PER_REQUEST)
                    {
                        this.eventsPerRequest = Math.Min((int)batchEvents.EventsPerRequest, MAX_EVENTS_PER_REQUEST);
                    }
                    else
                    {
                        LogDebugMessage.EventsPerRequestOutOfBound(file, 1, MAX_EVENTS_PER_REQUEST, eventsPerRequest);
                    }
                }
                else
                {
                    LogDebugMessage.EventsPerRequestOutOfBound(file, 1, MAX_EVENTS_PER_REQUEST, eventsPerRequest);

                }
                if (batchEvents.FlushCallback != null)
                {
                    this.flushCallback = batchEvents.FlushCallback;
                }

            }
            this.accountId = accountId;
            this.isDevelopmentMode = isDevelopmentMode;
            this.apikey = apikey;
        }
        /// <summary>
        /// Insert the event in the queue and flush if the queue is full.
        /// </summary>
        /// <param name="eventData"> event Map containing configs of the event</param>
        ///
        public void addInQueue(IDictionary<string, dynamic> eventData)
        {
            if (isDevelopmentMode)
            {
                return;
            }

            batchQueue.Enqueue(eventData);

            if (eventData.ContainsKey("eT"))
            {
                int eT = (int)eventData["eT"];
                addEventCount(eT);
                LogInfoMessage.ImpressionSuccessQueue(file);
            }


            if (timer == null)
            {
                createNewBatchTimer();
            }
            if (eventsPerRequest == batchQueue.Count)
            {
                flush(false);
            }
        }
        /// <summary>
        /// Flush the queue, clear timer and send POST network call to VWO servers.
        /// </summary>
        /// <param name="manual">manual Boolean specifying flush is triggered manual or not.</param>
        /// <returns>Boolean value specifying flush was successful or not.</returns>
        public bool flush(bool manual)
        {
            var batchMetadata= HttpRequestBuilder.GetJsonString(this.batchQueue);
            if (batchQueue.Count == 0)
            {
                LogDebugMessage.EventQueueEmpty(file);
            }
            if (manual)
            {
                if (batchQueue.Count > 0)
                {
                    LogDebugMessage.BeforeFlushing(file, "manually", batchQueue.Count.ToString(), accountId.ToString(), "Timer will be cleared and registered again", batchMetadata);
                    Task<bool> response = sendPostCall();
                    LogDebugMessage.AfterFlushing(file, "manually", batchQueue.Count.ToString(), batchMetadata);
                    disposeData();
                    return true;
                }
                clearRequestTimer();
                return true;
            }
            else
            {

                if (batchQueue.Count > 0 && !isBatchProcessing)
                {
                    isBatchProcessing = true;

                    LogDebugMessage.BeforeFlushing(file, "", batchQueue.Count.ToString(), accountId.ToString(), "", batchMetadata);
                    Task<bool> response = sendPostCall();
                    LogDebugMessage.AfterFlushing(file, "", batchQueue.Count.ToString(), batchMetadata);
                    disposeData();
                    return true;
                }
                clearRequestTimer();
                return true;
            }
        }
        /// <summary>
        /// Initialize a new Timer.
        /// </summary>
        private void createNewBatchTimer()
        {
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = requestTimeInterval * 1000L;
            timer.Enabled = true;
        }
        /// <summary>
        /// Specify what you want to happen when the Elapsed event is raised.
        /// </summary>
        /// <param name="source">Timer object</param>
        /// <param name="e">Timer Elapsed EventArgs</param>
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            flush(false);
        }
        /// <summary>
        ///  CLears the timer.
        /// </summary>
        private void clearRequestTimer()
        {
            if (timer != null)
            {
                timer.Stop();
                timer = null;
                isBatchProcessing = false;
            }
        }
        /// <summary>
        /// Send Post network call to VWO servers.
        /// </summary>
        /// <returns>Boolean value specifying flush was successful or not.</returns>
        private async Task<bool> sendPostCall()
        {
            string PayLoad = HttpRequestBuilder.GetJsonString(this.batchQueue);
            var ApiRequest = ServerSideVerb.EventBatching(this.accountId, this.isDevelopmentMode, this.apikey);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("Authorization", this.apikey);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var data = new StringContent(PayLoad, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(ApiRequest.Uri, data);
                response.EnsureSuccessStatusCode();
                if (response.StatusCode == System.Net.HttpStatusCode.OK && response.StatusCode < System.Net.HttpStatusCode.Ambiguous)
                {

                    if (flushCallback != null)
                    {
                        flushCallback.onFlush(null, PayLoad);
                    }
                    LogInfoMessage.ImpressionSuccess(file, ApiRequest.logUri?.ToString());
                    return true;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.RequestEntityTooLarge)
                {
                    if (flushCallback != null)
                    {
                        flushCallback.onFlush("Payload size too large", PayLoad);
                    }
                    LogDebugMessage.BatchEventLimitExceeded(file, ApiRequest.logUri?.ToString(), this.accountId.ToString(), eventsPerRequest.ToString());
                    LogErrorMessage.ImpressionFailed(file, ApiRequest.logUri?.ToString());

                    return false;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    if (flushCallback != null)
                    {
                        flushCallback.onFlush("Account id not found, no request app id found, or invalid API key", PayLoad);
                    }
                    LogErrorMessage.BulkNotProcessed(file);
                    LogErrorMessage.ImpressionFailed(file, ApiRequest.logUri?.ToString());
                    return false;
                }
                else
                {
                    LogErrorMessage.BulkNotProcessed(file);
                    LogErrorMessage.ImpressionFailed(file, ApiRequest.logUri?.ToString());
                    if (flushCallback != null)
                    {
                        flushCallback.onFlush("Invalid call", PayLoad);
                    }

                    return false;
                }

            }
            catch (HttpRequestException ex)
            {
                LogErrorMessage.BulkNotProcessed(file);
                LogErrorMessage.UnableToDisplayHttpRequest(file, ex.StackTrace);
                if (flushCallback != null)
                {
                    flushCallback.onFlush("HttpRequest Exception", PayLoad);
                }
                return false;
            }
        }
        /// <summary>
        /// Increase the count of a particular event.
        /// </summary>
        /// <param name="eventType">Type of the event .Track=1 , Goal=2 , Push=3 </param>
        private void addEventCount(int eventType)
        {
            if (eventType == (int)EVENT_TYPES.TRACK_USER)
            {
                if (queueMetaData.ContainsKey("visitorEvents"))
                {
                    int visitorEvents = queueMetaData["visitorEvents"];
                    queueMetaData["visitorEvents"] = visitorEvents + 1;
                }
                else
                {
                    queueMetaData["visitorEvents"] = 1;
                }
            }
            else if (eventType == (int)EVENT_TYPES.TRACK_GOAL)
            {
                if (queueMetaData.ContainsKey("goalEvents"))
                {
                    int goalEvents = queueMetaData["goalEvents"];
                    queueMetaData["goalEvents"] = goalEvents + 1;

                }
                else
                {
                    queueMetaData["goalEvents"] = 1;
                }
            }
            else if (eventType == (int)EVENT_TYPES.PUSH)
            {
                if (queueMetaData.ContainsKey("pushEvents"))
                {
                    int pushEvents = queueMetaData["pushEvents"];
                    queueMetaData["pushEvents"] = pushEvents + 1;

                }
                else
                {
                    queueMetaData["pushEvents"] = 1;
                }
            }
        }

        /// <summary>
        /// clear the queues and reset the timer.
        /// </summary>
        private void disposeData()
        {
            batchQueue.Clear();
            queueMetaData.Clear();
            clearRequestTimer();
        }
        /// <summary>
        /// Used for Unit Test
        /// </summary>
        /// <returns>
        /// Queue count.
        /// </returns>
        public int BatchQueueCount()
        {
            return this.batchQueue.Count;
        }
    }
}
