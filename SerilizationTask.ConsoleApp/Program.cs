using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SerilizationTask.Application.Interfaces;
using SerilizationTask.Application.Services;
using SerilizationTask.Data.Repositories;


public class Program
{
    private static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging(config => { config.AddConsole(); })
            .AddSingleton<IPersonRepository, PersonRepository>()
            .AddSingleton<IPersonService, PersonService>()
            .BuildServiceProvider();

        var personService = serviceProvider.GetService<IPersonService>();
        var logger = serviceProvider.GetService<ILogger<Program>>();

        while (true)
        {

            Console.WriteLine("Choose an operation:");
            Console.WriteLine("1. Generate persons");
            Console.WriteLine("2. Display statistics");
            Console.WriteLine("3. Serialize to file");
            Console.WriteLine("4. Deserialize from file");
            Console.WriteLine("5. Clear persons memory");
            Console.WriteLine("0. Exit");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Enter the number of persons to generate:");
                    if (!int.TryParse(Console.ReadLine(), out int count))
                    {
                        logger.LogError("Invalid number, try again.");
                        break;
                    }
                    personService.GeneratePersons(count);
                    break;
                case "2":
                    var stat = personService.GetStatisticDisplay();
                    logger.LogInformation($"Person count:         {stat.PersonCount} \n" +
                                          $"Average children age: {stat.AverageChildAge} \n" +
                                          $"CreditCard Count:     {stat.CreditCardCount} \n");
                    break;
                case "3":
                    Console.WriteLine("Enter the path to the file. Or use by default leaving the line empty");
                    var input = Console.ReadLine();
                    var filePath = string.IsNullOrEmpty(input) ? "Persons.json" : input;
                    try
                    {
                        personService.SerializeInFile(filePath);
                    }
                    catch (Exception e)
                    {
                        logger.LogError($"Failed to serialize due to: {e.Message}");
                    }
                    break;
                case "4":
                    Console.WriteLine("Enter the path to the file. Or use by default leaving the line empty");
                    input = Console.ReadLine();
                    filePath = string.IsNullOrEmpty(input) ? "Persons.json" : input;
                    try
                    {
                        personService.DeserializeInFile(filePath);
                    }
                    catch (Exception e)
                    {
                        logger.LogError($"Failed to deserialize due to: {e.Message}");
                    }
                    break;
                case "5":
                    personService.Clear();
                    logger.LogInformation("Cleared the list of persons.");
                    break;
                case "0":
                    return;
                default:
                    logger.LogInformation("Invalid choice, try again.");
                    break;
            }
            Thread.Sleep(100);
        }
    }
}
