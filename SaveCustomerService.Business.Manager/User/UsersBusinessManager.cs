using System;
using System.Collections.Generic;
using System.Text;
using Core.ExceptionHandling.Manager;
using Core.Model.Infrastructure.Results;
using Core.Operation.Derived.ValidationOperation;
using Core.Operation.Infrastructure;
using SaveCustomerService.Model.ViewModel.User.Inputs;
using SaveCustomerService.Model.ViewModel.User.Outputs;
using SaveCustomerService.Data.Infrastructure.User;
using SaveCustomerService.Model.DtoModel.User.Dto;
using SaveCustomerService.Core.Utility.Utility;
using SaveCustomerService.Business.Operation.ServiceOperation.NVI;
using PagedList.Core;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using SaveCustomerService.Operation.ServiceOperation.Sql;


namespace SaveCustomerService.Business.Manager.User
{
    public class UsersBusinessManager
    {
        public IResultObjectModel<UserCreateOutputModel> PostUser(UserCreateInputModel input)
        {
            IResultObjectModel<UserCreateOutputModel> resultObjectModel = new ResultObjectModel<UserCreateOutputModel>();
            EncryptDecryptUtility<UserCreateInputModel> findEncryptDecryptUtility = new EncryptDecryptUtility<UserCreateInputModel>();
            var encryptedIdentity = findEncryptDecryptUtility.Encrypt(input.IdentityNo);
            var userFindResult = GetByIdentityNo(encryptedIdentity);
            if (userFindResult.IsSuccess)
            {
                if (userFindResult.Data.Status == false)
                {
                    //kullanıcı kaydı varmı kontrolü eklendi
                    resultObjectModel.Data.Status = false;
                    resultObjectModel.IsSuccess = false;
                    resultObjectModel.Message = "Kullanıcı zaten kayıtlıdır";
                    return resultObjectModel;

                }

            }
            try
            {
                IdentityOperation identityOperation = new IdentityOperation();
                var validateModel = new Model.ServiceModel.NVI.Inputs.ValidateIdentityInputModel
                {
                    IdentityNo = long.Parse(input.IdentityNo),
                    Name = input.Name,
                    Surname = input.Surname,
                    BirthYear = input.BirthDate.Year
                };
                var validUser = identityOperation.ValidateIdentity(validateModel);
                if (validUser)
                {


                    UserOperation prOperation = new UserOperation();
                    UserCreateInputModel inputmodel = new UserCreateInputModel();
                    inputmodel = findEncryptDecryptUtility.Encrypt(input);

                    var result = prOperation.PostUser(inputmodel);
                    if (result == 1)
                    {
                        UserInfo info = new UserInfo
                        { 
                            Identitiy = input.IdentityNo
                        };

                        UserTokenOutputModel ou = new UserTokenOutputModel();
                        ou = new JwtTokenUtility(info).CreateToken();
                        resultObjectModel.Data.Token = ou.Token;
                        resultObjectModel.Data.Status = true;
                        resultObjectModel.IsSuccess = true;
                        resultObjectModel.Message = "Kayıt Başarılı";
                        return resultObjectModel;
                    }
                }
                else
                {
                    resultObjectModel.Data.Status = false;
                    resultObjectModel.IsSuccess = false;
                    resultObjectModel.Message = "Kayıt Başarısız";
                    resultObjectModel.Messages.Add(new ResultMessage { Code = "connection_error", Message = "Kullanıcı Doğrulanamadı" });
                }
            }
            catch (Exception ex)
            {
                resultObjectModel.Data.Status = false;
                resultObjectModel.IsSuccess = false;
                resultObjectModel.Message = "Kayıt Başarısız";
                resultObjectModel.Messages.Add(new ResultMessage { Code = "connection_error", Message = "Bağlantı hatası. Lütfen daha sonra tekrar deneyiniz." });
            }


            return resultObjectModel;


        }
        public IResultObjectModel<UserCreateOutputModel> GetByIdentityNo(string identityNo)
        {
            ResultObjectModel<UserCreateOutputModel> resultObjectModel = new ResultObjectModel<UserCreateOutputModel>();
            try
            {
                UserOperation prOperation = new UserOperation();
                var result = prOperation.SearchUser(identityNo);
                if (result==1)
                {
                    resultObjectModel.Data.Status = true;
                    resultObjectModel.IsSuccess = true;
                }
                else
                {
                    resultObjectModel.Data.Status = false;
                    resultObjectModel.IsSuccess = false;
                }
                

            }
            catch (Exception ex)
            {
                resultObjectModel.IsSuccess = false;
                resultObjectModel.Data.Status = false;
                //ExceptionManager exceptionManager = new ExceptionManager(ex);
                //resultObjectModel.Messages.AddRange(exceptionManager.GetMessages());
                resultObjectModel.Messages.Add(new ResultMessage { Code = "connection_error", Message = "Bağlantı hatası. Lütfen daha sonra tekrar deneyiniz." });
            }
            
            return resultObjectModel;
        }
        public IResultObjectModel<GetUserOutputModel> GetUserById(int Id)
        {
            ResultObjectModel<GetUserOutputModel> resultObjectModel = new ResultObjectModel<GetUserOutputModel>();
            try
            {
                UserOperation prOperation = new UserOperation();
                var result = prOperation.GetUserById(Id);
                GetUserOutputModel ou = new GetUserOutputModel();
                EncryptDecryptUtility<GetUserOutputModel> findEncryptDecryptUtility = new EncryptDecryptUtility<GetUserOutputModel>();
                
                if (result.Name !=null)
                {
                    ou = new GetUserOutputModel
                    {
                        Name = findEncryptDecryptUtility.Decrypt(result.Name),
                        Surname = findEncryptDecryptUtility.Decrypt(result.Surname),
                        BirthDate = result.BirthDate,
                        NationalIdentifier = findEncryptDecryptUtility.Decrypt(result.NationalIdentifier),
                        IsActive = result.IsActive,
                    };

                    resultObjectModel.Data = ou;
                    resultObjectModel.IsSuccess = true;

                }
                else
                {
                    resultObjectModel.IsSuccess = false;
                    resultObjectModel.Message = "Kullanıcı Bulunamadı";

                }

            }
            catch (Exception ex)
            {
                resultObjectModel.IsSuccess = false;
                //ExceptionManager exceptionManager = new ExceptionManager(ex);
                //resultObjectModel.Messages.AddRange(exceptionManager.GetMessages());
                resultObjectModel.Messages.Add(new ResultMessage { Code = "connection_error", Message = "Bağlantı hatası. Lütfen daha sonra tekrar deneyiniz." });
            }

            return resultObjectModel;
        }
        public IResultObjectModel<UserCreateOutputModel> DeleteUser(int Id)
        {
            ResultObjectModel<UserCreateOutputModel> resultObjectModel = new ResultObjectModel<UserCreateOutputModel>();
            try
            {
                UserOperation prOperation = new UserOperation();
                var result = prOperation.DeleteUser(Id);               
                if (result == 1)
                {
                   
                    resultObjectModel.Data.Status = true;
                    resultObjectModel.IsSuccess = true;

                }
                else
                {
                    resultObjectModel.IsSuccess = false;
                    resultObjectModel.Message = "Kullanıcı Bulunamadı";

                }

            }
            catch (Exception ex)
            {
                resultObjectModel.IsSuccess = false;
                //ExceptionManager exceptionManager = new ExceptionManager(ex);
                //resultObjectModel.Messages.AddRange(exceptionManager.GetMessages());
                resultObjectModel.Messages.Add(new ResultMessage { Code = "connection_error", Message = "Bağlantı hatası. Lütfen daha sonra tekrar deneyiniz." });
            }

            return resultObjectModel;
        }
        public IResultObjectModel<GetUserOutputModel> SearchUser(SearchUserInputModel input)
        {

            ResultObjectModel<GetUserOutputModel> resultObjectModel = new ResultObjectModel<GetUserOutputModel>();
            try
            {
                UserOperation prOperation = new UserOperation();
                var result = prOperation.SearchUser(input);
                GetUserOutputModel ou = new GetUserOutputModel();
                EncryptDecryptUtility<GetUserOutputModel> findEncryptDecryptUtility = new EncryptDecryptUtility<GetUserOutputModel>();

                if (result.Name != null)
                {
                    ou = new GetUserOutputModel
                    {
                        Name = findEncryptDecryptUtility.Decrypt(result.Name),
                        Surname = findEncryptDecryptUtility.Decrypt(result.Surname),
                        BirthDate = result.BirthDate,
                        NationalIdentifier = findEncryptDecryptUtility.Decrypt(result.NationalIdentifier),
                        IsActive = result.IsActive,
                    };

                    resultObjectModel.Data = ou;
                    resultObjectModel.IsSuccess = true;

                }
                else
                {
                    resultObjectModel.IsSuccess = false;
                    resultObjectModel.Message = "Kullanıcı Bulunamadı";

                }

            }
            catch (Exception ex)
            {
                resultObjectModel.IsSuccess = false;
                //ExceptionManager exceptionManager = new ExceptionManager(ex);
                //resultObjectModel.Messages.AddRange(exceptionManager.GetMessages());
                resultObjectModel.Messages.Add(new ResultMessage { Code = "connection_error", Message = "Bağlantı hatası. Lütfen daha sonra tekrar deneyiniz." });
            }

            return resultObjectModel;
        }
    }
}
