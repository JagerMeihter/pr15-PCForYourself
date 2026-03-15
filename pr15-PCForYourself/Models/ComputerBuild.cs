using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace pr15_PCForYourself.Models
{
    public class ComputerBuild
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public Dictionary<ComponentCategory, PCComponent> SelectedComponents { get; } = new Dictionary<ComponentCategory, PCComponent>();

        public decimal TotalPrice
        {
            get
            {
                return SelectedComponents.Values
                    .Where(c => c != null)
                    .Sum(c => c.Price);
            }
        }
    }
}
