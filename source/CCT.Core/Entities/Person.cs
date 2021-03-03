using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CCT.Core.Entities
{
    public class Person : EntityObject
    {
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        [MaxLength(100)]
        public string PhoneNumber { get; set; }
        public DateTime RecordTime { get; set; }
        public bool IsVaccinated { get; set; }
        public DateTime LastTested { get; set; }
        [NotMapped]
        public string LastTestedDate=> LastTested != DateTime.MinValue ? LastTested.ToString("dd.MM.yyyy HH:mm") : "---";
    }
}
