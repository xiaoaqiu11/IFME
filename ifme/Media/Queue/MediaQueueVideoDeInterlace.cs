﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ifme
{
    public class MediaQueueVideoDeInterlace
    {
        public bool Enable { get; set; } = false;
        public int Field { get; set; } = 1;
        public int Mode { get; set; } = 0;
    }
}
