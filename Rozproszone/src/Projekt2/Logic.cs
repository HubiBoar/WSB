using System.Collections.Concurrent;
using MoreLinq;

public static class Logic
{
    //Environment.ProcessorCount
    public static async Task SortFile(string inputFilePath, string outputFilePath, int processesCount)
    {
        var lines = await File.ReadAllLinesAsync(inputFilePath);

        var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = processesCount };

        var collection = new BlockingCollection<string>();

        var linesBatch = lines.Batch(processesCount);

        await Parallel.ForEachAsync(linesBatch, parallelOptions, (lineBatch, token) =>
        {
            var ordered = lineBatch.OrderBy(int.Parse).ToArray();

            foreach(var l in ordered)
            {
                collection.Add(l);
            }

            return ValueTask.CompletedTask;
        });

        var finalResult = collection.AsEnumerable().OrderBy(int.Parse).ToArray();

        await File.WriteAllLinesAsync(outputFilePath, finalResult);
    }

    public static async Task CreateFile(string filePath, int linesCount)
    {
        var random = new Random();
        var result = Enumerable.Range(0, linesCount)
            .Select(x => random.Next(0, linesCount).ToString()!)
            .ToArray();

        await File.WriteAllLinesAsync(filePath, result);
    }
}