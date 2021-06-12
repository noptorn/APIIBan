using APIIBan.Model;

namespace APIIBan.Services
{
    public abstract class IAccountServiceBase
    {
        public abstract AccountFileDB NewAccount(AccountResource resource);
    }
}