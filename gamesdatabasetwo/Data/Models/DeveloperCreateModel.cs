using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gamesdatabasetwo.Data
{
    public class DeveloperCreateModel
    {
        [StringLength(15, ErrorMessage = "Name can not be longer than 25 characters. ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name can not be empty. ")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Name { get; set; }
    }
}
