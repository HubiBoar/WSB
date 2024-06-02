using BenchmarkDotNet.Attributes;

public class FileSortBenchmark
{
    private const string InputFilePath = "Input.txt";
    private const string OutputFilePath = "Output.txt";

    [Params(1, 2, 3, 4, 5, 6)]
    public int ProcessorsCount;

    [Params(10000)]
    public int FileSize;

    [GlobalSetup]
    public Task Setup()
    {
        return Logic.CreateFile(InputFilePath, FileSize);
    }

    [Benchmark]
    public Task SortFile_Processor_Count()
    {
        return Logic.SortFile(InputFilePath, OutputFilePath, ProcessorsCount);
    }


    [Benchmark]
    public Task SortFile_Processor_Max()
    {
        return Logic.SortFile(InputFilePath, OutputFilePath, Environment.ProcessorCount);
    }
}