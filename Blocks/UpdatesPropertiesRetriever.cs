using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WUApiLib;

namespace SecurityReport.Blocks;
internal class UpdatesPropertiesRetriever : IDataRetriever
{
    private const int MaxNumber = 30;
    public List<Data.Data> RetrieveData()
    {
        List<Data.Data> result = new List<Data.Data>();

        var updateSession = new UpdateSession();
        var updateSearcher = updateSession.CreateUpdateSearcher();

        var history = updateSearcher.QueryHistory(0, 30);
        for (int i = 0; i < 30; i++)
        {
            if (history[i].ResultCode == OperationResultCode.orcSucceeded)
            {
                result.Add(new Data.Data(string.Empty, history[i].Title));
                result.Add(new Data.Data("Date", history[i].Date.ToString("G")));
                result.Add(new Data.Data("Description", history[i].Description));
            }
        }

        return result;
    }
}
