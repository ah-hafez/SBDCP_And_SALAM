/* 
 * Copyright (C) 2014 Mehdi El Gueddari
 * http://mehdi.me
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 */
using System;

namespace MCS.Common.TransactionContext
{
    /// <summary>
    /// A read-only transactionContextScope. Refer to the comments for ITransactionContextScope
    /// for more details.
    /// </summary>
    public interface ITransactionContextReadOnlyScope : IDisposable
    {
        /// <summary>
        /// The TransactionContext instances that this transactionContextScope manages.
        /// </summary>
        ITransactionContextCollection TransactionContexts { get; }
    }
}