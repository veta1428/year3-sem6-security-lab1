using System;
using System.Collections.Generic;
using System.IO;

namespace SecurityReport.Blocks;
internal class DrivesPropertiesRetriever : IDataRetriever
{
    public List<Data.Data> RetrieveData()
    {
        List<Data.Data> data = new List<Data.Data>();
        DriveInfo[] allDrives = DriveInfo.GetDrives();

        foreach (DriveInfo d in allDrives)
        {
            data.Add(new Data.Data($"Drive {d.Name}", String.Empty));
            data.Add(new Data.Data($"  Drive type: ", d.DriveType.ToString()));

            if (d.IsReady == true)
            {
                data.Add(new Data.Data($"  Volume label:", d.VolumeLabel));
                data.Add(new Data.Data($"  File system:", d.DriveFormat));
                data.Add(new Data.Data($"  Available space to current user:", d.AvailableFreeSpace.ToString()));
                data.Add(new Data.Data($"  Total available space:", d.TotalFreeSpace.ToString()));
                data.Add(new Data.Data($"  Total size of drive:", d.TotalSize.ToString()));
            }
        }

        return data;
    }
}
