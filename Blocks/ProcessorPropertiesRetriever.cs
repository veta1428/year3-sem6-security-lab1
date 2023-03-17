using System.Collections.Generic;
using System.Management;

namespace SecurityReport.Blocks;

internal class ProcessorPropertiesRetriever : IDataRetriever
{
    private const string Win32_Processor = "Win32_Processor";
    private readonly List<string> desiredProperties = new List<string>()
    {
        "Name",
        "Description",
        "AddressWidth",
        "NumberOfCores",
        "NumberOfEnabledCore",
        "NumberOfLogicalProcessors",
        "L2CacheSize",
        "L3CacheSize",
        "LoadPercentage",
    };

    public List<Data.Data> RetrieveData()
    {
        ManagementClass management = new ManagementClass(Win32_Processor);
        ManagementObjectCollection managementobject = management.GetInstances();

        PropertyDataCollection properties =
        management.Properties;

        List<string> propkeys = new List<string>();
        foreach (PropertyData property in properties)
        {
            if(desiredProperties.Contains(property.Name))
            {
                propkeys.Add(property.Name);
            }
        }

        List<Data.Data> result = new List<Data.Data>();

        foreach (ManagementObject mngObject in managementobject)
        {
            foreach (var item in propkeys)
            {
                result.Add(new Data.Data(item, mngObject.Properties[item]?.Value?.ToString() ?? string.Empty));
            }
            break;
        }

        return result;
    }
}
