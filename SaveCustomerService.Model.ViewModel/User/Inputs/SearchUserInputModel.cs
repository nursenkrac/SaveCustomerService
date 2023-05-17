using System;
using System.Collections.Generic;
using System.Text;

namespace SaveCustomerService.Model.ViewModel.User.Inputs
{
    public class SearchUserInputModel
    {
        public SearchUserType type { get; set; }

    }
    public class SearchUserType
    {
        public string IdentityNo { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
