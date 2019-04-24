using System;

namespace Homework4
{
    internal interface ITransactionManager
    {
        bool BeginTransaction();

        bool CommitTransaction(Action commitAction);

        bool RollbackTransaction(Action rollbackAction);
    }
}
