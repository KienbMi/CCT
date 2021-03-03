using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CCT.WebAPI.DataTransferObjects
{
    public class PersonDto
    {
        [DisplayName("Vorname")]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [DisplayName("Nachname")]
        [MaxLength(100)]
        public string LastName { get; set; }
        [DisplayName("Telefonnummer")]
        [MaxLength(100)]
        public string PhoneNumber { get; set; }
        [DisplayName("Datum")]
        public DateTime RecordTime { get; set; }
        [DisplayName("Ist geimpft")]
        public bool IsVaccinated { get; set; }
        public DateTime LastTested { get; set; }
    }
}
