using System.ComponentModel.DataAnnotations;

namespace SecurityReport.Enums;
public enum Block
{
    OS = 1,

    [Display(Name = "Processor")]
    Processor = 2,

    [Display(Name = "Random Access Memory")]
    Ram = 3,

    [Display(Name = "Drives")]
    Drives = 4,

    [Display(Name = "Windows Updates")]
    Updates = 5,

    AntiVirus = 6,

    FireWall = 7,
}
