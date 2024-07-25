using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreWithEntity.Entities
{
    internal class Teacher : BaseEntity
    {
      public string Name { get; set; }
      public string Surname { get; set; }
      public ICollection<Group>groups { get; set; }
    }
}
