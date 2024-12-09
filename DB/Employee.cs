using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GSI_WebApi.DB
{
    [Table("Employees")]
    public partial class Employee
    {
        [Key]
        public string Guid { get; set; }
        public string Name { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastPasswordSet { get; set; }
        public int LoginCount { get; set; }
        public string? department { get; set; }
    }
}
