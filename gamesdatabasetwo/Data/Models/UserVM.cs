﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gamesdatabasetwo.Data
{
    public class UserVM
    {
        public string Email { get; set; }
        public IList<string> Role { get; set; }

    }
}
