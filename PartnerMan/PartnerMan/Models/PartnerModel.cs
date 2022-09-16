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

        [Required(AllowEmptyStrings = false, ErrorMessage = "A név megadása kötelező!")]
        public string Name { get; set; }

        public List<AddressModel> Addresses { get; set; }
    }
}
