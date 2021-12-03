namespace UniversitySystem.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;
    public class PersonFormModel
    {

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string LastName { get; set; }

    }
}
