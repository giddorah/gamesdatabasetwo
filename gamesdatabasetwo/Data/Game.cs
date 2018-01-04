using gamesdatabasetwo.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gamesdatabasetwo.Data
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string Platforms { get; set; }
        public string Theme { get; set; }
        public string Genre { get; set; }
        public string ReleasedWhere { get; set; }
        public Publisher Publisher { get; set; }
        public Developer Developer { get; set; }
        public Rating Score { get; set; }
        public int PublisherId { get; set; }
        public int DeveloperId { get; set; }
        public int ScoreId { get; set; }
    }
}
