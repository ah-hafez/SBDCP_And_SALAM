/* 
 * Copyright (C) 2014 Mehdi El Gueddari
 * http://mehdi.me
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 */
using System.Data.Entity;
using MCS.Framework.Persistence;

namespace MCS.Common.TransactionContext
{
    /// <summary>
    /// Convenience methods to retrieve ambient TransactionContext instances. 
    /// </summary>
    public interface IAmbienTTransactionContextLocator
    {
        /// <summary>
        /// If called within the scope of a transactionContextScope, gets or creates 
        /// the ambient TransactionContext instance for the provided TransactionContext type. 
        /// 
        /// Otherwise returns null. 
        /// </summary>
        TTransactionContext Get<TTransactionContext>() where TTransactionContext : DbContextBase;
    }
}
