using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DynCmd
{
    /// <summary>
    /// Provides methods for splitting input into arguments.
    /// </summary>
    public class ArgumentDelimiter
    {
        /// <summary>
        /// Represents a set of delimiters
        /// </summary>
        public class DelimiterSet : IReadOnlyCollection<char>, ISet<char>, IDeserializationCallback, ISerializable
        {
            private readonly ArgumentDelimiter _argumentDelimiter;
            private readonly HashSet<char> _hashSet = new HashSet<char>();

            /// <inheritdoc />
            public int Count => _hashSet.Count;

            /// <inheritdoc />
            public bool IsReadOnly => false;

            internal DelimiterSet(ArgumentDelimiter argumentDelimiter)
            {
                _argumentDelimiter = argumentDelimiter;
            }

            #region CRUD

            /// <inheritdoc />
            public bool Add(char item)
            {
                if (_argumentDelimiter.QuotePairs.ContainsKey(item) || _argumentDelimiter.QuotePairs.ContainsValue(item))
                {
                    throw new ArgumentException($"{nameof(item)} is already part of a quote pair", nameof(item));
                }

                return _hashSet.Add(item);
            }

            /// <inheritdoc />
            void ICollection<char>.Add(char item)
            {
                if (_argumentDelimiter.QuotePairs.ContainsKey(item) || _argumentDelimiter.QuotePairs.ContainsValue(item))
                {
                    throw new ArgumentException($"{nameof(item)} is already part of a quote pair", nameof(item));
                }

                _hashSet.Add(item);
            }

            /// <inheritdoc />
            public bool Contains(char item)
            {
                return _hashSet.Contains(item);
            }

            /// <inheritdoc />
            public bool Remove(char item)
            {
                return _hashSet.Remove(item);
            }

            /// <inheritdoc />
            public void Clear()
            {
                _hashSet.Clear();
            }

            #endregion

            #region Set Operations

            /// <inheritdoc />
            public void ExceptWith(IEnumerable<char> other)
            {
                _hashSet.ExceptWith(other);
            }

            /// <inheritdoc />
            public void IntersectWith(IEnumerable<char> other)
            {
                _hashSet.IntersectWith(other);
            }

            /// <inheritdoc />
            public bool IsProperSubsetOf(IEnumerable<char> other)
            {
                return _hashSet.IsProperSubsetOf(other);
            }

            /// <inheritdoc />
            public bool IsProperSupersetOf(IEnumerable<char> other)
            {
                return _hashSet.IsProperSupersetOf(other);
            }

            /// <inheritdoc />
            public bool IsSubsetOf(IEnumerable<char> other)
            {
                return _hashSet.IsSubsetOf(other);
            }

            /// <inheritdoc />
            public bool IsSupersetOf(IEnumerable<char> other)
            {
                return _hashSet.IsSupersetOf(other);
            }

            /// <inheritdoc />
            public bool Overlaps(IEnumerable<char> other)
            {
                return _hashSet.Overlaps(other);
            }

            /// <inheritdoc />
            public bool SetEquals(IEnumerable<char> other)
            {
                return _hashSet.SetEquals(other);
            }

            /// <inheritdoc />
            public void SymmetricExceptWith(IEnumerable<char> other)
            {
                _hashSet.SymmetricExceptWith(other);
            }

            /// <inheritdoc />
            public void UnionWith(IEnumerable<char> other)
            {
                _hashSet.UnionWith(other);
            }

            #endregion

            /// <inheritdoc />
            public void CopyTo(char[] array, int arrayIndex)
            {
                _hashSet.CopyTo(array, arrayIndex);
            }

            /// <inheritdoc />
            public IEnumerator<char> GetEnumerator()
            {
                return _hashSet.GetEnumerator();
            }

            /// <inheritdoc />
            IEnumerator IEnumerable.GetEnumerator()
            {
                return _hashSet.GetEnumerator();
            }

            /// <inheritdoc />
            public void OnDeserialization(object sender)
            {
                _hashSet.OnDeserialization(sender);
            }

            /// <inheritdoc />
            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                _hashSet.GetObjectData(info, context);
            }
        }

        /// <summary>
        /// Represents a dictionary of quotes
        /// </summary>
        public class QuoteDictionary : IDictionary<char, char>, IReadOnlyCollection<KeyValuePair<char, char>>, IReadOnlyDictionary<char, char>, ICollection, IDictionary, IDeserializationCallback, ISerializable
        {
            private readonly ArgumentDelimiter _argumentDelimiter;
            private readonly Dictionary<char, char> _quotePairs = new Dictionary<char, char>();

            /// <inheritdoc />
            public char this[char key]
            {
                get => _quotePairs[key];
                set => _quotePairs[key] = value;
            }

            /// <inheritdoc />
            object IDictionary.this[object key] 
            {
                get => ((IDictionary)_quotePairs)[key];
                set => ((IDictionary)_quotePairs)[key] = value;
            }

            /// <inheritdoc />
            public ICollection<char> Keys => _quotePairs.Keys;

            /// <inheritdoc />
            ICollection IDictionary.Keys => _quotePairs.Keys;

            /// <inheritdoc />
            IEnumerable<char> IReadOnlyDictionary<char, char>.Keys => _quotePairs.Values;

            /// <inheritdoc />
            public ICollection<char> Values => _quotePairs.Values;

            /// <inheritdoc />
            ICollection IDictionary.Values => _quotePairs.Values;

            /// <inheritdoc />
            IEnumerable<char> IReadOnlyDictionary<char, char>.Values => _quotePairs.Values;

            /// <inheritdoc />
            public int Count => _quotePairs.Count;

            /// <inheritdoc />
            public bool IsReadOnly => false;

            /// <inheritdoc />
            public bool IsSynchronized => false;

            /// <inheritdoc />
            public object SyncRoot => this;

            /// <inheritdoc />
            public bool IsFixedSize => false;

            internal QuoteDictionary(ArgumentDelimiter argumentDelimiter)
            {
                _argumentDelimiter = argumentDelimiter;
            }

            #region CRUD

            /// <inheritdoc />
            public void Add(char key, char value)
            {
                if (_argumentDelimiter.Delimiters.Contains(key))
                {
                    throw new ArgumentException($"{nameof(key)} is already a delimiter", nameof(key));
                }
                if (ContainsValue(key))
                {
                    throw new ArgumentException($"{nameof(key)} is already an end quote", nameof(key));
                }
                if (_argumentDelimiter.Delimiters.Contains(value))
                {
                    throw new ArgumentException($"{nameof(value)} is already a delimiter", nameof(value));
                }
                if (ContainsValue(value))
                {
                    throw new ArgumentException($"{nameof(value)} is already an end quote", nameof(value));
                }

                _quotePairs.Add(key, value);
            }

            /// <inheritdoc />
            void ICollection<KeyValuePair<char, char>>.Add(KeyValuePair<char, char> item)
            {
                if (_argumentDelimiter.Delimiters.Contains(item.Key))
                {
                    var paramName = $"{nameof(item)}.{nameof(item.Key)}";
                    throw new ArgumentException($"{paramName} is already a delimiter", paramName);
                }
                if (ContainsValue(item.Key))
                {
                    var paramName = $"{nameof(item)}.{nameof(item.Key)}";
                    throw new ArgumentException($"{paramName} is already an end quote", paramName);
                }
                if (_argumentDelimiter.Delimiters.Contains(item.Value))
                {
                    var paramName = $"{nameof(item)}.{nameof(item.Value)}";
                    throw new ArgumentException($"{paramName} is already a delimiter", paramName);
                }
                if (ContainsValue(item.Value))
                {
                    var paramName = $"{nameof(item)}.{nameof(item.Value)}";
                    throw new ArgumentException($"{paramName} is already an end quote", paramName);
                }

                _quotePairs.Add(item.Key, item.Value);
            }

            /// <inheritdoc />
            void IDictionary.Add(object key, object value)
            {
                ((IDictionary)_quotePairs).Add(key, value);
            }

            /// <inheritdoc />
            bool ICollection<KeyValuePair<char, char>>.Contains(KeyValuePair<char, char> item)
            {
                return ((ICollection<KeyValuePair<char, char>>)_quotePairs).Contains(item);
            }

            /// <inheritdoc />
            bool IDictionary.Contains(object key)
            {
                return ((IDictionary)_quotePairs).Contains(key);
            }

            /// <inheritdoc />
            public bool ContainsKey(char key)
            {
                return _quotePairs.ContainsKey(key);
            }

            /// <inheritdoc cref="Dictionary{TKey, TValue}"/>
            public bool ContainsValue(char value)
            {
                return _quotePairs.ContainsValue(value);
            }

            /// <inheritdoc />
            public bool TryGetValue(char key, out char value)
            {
                return _quotePairs.TryGetValue(key, out value);
            }

            /// <inheritdoc />
            public bool Remove(char key)
            {
                return _quotePairs.Remove(key);
            }

            /// <inheritdoc />
            bool ICollection<KeyValuePair<char, char>>.Remove(KeyValuePair<char, char> item)
            {
                return ((ICollection<KeyValuePair<char, char>>)_quotePairs).Remove(item);
            }

            /// <inheritdoc />
            void IDictionary.Remove(object key)
            {
                ((IDictionary)_quotePairs).Remove(key);
            }

            /// <inheritdoc />
            public void Clear()
            {
                _quotePairs.Clear();
            }

            #endregion

            /// <inheritdoc />
            void ICollection<KeyValuePair<char, char>>.CopyTo(KeyValuePair<char, char>[] array, int arrayIndex)
            {
                ((ICollection<KeyValuePair<char, char>>)_quotePairs).CopyTo(array, arrayIndex);
            }

            /// <inheritdoc />
            void ICollection.CopyTo(Array array, int index)
            {
                ((ICollection)_quotePairs).CopyTo(array, index);
            }

            /// <inheritdoc />
            public IEnumerator<KeyValuePair<char, char>> GetEnumerator()
            {
                return _quotePairs.GetEnumerator();
            }

            /// <inheritdoc />
            IEnumerator IEnumerable.GetEnumerator()
            {
                return _quotePairs.GetEnumerator();
            }

            /// <inheritdoc />
            IDictionaryEnumerator IDictionary.GetEnumerator()
            {
                return _quotePairs.GetEnumerator();
            }

            /// <inheritdoc />
            public void OnDeserialization(object sender)
            {
                _quotePairs.OnDeserialization(sender);
            }

            /// <inheritdoc />
            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                _quotePairs.GetObjectData(info, context);
            }
        }

        /// <summary>
        /// Represents a set of characters that mark the end of an argument and beginning of a new one.
        /// </summary>
        public DelimiterSet Delimiters { get; }

        /// <summary>
        /// Represents a collection of start literal-end literal pairs.
        /// Start/end-literals mark the beginning/ending of literals.
        /// <see cref="Delimiters"/> and other start-literals are ignored in literals.
        /// </summary>
        public QuoteDictionary QuotePairs { get; }

        public ArgumentDelimiter()
        {
            Delimiters = new DelimiterSet(this);
            QuotePairs = new QuoteDictionary(this);
        }

        /// <summary>
        /// Splits the given <paramref name="input"/> into arguments using <see cref="Delimiters"/> and <see cref="QuotePairs"/> as guides.
        /// </summary>
        /// <param name="input">The input to split</param>
        /// <returns>An array of arguments split using <see cref="Delimiters"/> and <see cref="QuotePairs"/>, and with both omitted</returns>
        public string[] Split(string input)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            return SplitInternal(input, 0, input.Length);
        }

        /// <summary>
        /// Splits the given <paramref name="input"/> into arguments,
        /// using <see cref="Delimiters"/> and <see cref="QuotePairs"/> as guides,
        /// starting with character located at <paramref name="index"/> position, and processes up to <paramref name="length"/> characters
        /// </summary>
        /// <param name="input">The input to split</param>
        /// <param name="index">The index in the input to start at</param>
        /// <param name="length">The length of the substring to split</param>
        /// <returns>An array of arguments split using <see cref="Delimiters"/> and <see cref="QuotePairs"/>, and with both omitted</returns>
        public string[] SplitAt(string input, int index, int length)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            if (index < 0 || index >= input.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (length < 0 || index + length > input.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            return SplitInternal(input, index, index + length);
        }

        /// <summary>
        /// Splits the given <paramref name="input"/> into arguments,
        /// using <see cref="Delimiters"/> and <see cref="QuotePairs"/> as guides,
        /// starting with character located at <paramref name="index"/> position, and ending with the character at <paramref name="endIndex"/> position
        /// </summary>
        /// <param name="input">The input to split</param>
        /// <param name="index">The index in the input to start at</param>
        /// <param name="endIndex">The index in the input to stop at</param>
        /// <returns>An array of arguments split using <see cref="Delimiters"/> and <see cref="QuotePairs"/>, and with both omitted</returns>
        public string[] SplitAtTo(string input, int index, int endIndex)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            if (index < 0 || index >= input.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (endIndex < 0 || endIndex > input.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(endIndex));
            }

            return SplitInternal(input, index, endIndex);
        }

        /// <summary>
        /// Splits the given <paramref name="input"/> into arguments,
        /// using <see cref="Delimiters"/> and <see cref="QuotePairs"/> as guides,
        /// starting with character located at <paramref name="index"/> position, and ending with the character at <paramref name="endIndex"/> position
        /// </summary>
        /// <param name="input">The input to split</param>
        /// <param name="index">The index in the input to start at</param>
        /// <param name="endIndex">The index in the input to stop at</param>
        /// <returns>An array of arguments split using <see cref="Delimiters"/> and <see cref="QuotePairs"/>, and with both omitted</returns>
        private string[] SplitInternal(string input, int index, int endIndex)
        {
            var result = new string[endIndex - index];
            // Index of the first available slot in result
            var resultIndex = 0;
            // End index of the previous argument in input
            var lastResultIndex = 0;

            var inQuote = false;
            var endQuote = '\0';
            for (var i = index; i < endIndex; i++)
            {
                var currentChar = input[i];

                if (inQuote)
                {
                    if (currentChar == endQuote)
                    {
                        goto SplitArgument;
                    }
                }
                else
                {
                    if (Delimiters.Contains(currentChar))
                    {
                        goto SplitArgument;
                    }

                    if (QuotePairs.TryGetValue(currentChar, out var eQuote))
                    {
                        inQuote = true;
                        endQuote = eQuote;
                        goto SplitArgument;
                    }
                }

                continue;

            SplitArgument:
                result[resultIndex] = input.Substring(index + lastResultIndex + 1, i - lastResultIndex - 1);
                lastResultIndex = i;
                resultIndex++;
                continue;
            }

            return result;
        }
    }
}
