namespace Lib
{
    public interface IBorrower
    {
        void BorrowBook();
        void ReturnBook();
        void PayFineBook();
    }

    public abstract class Login
    {
        public abstract void ExecuteRoleActions();
    }
}
