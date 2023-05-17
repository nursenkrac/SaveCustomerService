using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;
using SaveCustomerService.Business.Manager.User;
using SaveCustomerService.Presentation.Api.Helpers;
using Core.Model.Infrastructure.Results;
using SaveCustomerService.Model.ViewModel.User.Inputs;
using SaveCustomerService.Model.ViewModel.User.Outputs;
using SaveCustomerService.Core.Api;
using SaveCustomerService.Encryption.Derived;

using Microsoft.AspNetCore.Authorization;
using SaveCustomerService.ActionFilters;

namespace SaveCustomerService.Controllers
{
  
    [AllowAnonymous]
    public class UserController : BaseController
    {
        private readonly UsersBusinessManager _businessManager;
        private readonly TokenUser _tokenUser;
        private readonly IStringLocalizer<UserController> _stringLocalizer;
        public UserController(IHttpContextAccessor httpContextAccessor, IStringLocalizer<UserController> stringLocalizer)
        {
            _businessManager = new UsersBusinessManager();
            _tokenUser = new TokenUser(httpContextAccessor);
            _stringLocalizer = stringLocalizer;
        }
      

        [HttpPost]
        public IResultObjectModel<UserCreateOutputModel> Create(UserCreateInputModel userCreateInputModel)
        {
            return _businessManager.PostUser(userCreateInputModel);
        }

        [HttpPost]
        [Rs256Authorization]
        public IResultObjectModel<GetUserOutputModel> UserGetById(int Id)
        {
            return _businessManager.GetUserById(Id);
        }
        [HttpPost]
        [Rs256Authorization]
        public IResultObjectModel<UserCreateOutputModel> DeleteUser(int Id)
        {
            return _businessManager.DeleteUser(Id);
        }
        [HttpPost]
        [Rs256Authorization]
        public IResultObjectModel<GetUserOutputModel> SearchUser(SearchUserInputModel input)
        {
            return _businessManager.SearchUser(input);
        }
    }
}
