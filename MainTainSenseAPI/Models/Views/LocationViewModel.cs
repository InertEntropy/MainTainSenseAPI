using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models.Views
{
    public class LocationViewModel : BaseViewModels
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide a name for the location.")]
        [StringLength(50, ErrorMessage = "Location Name should be between 2 and 50 characters long.", MinimumLength = 2)]
        public string? LocationName { get; set; }

        [Required]
        public string? LocationDescription { get; set; }

        [Required]
        public int BuildingId { get; set; }
        public IEnumerable<SelectListItem>? AvailableBuildings { get; set; }

        public string? LocationPath { get; set; }

        [Required]
        public int ParentLocationId { get; set; }
        public IEnumerable<SelectListItem>? AvailableParentLocations { get; set; }

    }
}
