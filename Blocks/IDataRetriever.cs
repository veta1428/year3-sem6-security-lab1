using System.Collections.Generic;
using SecurityData = SecurityReport.Data.Data;
namespace SecurityReport.Blocks;

internal interface IDataRetriever
{
    List<SecurityData> RetrieveData();
}
