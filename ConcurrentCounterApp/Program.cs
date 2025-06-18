using ConcurrentCounter;
using ConcurrentCounter.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
       Console.WriteLine("\t\t\t\tВсе читатели параллельно читают одно и тоже значение");
       await MultipleReaders_ShouldReadSameValue_ReaderWriterLockSlim();
       await MultipleReaders_ShouldReadSameValue_Manual();
    }
    #region Все читатели параллельно читают одно и тоже значение

    #region Реализация через ReaderWriterLockSlim
    public static async Task MultipleReaders_ShouldReadSameValue_ReaderWriterLockSlim()
    {
        Console.WriteLine($"\nРеализация через ReaderWriterLockSlim\n");
        ServerWithReaderWriterLockSlim.Reset();
        ServerWithReaderWriterLockSlim.AddToCount(10);

        var tasks = Enumerable.Range(0, 5)
            .Select(i => Task.Run(() => (Index: i, Value: ServerWithReaderWriterLockSlim.GetCount())))
            .ToArray();

        var results = await Task.WhenAll(tasks);

        foreach (var result in results)
        {
            Console.WriteLine($"Читатель {result.Index} читает значение: {result.Value}");
        }
    }
    #endregion

    #region Реализация без использования ReaderWriterLockSlim
    public static async Task MultipleReaders_ShouldReadSameValue_Manual()
    {
        Console.WriteLine($"\nРеализация без использования ReaderWriterLockSlim\n");
        ServerManual.Reset();
        ServerManual.AddToCount(20);

        var tasks = Enumerable.Range(0, 5)
            .Select(i => Task.Run(() => (Index: i, Value: ServerManual.GetCount())))
            .ToArray();

        var results = await Task.WhenAll(tasks);

        foreach (var result in results)
        {
            Console.WriteLine($"Читатель {result.Index} читает значение: {result.Value}");
        }
    }
    #endregion
    #endregion

}


