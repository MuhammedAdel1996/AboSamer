﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Helper
{
    public class Jwt
    {
        public string Secret { get; set; }
        public string Iss { get; set; }
        public string Aud { get; set; }
    }
}
