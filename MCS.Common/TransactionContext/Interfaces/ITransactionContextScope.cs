/* 
 * Copyright (C) 2014 Mehdi El Gueddari
 * http://mehdi.me
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 */
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace MCS.Common.TransactionContext
{
    /// <summary>
    /// Creates and manages the TransactionContext instances used by this code block. 
    /// 
    /// You typically use a transactionContextScope at the business logic service level. Each 
	/// business transaction (i.e. each service method) that uses Entity Framework must
    /// be wrapped in a transactionContextScope, ensuring that the same TransactionContext instances 
    /// are used throughout the business transaction and are committed or rolled 
    /// back atomically.
    /// 
    /// Think of it as TransactionScope but for managing TransactionContext instances instead 
    /// of database transactions. Just like a TransactionScope, a transactionContextScope is 
    /// ambient, can be nested and supports async execution flows.
    /// 
	/// And just like TransactionScope, it does not support parallel execution flows. 
	/// You therefore MUST suppress the ambient transactionContextScope before kicking off parallel 
	/// tasks or you will end up with multiple threads attempting to use the same TransactionContext
	/// instances (use ITransactionContextScopeFactory.SuppressAmbientContext() for this).
    /// 
    /// You can access the TransactionContext instances that this scopes manages via either:
    /// - its TransactionContexts property, or
    /// - an IAmbienTTransactionContextLocator
    /// 
    /// (you would typically use the later in the repository / query layer to allow your repository
    /// or query classes to access the ambient TransactionContext instances without giving them access to the actual
    /// transactionContextScope).
    /// 
    /// </summary>
    public interface ITransactionContextScope : IDisposable
    {
        /// <summary>
        /// Saves the changes in all the TransactionContext instances that were created within this scope.
        /// This method can only be called once per scope.
        /// </summary>
        int Commit();

        /// <summary>
        /// Saves the changes in all the TransactionContext instances that were created within this scope.
        /// This method can only be called once per scope.
        /// </summary>
        Task<int> CommitAsync();

        /// <summary>
        /// Saves the changes in all the TransactionContext instances that were created within this scope.
        /// This method can only be called once per scope.
        /// </summary>
        Task<int> CommitAsync(CancellationToken cancelToken);

        /// <summary>
        /// Reloads the provided persistent entities from the data store
        /// in the TransactionContext instances managed by the parent scope. 
        /// 
		/// If there is no parent scope (i.e. if this transactionContextScope
		/// if the top-level scope), does nothing.
        /// 
        /// This is useful when you have forced the creation of a new
        /// transactionContextScope and want to make sure that the parent scope
        /// (if any) is aware of the entities you've modified in the 
        /// inner scope.
        /// 
        /// (this is a pretty advanced feature that should be used 
        /// with parsimony). 
        /// </summary>
        void RefreshEntitiesInParentScope(IEnumerable entities);

		/// <summary>
		/// Reloads the provided persistent entities from the data store
		/// in the TransactionContext instances managed by the parent scope. 
		/// 
		/// If there is no parent scope (i.e. if this transactionContextScope
		/// if the top-level scope), does nothing.
		/// 
		/// This is useful when you have forced the creation of a new
		/// transactionContextScope and want to make sure that the parent scope
		/// (if any) is aware of the entities you've modified in the 
		/// inner scope.
		/// 
		/// (this is a pretty advanced feature that should be used 
		/// with parsimony). 
		/// </summary>
        Task RefreshEntitiesInParentScopeAsync(IEnumerable entities);

        /// <summary>
        /// The TransactionContext instances that this transactionContextScope manages. Don't call SaveChanges() on the TransactionContext themselves!
        /// Save the scope instead.
        /// </summary>
        ITransactionContextCollection TransactionContexts { get; }
    }
}
