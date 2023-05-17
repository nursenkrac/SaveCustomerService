using System;
using System.Collections.Generic;
using System.Text;

namespace SaveCustomerService.Model.ViewModel.User.Outputs
{
    public class GetUserOutputModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string BirthDate { get; set; }
        public string NationalIdentifier { get; set; }
        public bool IsActive { get; set; }
        public DateTime DeletedDate { get; set; }

    }
}
