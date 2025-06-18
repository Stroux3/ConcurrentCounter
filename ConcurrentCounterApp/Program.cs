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

       Console.WriteLine("\t\t\t\tВсе писатели пишут последовательно и никогда одновременно");
       await Writers_ShouldBeSequentialAndAccurate_ReaderWriterLockSlim();
       await Writers_ShouldBeSequentialAndAccurate_Manual();
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

    #region Все писатели пишут последовательно и никогда одновременно
    #region Реализация через ReaderWriterLockSlim
    public static async Task Writers_ShouldBeSequentialAndAccurate_ReaderWriterLockSlim()
    {
        Console.WriteLine($"\nРеализация через ReaderWriterLockSlim\n");
        ServerWithReaderWriterLockSlim.Reset();
        int count = 5;
        var tasks = Enumerable.Range(0, 10)
            .Select(i => Task.Run(() =>
            {
                ServerWithReaderWriterLockSlim.AddToCount(count);
                int current = ServerWithReaderWriterLockSlim.GetCount();
                Console.WriteLine($"Писатель {i} пишет {count}, текущие значение: {current}");
            }))
            .ToArray();

        await Task.WhenAll(tasks);
    }
    #endregion

    #region Реализация без использования ReaderWriterLockSlim
    public static async Task Writers_ShouldBeSequentialAndAccurate_Manual()
    {
        Console.WriteLine($"\nРеализация без использования ReaderWriterLockSlim\n");
        ServerManual.Reset();
        int count = 5;
        var tasks = Enumerable.Range(0, 10)
            .Select(i => Task.Run(() =>
            {
                ServerManual.AddToCount(count);
                int current = ServerManual.GetCount();
                Console.WriteLine($"Писатель {i} пишет {count}, текущие значение: {current}");
            }))
            .ToArray();

        await Task.WhenAll(tasks);
    }
    #endregion

    #endregion



}


