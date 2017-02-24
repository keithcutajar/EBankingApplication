using Common;

namespace DataAccess
{
    public class ConnectionClass
    {
        public BankSystemDBEntities Entity { get; set; }

        public ConnectionClass()
        {
            Entity = new BankSystemDBEntities();
        }
    }
}
