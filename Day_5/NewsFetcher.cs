using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace NewsAnalyzer
{
     class Program
     {
          private static readonly List<string> NewsSources = new List<string>
        {
            "VNExpress",
            "TuoiTre",
            "ThanhNien",
            "BBC",
            "CNN"
        };
          private static CancellationTokenSource cts = new CancellationTokenSource();

          static async Task Main(string[] args)
          {
               while (true)
               {
                    Console.WriteLine("\n=== News Analyzer Menu ===");
                    Console.WriteLine("1. Start fetching news");
                    Console.WriteLine("2. Cancel operation");
                    Console.WriteLine("3. Compare Thread vs Task");
                    Console.WriteLine("4. Exit");
                    Console.Write("Choose an option (1-4): ");

                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                         case "1":
                              cts = new CancellationTokenSource();
                              await FetchAllNewsAsync(cts.Token);
                              break;
                         case "2":
                              cts.Cancel();
                              Console.WriteLine("Cancellation requested...");
                              break;
                         case "3":
                              await CompareThreadVsTaskAsync();
                              break;
                         case "4":
                              Console.WriteLine("Exiting...");
                              return;
                         default:
                              Console.WriteLine("Invalid option. Please try again.");
                              break;
                    }
               }
          }

          static async Task<string> GetNewsAsync(string source, CancellationToken token)
          {
               var stopwatch = Stopwatch.StartNew();
               Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Starting to fetch from {source}...");

               try
               {
                    token.ThrowIfCancellationRequested();

                    await Task.Delay(new Random().Next(1000, 5000), token);

                    if (source == "CNN")
                    {
                         throw new HttpRequestException("Failed to fetch news from CNN");
                    }

                    string content = $"Sample news content from {source} ({new Random().Next(100, 1000)} words)";
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Finished fetching from {source} in {stopwatch.ElapsedMilliseconds}ms");
                    return content;
               }
               catch (OperationCanceledException)
               {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Fetching from {source} was cancelled");
                    throw;
               }
               catch (HttpRequestException ex)
               {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Error fetching from {source}: {ex.Message}");
                    throw;
               }
          }

          static async Task FetchAllNewsAsync(CancellationToken token)
          {
               var stopwatch = Stopwatch.StartNew();
               var results = new List<(string Source, string? Content)>();

               try
               {
                    var tasks = NewsSources.Select(async source =>
                    {
                         try
                         {
                              var content = await GetNewsAsync(source, token);
                              return (Source: source, Content: content);
                         }
                         catch (Exception)
                         {
                              return (Source: source, Content: null);
                         }
                    });

                    var completedResults = await Task.WhenAll(tasks);

                    results.AddRange(completedResults.Where(r => r.Content != null));

                    int totalCharacters = results.Sum(r => r.Content?.Length ?? 0);
                    Console.WriteLine($"\nFetch completed in {stopwatch.ElapsedMilliseconds}ms");
                    Console.WriteLine($"Total characters from successful sources: {totalCharacters}");
                    Console.WriteLine("Successful sources:");
                    foreach (var result in results)
                    {
                         Console.WriteLine($"- {result.Source}: {result.Content?.Length ?? 0} characters");
                    }
               }
               catch (OperationCanceledException)
               {
                    Console.WriteLine("Operation was cancelled by user");
               }
               catch (Exception ex)
               {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
               }
          }

          static async Task CompareThreadVsTaskAsync()
          {
               Console.WriteLine("\n=== Comparing Thread vs Task ===");

               var threadStopwatch = Stopwatch.StartNew();
               var threads = new List<Thread>();
               for (int i = 0; i < 3; i++)
               {
                    int index = i;
                    var thread = new Thread(() =>
                    {
                         Console.WriteLine($"Thread {index} started");
                         Thread.Sleep(2000);
                         Console.WriteLine($"Thread {index} finished");
                    });
                    threads.Add(thread);
                    thread.Start();
               }

               foreach (var thread in threads)
               {
                    thread.Join();
               }
               Console.WriteLine($"Threads completed in {threadStopwatch.ElapsedMilliseconds}ms");

               var taskStopwatch = Stopwatch.StartNew();
               var tasks = new List<Task>();
               for (int i = 0; i < 3; i++)
               {
                    int index = i;
                    tasks.Add(Task.Run(async () =>
                    {
                         Console.WriteLine($"Task {index} started");
                         await Task.Delay(2000);
                         Console.WriteLine($"Task {index} finished");
                    }));
               }

               await Task.WhenAll(tasks);
               Console.WriteLine($"Tasks completed in {taskStopwatch.ElapsedMilliseconds}ms");

               Console.WriteLine("\nAnalysis: Tasks are generally more efficient than Threads because:");
               Console.WriteLine("- Tasks use the ThreadPool, reducing thread creation overhead");
               Console.WriteLine("- Tasks support async/await for non-blocking operations");
               Console.WriteLine("- Tasks provide better cancellation and error handling");
          }

          static async Task ParallelFetchAsync(CancellationToken token)
          {
               var stopwatch = Stopwatch.StartNew();
               var results = new ConcurrentBag<(string Source, string? Content)>();

               try
               {
                    await Task.Run(() =>
                    {
                         Parallel.ForEach(NewsSources, new ParallelOptions { CancellationToken = token }, source =>
                     {
                          try
                          {
                               var content = GetNewsAsync(source, token).GetAwaiter().GetResult();
                               results.Add((Source: source, Content: content));
                          }
                          catch (Exception ex)
                          {
                               Console.WriteLine($"Parallel error for {source}: {ex.Message}");
                          }
                     });
                    }, token);

                    int totalCharacters = results.Sum(r => r.Content?.Length ?? 0);
                    Console.WriteLine($"\nParallel fetch completed in {stopwatch.ElapsedMilliseconds}ms");
                    Console.WriteLine($"Total characters from successful sources: {totalCharacters}");
               }
               catch (OperationCanceledException)
               {
                    Console.WriteLine("Parallel operation was cancelled");
               }
          }
     }
}