﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInWpf.Models
{
   public class UserModel
    {
        public string Fullname { get; set; }
        public int Age { get; set; }
        public byte[] ImageBytes { get; set; }
    }
}
