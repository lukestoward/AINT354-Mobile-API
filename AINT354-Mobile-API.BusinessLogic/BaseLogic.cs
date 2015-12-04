using System.Threading.Tasks;
using GamingSessionApp.DataAccess;

namespace AINT354_Mobile_API.BusinessLogic
{
    public class BaseLogic
    {
        protected readonly UnitOfWork UoW = new UnitOfWork();

        protected void SaveChanges()
        {
            UoW.Save();
        }

        protected async Task<bool> SaveChangesAsync()
        {
            return await UoW.SaveAsync();
        }

        public void Dispose()
        {
            UoW.Dispose();
        }
    }
}