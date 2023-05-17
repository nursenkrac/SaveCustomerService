using System;
using System.Collections.Generic;
using System.Text;

namespace SaveCustomerService.Model.ViewModel.User.Outputs
{
    public class UserInfoOutputModel
    {
        public bool status { get; set; }
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }

    }
    public class UserInfo
    {
        public string Identitiy { get; set; }
    }
}
