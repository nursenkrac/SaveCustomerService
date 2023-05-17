using System;

namespace SaveCustomerService.Model.ViewModel.User.Outputs
{
    public class UserTokenOutputModel
    {
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
