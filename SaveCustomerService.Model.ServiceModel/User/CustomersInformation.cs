using System;
using System.Collections.Generic;
using System.Text;

namespace SaveCustomerService.Model.ServiceModel.User
{
    public class CustomersInformation
    {
        public int Id { get; set; }
        public string National_Identifier { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string BirthDate { get; set; }
        public DateTime DeletedTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }


    }
}
