using gamesdatabasetwo.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace gamesdatabasetwo.Data
{
    public class EditGameModel
    {


        [StringLength(15, ErrorMessage = "Name can not be longer than 15 characters. ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name can not be empty. ")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Name { get; set; }

        public string UnEditedName { get; set; }

        [Required(ErrorMessage = "Year can not be empty. ")]
        [Range(1970, 2018, ErrorMessage = "Year has to be between 1970 and 2018. ")]
        public int Year { get; set; }

        [StringLength(35, ErrorMessage = "Platforms can not be longer than 35 characters. ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Platforms can not be empty. ")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Platforms { get; set; }

        [StringLength(15, ErrorMessage = "Theme can not be longer than 15 characters. ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Theme can not be empty. ")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Theme { get; set; }

        [StringLength(50, ErrorMessage = "Genre can not be longer than 50 characters. ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Genre can not be empty. ")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Genre { get; set; }

        [StringLength(35, ErrorMessage = "Locations for release can not be longer than 35 characters. ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Game can not be released Nowhere. ")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ReleasedWhere { get; set; }
        public Publisher Publisher { get; set; }
        public Developer Developer { get; set; }
        public Rating Score { get; set; }
    }
}
