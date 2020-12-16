using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CCT.Core.Entities
{
    public class Setting : EntityObject
    {
        [MaxLength(50)]
        public string Name { get; set; }
        public int Type { get; set; }
        [MaxLength(100)]
        public string Value { get; set; }
    }
}
