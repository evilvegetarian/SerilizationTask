namespace SerilizationTask.Application.Services
{
    public interface IPersonService
    {
        void Clear();
        void DeserializeInFile(string path);
        void GeneratePersons(int count);
        StatisticDisplay GetStatisticDisplay();
        void SerializeInFile(string path);
    }
}