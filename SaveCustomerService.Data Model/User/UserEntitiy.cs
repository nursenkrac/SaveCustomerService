using Core.Data.Model.Infrastructure;
using System;

namespace SaveCustomerService.Data.Infrastructure.User
{
    public class UserEntitiy : BaseEntity
    {
        public string National_Identifier { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string BirthDate { get; set; }
        public DateTime DeletedTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

    }
}
