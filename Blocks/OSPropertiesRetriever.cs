using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityReport.Blocks;
internal class OSPropertiesRetriever : IDataRetriever
{
    public List<Data.Data> RetrieveData()
    {
        List<Data.Data> result = new List<Data.Data>();
        var os = Environment.OSVersion;
        result.Add(new Data.Data("Platform", os.Platform.ToString()));
        result.Add(new Data.Data("Version String:", os.VersionString));
        result.Add(new Data.Data("Version Major:", os.Version.Major.ToString()));
        result.Add(new Data.Data("Version Minor:", os.Version.Minor.ToString()));
        result.Add(new Data.Data("Service Pack:", os.ServicePack));
        return result;
    }
}
