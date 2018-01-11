using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gamesdatabasetwo.Data.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int Votes { get; set; }
        public double Score { get; set; }
    }
}
