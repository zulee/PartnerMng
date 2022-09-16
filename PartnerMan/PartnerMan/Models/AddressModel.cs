using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PartnerMan.Models
{
    public class AddressModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Az ország megadása kötelező!")]
        public string CountryCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Az város megadása kötelező!")]
        public string City { get; set; }

        // Irányítószám string, mert külföldi cím esetében nem ismert a formátuma
        [Required(AllowEmptyStrings = false, ErrorMessage = "Az irányítószám megadása kötelező!")]
        public string PostalCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Az utca, házszám megadása kötelező!")]
        public string Address { get; set; }
    }
}
