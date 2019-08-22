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