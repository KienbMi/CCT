using CCT.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CCT.Core
{
    public class EntityObject : IEntityObject
    {
        [Key]
        public int Id { get; set; }

        [Timestamp]
        public byte[] RowVersion
        {
            get;
            set;
        }
    }
}
