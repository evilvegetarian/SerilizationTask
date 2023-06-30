using Microsoft.Extensions.Logging;
using SerilizationTask.Application;
using SerilizationTask.Application.Interfaces;
using SerilizationTask.Domain.Enums;
using SerilizationTask.Domain.Models;
using System.Text.Json;

namespace SerilizationTask.Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private List<Person> people;
        private static readonly Random random = new Random();
        private readonly ILogger<PersonRepository> logger;

        public PersonRepository(ILogger<PersonRepository> logger)
        {
            people = new List<Person>();
            this.logger = logger;
        }

        private Person RandomPerson(int id)
        {
            var person = new Person
            {
                Id = id,
                TransportId = Guid.NewGuid(),
                FirstName = $"FirstName{id}",
                LastName = $"LastName{id}",
                SequenceId = random.Next(1, 10000),
                Age = random.Next(18, 70),
                Phones = new[] { $"+1-555-{id:0000}" },
                BirthDate = new DateTimeOffset(DateTime.UtcNow.AddYears(-random.Next(18, 70))).ToUnixTimeSeconds(),
                Salary = random.NextDouble() * 100000,
                IsMarred = random.NextDouble() > 0.5,
                Gender = (Gender)(id % 2)
            };

            var cardsCount = random.Next(0, 5);
            person.CreditCardNumbers = new string[cardsCount];
            person.Children = new Child[cardsCount];
            for (int i = 0; i < cardsCount; i++)
            {
                person.CreditCardNumbers[i] = $"4532{random.Next(1000000000, 2000000000)}:0000";

                person.Children[i] = new Child
                {
                    Id = id,
                    FirstName = $"ChildFirstName{id}",
                    LastName = $"ChildLastName{id}",
                    BirthDate = new DateTimeOffset(DateTime.UtcNow.AddYears(-random.Next(1, 18))).ToUnixTimeSeconds(),
                    Gender = (Gender)(id % 2)
                };
            }

            return person;
        }

        public void GeneratePersons(int count)
        {
            for (int i = 0; i < count; i++)
            {
                people.Add(RandomPerson(i));
            }
        }

        public StatisticDisplay GetStatistic()
        {
            int personCount = people.Count();
            int creditCardCount = people.Sum(p => p.CreditCardNumbers.Length);
            double averageChildAge = people.SelectMany(x => x.Children)
                                          .Select(x => DateTimeOffset.UtcNow.ToUnixTimeSeconds() - x.BirthDate)
                                          .Average(ageInSeconds => ageInSeconds / (365.25 * 24 * 60 * 60));


            return new StatisticDisplay { AverageChildAge = averageChildAge, CreditCardCount = creditCardCount, PersonCount = personCount };
        }

        public void Clear()
        {
            people.Clear();
        }

        public void SerializeInFile(string path)
        {
            var opt = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var data = JsonSerializer.Serialize(people, opt);
            File.WriteAllText(path, data);

        }

        public void Deserialize(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();

            var json = File.ReadAllText(path);
            var opt = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            people = JsonSerializer.Deserialize<List<Person>>(json, opt);
        }
    }
}