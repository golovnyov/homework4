namespace Homework4
{
    public interface ITransactionManager
    {
        bool BeginTransaction();

        bool CommitTransaction();

        bool RollbackTransaction();
    }
}
