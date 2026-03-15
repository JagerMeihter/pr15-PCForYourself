using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace pr15_PCForYourself.Models
{
    public class PCComponent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public decimal Price { get; set; }
        public ComponentCategory Category { get; set; }

        public string Socket { get; set; }
        public List<string> SupportedSockets { get; set; }
        public string FormFactor { get; set; }
        public List<string> SupportedFormFactors { get; set; }
        public string MemoryType { get; set; }
        public int? PowerConsumption { get; set; }
        public int? PowerCapacity { get; set; }
    }
}