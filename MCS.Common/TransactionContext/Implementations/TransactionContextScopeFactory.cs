/* 
 * Copyright (C) 2014 Mehdi El Gueddari
 * http://mehdi.me
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 */
using System;
using System.Data;

namespace MCS.Common.TransactionContext
{
    public class TransactionContextScopeFactory : ITransactionContextScopeFactory
    {
        private readonly ITransactionContextFactory _TransactionContextFactory;

        public TransactionContextScopeFactory()
        {
            _TransactionContextFactory = null;
        }

        public ITransactionContextScope Create(transactionContextScopeOption joiningOption = transactionContextScopeOption.JoinExisting, int? tenantId = -1)
        {
            return new TransactionContextScope(
                joiningOption: joiningOption,
                readOnly: false,
                isolationLevel: null,
                TransactionContextFactory: _TransactionContextFactory,
                tenantId: tenantId
                );
        }

        public ITransactionContextReadOnlyScope CreateReadOnly(transactionContextScopeOption joiningOption = transactionContextScopeOption.JoinExisting)
        {
            return new TransactionContextReadOnlyScope(
                joiningOption: joiningOption,
                isolationLevel: null,
                TransactionContextFactory: _TransactionContextFactory);
        }

        public ITransactionContextScope CreateWithTransaction(IsolationLevel isolationLevel)
        {
            return new TransactionContextScope(
                joiningOption: transactionContextScopeOption.ForceCreateNew,
                readOnly: false,
                isolationLevel: isolationLevel,
                TransactionContextFactory: _TransactionContextFactory);
        }

        public ITransactionContextReadOnlyScope CreateReadOnlyWithTransaction(IsolationLevel isolationLevel)
        {
            return new TransactionContextReadOnlyScope(
                joiningOption: transactionContextScopeOption.ForceCreateNew,
                isolationLevel: isolationLevel,
                TransactionContextFactory: _TransactionContextFactory);
        }

        public IDisposable SuppressAmbientContext()
        {
            return new AmbientContextSuppressor();
        }
    }
}