using System;
using System.Collections.Generic;
using System.Text;
using Core.DtoModel.Infrastructure;

namespace SaveCustomerService.Model.DtoModel.User.Dto
{
    public class UserDto
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
