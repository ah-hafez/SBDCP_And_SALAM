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
    public class AmbienTTransactionContextLocator : IAmbienTTransactionContextLocator
    {
        public TTransactionContext Get<TTransactionContext>() where TTransactionContext : DbContextBase
        {
            var ambienttransactionContextScope = TransactionContextScope.GetAmbientScope();
            return ambienttransactionContextScope == null ? null : ambienttransactionContextScope.TransactionContexts.Get<TTransactionContext>(ambienttransactionContextScope.tenantId);
        }
    }
}