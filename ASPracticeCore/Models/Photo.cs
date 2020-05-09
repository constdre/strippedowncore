﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPracticeCore.Models
{
    public class Photo:IEntity
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public int Size { get; set; }
        public string FileType { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}