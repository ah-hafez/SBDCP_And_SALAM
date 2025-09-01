/* 
 * Copyright (C) 2014 Mehdi El Gueddari
 * http://mehdi.me
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 */
using System.Data;

namespace MCS.Common.TransactionContext
{
    public class TransactionContextReadOnlyScope : ITransactionContextReadOnlyScope
    {
        private TransactionContextScope _internalScope;

        public ITransactionContextCollection TransactionContexts { get { return _internalScope.TransactionContexts; } }

        public TransactionContextReadOnlyScope(ITransactionContextFactory TransactionContextFactory = null)
            : this(joiningOption: transactionContextScopeOption.JoinExisting, isolationLevel: null, TransactionContextFactory: TransactionContextFactory)
        {}

        public TransactionContextReadOnlyScope(IsolationLevel isolationLevel, ITransactionContextFactory TransactionContextFactory = null)
            : this(joiningOption: transactionContextScopeOption.ForceCreateNew, isolationLevel: isolationLevel, TransactionContextFactory: TransactionContextFactory)
        { }

        public TransactionContextReadOnlyScope(transactionContextScopeOption joiningOption, IsolationLevel? isolationLevel, ITransactionContextFactory TransactionContextFactory = null)
        {
            _internalScope = new TransactionContextScope(joiningOption: joiningOption, readOnly: true, isolationLevel: isolationLevel, TransactionContextFactory: TransactionContextFactory);
        }

        public void Dispose()
        {
            _internalScope.Dispose();
        }
    }
}