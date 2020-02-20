#pragma warning disable 1587
/**
 * Copyright 2019-2020 Wingify Software Pvt. Ltd.
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

namespace VWOSdk
{
    internal class RangeBucket<T>
    {
        private readonly double _maxWeight = 100;
        private double _currentWeight = 0;
        private static readonly double _seed = 1;
        private bool _resetRange;
        private List<KeyValuePair<WeightRange, T>> _buckets;

        /// <summary>
        /// Initialise a RangeBucket Instance.
        /// </summary>
        /// <param name="continuosRange">This determines the StartRange of an element. If false, then every element range will be zero - elementWeight.</param>
        public RangeBucket(bool continuosRange = true)
        {
            this._resetRange = !continuosRange;
            this._buckets = new List<KeyValuePair<WeightRange, T>>();
        }

        /// <summary>
        /// Initialise a RangeBucket Instance.
        /// </summary>
        /// <param name="maxWeight"></param>
        /// <param name="continuosRange">This determines the StartRange of an element. If false, then every element range will be zero - elementWeight.</param>
        public RangeBucket(double maxWeight, bool continuosRange = true) : this(continuosRange)
        {
            this._maxWeight = maxWeight;
        }

        /// <summary>
        /// Find an element whose range qualifies the given hash value, returns the first match.
        /// </summary>
        /// <param name="hash">Value to be checked if it falls under any element range.</param>
        /// <returns></returns>
        public T Find(double hash)
        {
            foreach (var pair in this._buckets)
            {
                if (pair.Key.IsInRange(hash))
                {
                    return pair.Value;
                }
            }
            return default(T);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="SearchType">Type of searchValue.</typeparam>
        /// <param name="hash">Value to be checked if it falls under any element range.</param>
        /// <param name="searchValue">Look up value in bucket.</param>
        /// <param name="searchValueSelector">Func of T Type which returns a value to be compared with passed searchValue.</param>
        /// <returns></returns>
        //internal T Find<SearchType>(double hash, SearchType searchKey, Func<T, SearchType> searchKeySelector)
        //{
        //    foreach (var pair in this._buckets)
        //    {
        //        if (pair.Key.IsInRange(hash) && searchKey.Equals(searchKeySelector(pair.Value)))
        //        {
        //            return pair.Value;
        //        }
        //    }
        //    return default(T);
        //}

        /// <summary>
        /// Add elements to bucket and out start and end range of added elements.
        /// </summary>
        /// <param name="weight">Weight of provided value within the bucket.</param>
        /// <param name="item">Value to be added.</param>
        /// <param name="start">Out Start range of item.</param>
        /// <param name="end">Out End range of item.</param>
        internal bool Add(double weight, T item, out double start, out double end)
        {
            if (_resetRange)
                this._currentWeight = 0;

            weight *= this._maxWeight / 100;

            end = this._currentWeight + Math.Floor(weight);
            start = this._currentWeight + _seed;

            var weightRange = new WeightRange(start, end);
            this._buckets.Add(new KeyValuePair<WeightRange, T>(weightRange, item));

            this._currentWeight = end;
            return true;
        }

        /// <summary>
        /// Find an element within bucket matching the given criteria, returns the first match.
        /// </summary>
        /// <typeparam name="SearchType">Type of searchValue.</typeparam>
        /// <param name="searchValue">Look up value in bucket.</param>
        /// <param name="searchValueSelector">Func of T Type which returns a value to be compared with passed searchValue.</param>
        /// <returns>
        /// Matching element. If no matched element found then null.
        /// </returns>
        internal T Find<SearchType>(SearchType searchValue, Func<T, SearchType> searchValueSelector)
        {
            foreach (var pair in this._buckets)
            {
                if (searchValue.Equals(searchValueSelector(pair.Value)))
                    return pair.Value;
            }
            return default(T);
        }

        /// <summary>
        /// Add elements to bucket.
        /// </summary>
        /// <param name="weight">Weight of provided value within the bucket.</param>
        /// <param name="item">Value to be added.</param>
        /// <returns></returns>
        internal bool Add(double weight, T item)
        {
            return Add(weight, item, out double start, out double end);
        }
    }
}
