using Core.Data.Derived.EntityFramework;
using SaveCustomerService.Data.Model.Infrastructure.Users.User.Entity;
using SaveCustomerService.Data.Repository.Infrastructure.Users.User;

namespace SaveCustomerService.Data.Repository.Derived
{
    public class UserRepository : BaseEntityRepository<KolinGazContext, UserEntity>, IUserRepository
    {
        public bool Delete(int Id)
        {
            var user = _dbSet.Find(Id);
            _context.Remove(user);
            return _context.SaveChanges() > 0;
        }
    }
}
