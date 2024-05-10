namespace ESTORE.Shared.DTO.User
{
    public class AddressDTO
    {

        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Barangay { get; set; } = string.Empty;
        public int ZipCode { get; set; }
        public string Country { get; set; } = string.Empty;
    }
}
