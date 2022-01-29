using System.ComponentModel.DataAnnotations;

namespace WebApiCampaign.DTOs
{
    public class CampaignCreateDTO : IValidatableObject
    {
        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(maximumLength: 100, ErrorMessage = "Field {0} can not have more than {1} characters")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(maximumLength: 20, ErrorMessage = "Field {0} can not have more than {1} characters")]
        public string Code { get; set; } = string.Empty;
        [Required(ErrorMessage = "Field {0} is required")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Field {0} is required")]
        public DateTime EndDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate > EndDate)
            {
                yield return new ValidationResult("StartDate can't be greater than the EndDate",
                    new string[] { nameof(StartDate) });
            }
        }
    }
}
