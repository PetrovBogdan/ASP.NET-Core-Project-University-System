namespace UniversitySystem.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;
    public class PersonFormModel
    {

        [Required]
        [Display(Name ="First Name")]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string LastName { get; set; }

    }
}
