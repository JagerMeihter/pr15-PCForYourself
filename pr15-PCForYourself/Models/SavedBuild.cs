using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace pr15_PCForYourself.Models
{
    public class SavedBuild
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public Dictionary<ComponentCategory, PCComponent> Components { get; set; }
        public decimal TotalPrice { get; set; }
    }
}