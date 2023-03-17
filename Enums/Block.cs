using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityReport.Enums;
public enum Block
{
    [Display(Name = "Processor")]
    Processor = 1,

    [Display(Name = "Random Access Memory")]
    Ram = 2,

    [Display(Name = "Drives")]
    Drives = 3,

    [Display(Name = "Windows Updates")]
    Updates = 4,

    AntiVirus = 5,

    FireWall = 6,

    OS = 7,
}
