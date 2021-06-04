using APIIBan.Model;

namespace APIIBan.Services
{
    public abstract class IAccountServiceBase
    {
        public abstract Account NewAccount(AccountResource resource);
    }
}