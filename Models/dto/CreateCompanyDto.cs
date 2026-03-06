namespace Genesis_Core_Api.Models.dto
{
    public class CreateCompanyDto
    {
        public string Name { get; set; } = null!;
        public string Rnc { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }
}
