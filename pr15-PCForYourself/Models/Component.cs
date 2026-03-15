using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace pr15_PCForYourself.Models
{
    public class Component
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public decimal Price { get; set; }
        public ComponentCategory Category { get; set; }

        public string Socket { get; set; }                  // CPU, Motherboard, Cooler
        public List<string> SupportedSockets { get; set; } // Cooler
        public string FormFactor { get; set; }              // Motherboard, Case
        public List<string> SupportedFormFactors { get; set; } // Case
        public string MemoryType { get; set; }              // Motherboard, RAM
        public int? PowerConsumption { get; set; }           // CPU, GPU (int? оставляем, это значимый тип)
        public int? PowerCapacity { get; set; }              // PSU
    }
}