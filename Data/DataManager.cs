using SecurityReport.Blocks;
using SecurityReport.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SecurityReport.Data;

public class DataManager
{
    private static DataManager _instance;

    private Dictionary<Block, List<Data>> _data = new Dictionary<Block, List<Data>>();

    private DataManager()
    {
        _instance = this;
    }

    public static DataManager GetManager()
    {
        if (_instance is null)
            _instance = new DataManager();
        return _instance;
    }

    public void UpdateData(IEnumerable<Block> blocks)
    {
        _data.Clear();
        foreach (var block in blocks)
        {
            var data = GetDelegate(block).Invoke();
            _data.Add(block, data);
        }
    }

    public Dictionary<Block, List<Data>> Data => _data;

    private Func<List<Data>> GetDelegate(Block block)
    {
        switch (block)
        {
            case Block.Processor:
                return GetProcessorProperties;
            case Block.Ram:
                return GetRam;
            case Block.Drives:
                return GetDisks;
            case Block.Updates:
                return GetUpdates;
            case Block.AntiVirus:
                return GetAntivirus;
            case Block.FireWall:
                return GetFireWall;
            case Block.OS:
                return GetOs;
            default:
                throw new Exception($"Delegate not specified for Block = {block.ToString()}");
        }
    }

    private List<Data> GetProcessorProperties()
    {
        var retriever = new ProcessorPropertiesRetriever();
        return retriever.RetrieveData();
    }

    private List<Data> GetUpdates()
    {
        var retriever = new UpdatesPropertiesRetriever();
        return retriever.RetrieveData();
    }

    private List<Data> GetRam()
    {
        var retriever = new RamPropertiesRetriever();
        return retriever.RetrieveData();
    }

    private List<Data> GetDisks()
    {
        var retriever = new DrivesPropertiesRetriever();
        return retriever.RetrieveData();
    }
    private List<Data> GetAntivirus()
    {
        var retriever = new AntivirusPropertiesRetriever();
        return retriever.RetrieveData();
    }

    private List<Data> GetFireWall()
    {
        var retriever = new FireWallPropertiesRetriever();
        return retriever.RetrieveData();
    }

    private List<Data> GetOs()
    {
        var retriever = new OSPropertiesRetriever();
        return retriever.RetrieveData();
    }
    public MemoryStream GetJson()
    {
        MemoryStream stream = new MemoryStream();
        var options = new JsonSerializerOptions { WriteIndented = true };
        JsonSerializer.Serialize<Dictionary<Block, List<Data>>>(stream, _data, options);
        return stream;
    }
}
