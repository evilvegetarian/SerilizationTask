using Microsoft.Extensions.Logging;
using SerilizationTask.Application.Interfaces;
using System.Text.Json;

namespace SerilizationTask.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository personRepository;
        private readonly ILogger<PersonService> logger;

        public PersonService(IPersonRepository personRepository, ILogger<PersonService> logger)
        {
            this.personRepository = personRepository;
            this.logger = logger;
        }

        public void GeneratePersons(int count)
        {
            try
            {
                personRepository.GeneratePersons(count);
                logger.LogInformation("Successfully generated {Count} persons.", count);
            }
            catch (ArgumentException ae)
            {
                logger.LogError("Failed to generate persons due to an invalid argument: {ExceptionMessage}", ae.Message);
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to generate persons due to an unexpected error: {ExceptionMessage}", ex.Message);
                throw;
            }
        }

        public void Clear()
        {
            try
            {
                personRepository.Clear();
                logger.LogInformation("Successfully cleared the person list.");
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to clear the person list due to an unexpected error: {ExceptionMessage}", ex.Message);
                throw;
            }
        }

        public StatisticDisplay GetStatisticDisplay()
        {
            try
            {
                var stat = personRepository.GetStatistic();
                if (stat.PersonCount == 0) // check if the default value returned
                {
                    logger.LogInformation("The list is empty, no statistics to show.");
                }
                else
                {
                    logger.LogInformation("Successfully calculated statistics: {Statistics}.", stat);
                }
                return stat;
            }

            catch (Exception ex)
            {
                logger.LogError("Failed to get statistics due to an unexpected error: {ExceptionMessage}", ex.Message);
                throw;

            }
        }


        public void SerializeInFile(string path)
        {
            try
            {
                personRepository.SerializeInFile(path);

                logger.LogInformation("Successfully serialized the person list to file at {path}.", path);

            }
            catch (ArgumentException ae)
            {
                logger.LogError("Failed to serialize the person list due to an invalid argument: {ExceptionMessage}", ae.Message);
                throw;
            }
            catch (Exception e)
            {
                logger.LogError("Failed to serialize the person list due to an unexpected error: {ExceptionMessage}", e.Message);
                throw;
            }
        }

        public void DeserializeInFile(string path)
        {
            try
            {
                personRepository.Deserialize(path);
                logger.LogInformation($"Successfully deserialized the person list from file at {path}.");

            }
            catch (FileNotFoundException fnfe)
            {
                logger.LogError("Failed to deserialize the person list because the file was not found: {ExceptionMessage}", fnfe.Message);
                throw;
            }
            catch (JsonException je)
            {
                logger.LogError("Failed to deserialize the person list due to an invalid JSON format: {ExceptionMessage}", je.Message);
                throw;
            }
            catch (Exception e)
            {
                logger.LogError("Failed to deserialize the person list due to an unexpected error: {ExceptionMessage}", e.Message);
                throw;
            }
        }



    }
}