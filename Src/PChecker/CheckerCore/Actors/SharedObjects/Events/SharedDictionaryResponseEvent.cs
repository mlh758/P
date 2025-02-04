﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using PChecker.Actors.Events;

namespace PChecker.Actors.SharedObjects.Events
{
    /// <summary>
    /// Event containing the value of a shared dictionary.
    /// </summary>
    internal class SharedDictionaryResponseEvent<T> : Event
    {
        /// <summary>
        /// Value.
        /// </summary>
        internal T Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharedDictionaryResponseEvent{T}"/> class.
        /// </summary>
        internal SharedDictionaryResponseEvent(T value)
        {
            Value = value;
        }
    }
}
