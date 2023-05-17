using SaveCustomerService.Model.ServiceModel.NVI.Inputs;
using System.Threading.Tasks;

namespace SaveCustomerService.Business.Operation.ServiceOperation.NVI
{
    public class IdentityOperation
    {
        public bool ValidateIdentity(ValidateIdentityInputModel validateIdentityInputModel)
        {
            KPSPublicService.KPSPublicSoapClient client = new KPSPublicService.KPSPublicSoapClient(KPSPublicService.KPSPublicSoapClient.EndpointConfiguration.KPSPublicSoap12);
            var response = Task.Run(async () => await client.TCKimlikNoDogrulaAsync(
                validateIdentityInputModel.IdentityNo,
                validateIdentityInputModel.Name,
                validateIdentityInputModel.Surname,
                validateIdentityInputModel.BirthYear));
            return response.Result.Body.TCKimlikNoDogrulaResult;
        }
    }
}
