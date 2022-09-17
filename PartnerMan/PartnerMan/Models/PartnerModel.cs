using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PartnerMan.Models
{
    public class PartnerModel
    {

        public PartnerModel()
        {
            Addresses = new List<AddressModel>();
        }

        [DatabaseGenerated( DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(10)]
        public string Title { get; set; }

        [StringLength(100)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A vezetéknév megadása kötelező!")]
        public string LastName { get; set; }

        [StringLength(100)]
        public string MiddleName { get; set; }

        [StringLength(100)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A keresztnév megadása kötelező!")]
        public string FirstName { get; set; }

        [NotMapped]
        public string DisplayName
        {
            get { return $"{Title} {LastName} {MiddleName} {FirstName}".Replace("  "," "); }
        }        

        [StringLength(500)]
        public string Comment { get; set; }

        public List<AddressModel> Addresses { get; set; }
    }
}
