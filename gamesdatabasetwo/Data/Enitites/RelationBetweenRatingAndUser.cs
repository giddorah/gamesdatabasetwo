using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gamesdatabasetwo.Data.Enitites
{
    public class RelationBetweenRatingAndUser
    {
        public int id { get; set; }
        public string UserId { get; set; }
        public int RatingId { get; set; }
    }
}
