using SaveCustomerService.Data.Repository.Derived.EFSQL;
using SaveCustomerService.Model.ServiceModel.User;
using SaveCustomerService.Model.ViewModel.User.Inputs;
using SaveCustomerService.Model.ViewModel.User.Outputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaveCustomerService.Operation.ServiceOperation.Sql
{

    public class UserOperation
    {
        private readonly SaveCustomerContext _context;
        public UserOperation()
        {
            _context = new SaveCustomerContext();
        }
        public int PostUser(UserCreateInputModel input)
        {

            try
            {
                CustomersInformation info = new CustomersInformation()
                {
                    Name = input.Name,
                    Surname = input.Surname,
                    BirthDate = input.BirthDate.ToString(),
                    IsDeleted = false,
                    IsActive = true,
                    National_Identifier = input.IdentityNo
                };

                _context.CustomersInformation.Add(info);

                if (_context.SaveChanges() == 1)
                    return 1;

            }
            catch (Exception ex)
            {

                return 0;
            }
         
            return 0;
        }
        public int SearchUser(string identitiy)
        {
            CustomersInformation info = new CustomersInformation();
            var users = (from pq in _context.CustomersInformation
                         where pq.National_Identifier == identitiy
                         select new
                         {
                             Id = pq.Id,

                         }).ToList();


            if (users.Count() != 0)
                return 0;

            return 1;
        }
        public GetUserOutputModel GetUserById(int Id)
        {
            GetUserOutputModel info = new GetUserOutputModel();
            var users = (from pq in _context.CustomersInformation
                         where pq.Id == Id
                         select new
                         {
                             pq.Name,
                             pq.Surname,
                             pq.National_Identifier,
                             pq.BirthDate,
                             pq.IsActive,
                             pq.IsDeleted,

                         }).ToList().First();


            if (users.Name != null)
            {
                info.Name = users.Name;
                info.Surname = users.Surname;
                info.BirthDate = users.BirthDate;
                info.IsActive = users.IsActive;
                info.NationalIdentifier = users.National_Identifier;

            }
                

            return info;
        }
        public int DeleteUser(int Id)
        {
            var users = (from pq in _context.CustomersInformation
                         where pq.Id == Id
                         select new
                         {
                             pq.Name,
                             pq.Surname,
                             pq.National_Identifier,
                             pq.BirthDate,
                             pq.IsActive,
                             pq.IsDeleted,

                         }).ToList().First();


            if (users.Name != null)
            {
                try
                {
                    var user = new CustomersInformation { Id = Id };
                    _context.CustomersInformation.Remove(user);
                    if (_context.SaveChanges() == 1)
                     return 1;
                }
                catch (Exception ex)
                {

                    return 0;
                }
             
            }
           

            return 0;

        }
        public GetUserOutputModel SearchUser(SearchUserInputModel input)
        {
            GetUserOutputModel info = new GetUserOutputModel();
            if (input.type.Name !=null)
            {
               info = (from pq in _context.CustomersInformation
                             where pq.Name == input.type.Name
                             select new GetUserOutputModel()
                             {
                                Name= pq.Name,
                                Surname = pq.Surname,
                                NationalIdentifier = pq.National_Identifier,
                                BirthDate = pq.BirthDate,
                                IsActive = pq.IsActive,

                             }).First();
             

            }
            else if (input.type.Name != null)
            {
                info = (from pq in _context.CustomersInformation
                             where pq.Name == input.type.Surname
                             select new GetUserOutputModel()
                             {
                                 Name = pq.Name,
                                 Surname = pq.Surname,
                                 NationalIdentifier = pq.National_Identifier,
                                 BirthDate = pq.BirthDate,
                                 IsActive = pq.IsActive,

                             }).First();

            }
            else
            {
               info = (from pq in _context.CustomersInformation
                             where pq.Name == input.type.IdentityNo
                             select new GetUserOutputModel()
                             {
                                 Name = pq.Name,
                                 Surname = pq.Surname,
                                 NationalIdentifier = pq.National_Identifier,
                                 BirthDate = pq.BirthDate,
                                 IsActive = pq.IsActive,

                             }).First();

            }


            return info;
        }


    }
}
