/* 
 * Copyright (C) 2014 Mehdi El Gueddari
 * http://mehdi.me
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 */
using System;
using System.Data.Entity;
using MCS.Framework.Persistence;

namespace MCS.Common.TransactionContext
{
    /// <summary>
    /// Maintains a list of lazily-created TransactionContext instances.
    /// </summary>
    public interface ITransactionContextCollection : IDisposable
   {
        /// <summary>
        /// Get or create a TransactionContext instance of the specified type. 
        /// </summary>
		TTransactionContext Get<TTransactionContext>(int? tenantId) where TTransactionContext : DbContextBase;
    }
}