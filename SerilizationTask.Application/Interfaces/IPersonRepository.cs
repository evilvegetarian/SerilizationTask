namespace SerilizationTask.Application.Interfaces
{
    public interface IPersonRepository
    {
        void Clear();
        void Deserialize(string path);
        void GeneratePersons(int count);
        StatisticDisplay GetStatistic();
        void SerializeInFile(string path);
    }
}