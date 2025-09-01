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
    /// Factory for TransactionContext-derived classes that don't expose 
    /// a default constructor.
    /// </summary>
    /// <remarks>
	/// If your TransactionContext-derived classes have a default constructor, 
	/// you can ignore this factory. transactionContextScope will take care of
	/// instanciating your TransactionContext class with Activator.CreateInstance() 
	/// when needed.
	/// 
	/// If your TransactionContext-derived classes don't expose a default constructor
	/// however, you must impement this interface and provide it to transactionContextScope
	/// so that it can create instances of your TransactionContexts.
	/// 
	/// A typical situation where this would be needed is in the case of your TransactionContext-derived 
	/// class having a dependency on some other component in your application. For example, 
	/// some data in your database may be encrypted and you might want your TransactionContext-derived
	/// class to automatically decrypt this data on entity materialization. It would therefore 
	/// have a mandatory dependency on an IDataDecryptor component that knows how to do that. 
	/// In that case, you'll want to implement this interface and pass it to the transactionContextScope
	/// you're creating so that transactionContextScope is able to create your TransactionContext instances correctly. 
    /// </remarks>
    public interface ITransactionContextFactory
    {
		TTransactionContext CreateTransactionContext<TTransactionContext>(int? tenantId) where TTransactionContext : DbContextBase;
    }
}
