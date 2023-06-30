using SerilizationTask.Domain.Enums;

namespace SerilizationTask.Domain.Models
{
    public class Child
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public long BirthDate { get; set; }
        public Gender Gender { get; set; }
    }

}