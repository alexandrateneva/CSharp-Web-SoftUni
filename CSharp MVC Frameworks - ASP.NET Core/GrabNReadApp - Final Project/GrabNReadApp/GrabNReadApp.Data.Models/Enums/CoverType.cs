using System.ComponentModel.DataAnnotations;

namespace GrabNReadApp.Data.Models.Enums
{
    public enum CoverType
    {
        [Display(Name = "Soft Cover")]
        SoftCover = 1,
        [Display(Name = "Hard Cover")]
        HardCover = 2
    }
}
