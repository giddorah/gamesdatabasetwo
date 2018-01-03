using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gamesdatabasetwo.Data
{
    public class ViewGameModel
    {

        public string Name { get; set; }
        public int Year { get; set; }
        public string Platforms { get; set; }
        public string Theme { get; set; }
        public string Genre { get; set; }
        public string ReleasedWhere { get; set; }
        public Publisher Publisher { get; set; }
        public Developer Developer { get; set; }
    }
}
