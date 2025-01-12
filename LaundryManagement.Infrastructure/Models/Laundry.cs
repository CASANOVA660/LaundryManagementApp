using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace LaundryManagement.Infrastructure.Models
{
    public class Laundry
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int IdProprietaire { get; set; }
        public Owner Proprietaire { get; set; }
        public ICollection<Machine> Machines { get; set; }
    }
}
