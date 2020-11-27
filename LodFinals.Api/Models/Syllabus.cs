using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LodFinals.Api.Models
{
    public class Syllabus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Exercise> Exercises { get; }
    }
}
