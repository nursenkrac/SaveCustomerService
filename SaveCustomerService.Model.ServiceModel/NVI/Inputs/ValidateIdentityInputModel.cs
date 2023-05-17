namespace SaveCustomerService.Model.ServiceModel.NVI.Inputs
{
    public class ValidateIdentityInputModel
    {
        public long IdentityNo { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int BirthYear { get; set; }
    }
}
