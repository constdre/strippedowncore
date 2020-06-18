﻿using ASPracticeCore.Areas.Accounts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASPracticeCore.Models
{
    public class Shareable : IEntity
    {
        public Shareable()
        {
            Paragraphs = new List<Paragraph>();
            FilePaths = new List<FilePath>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Introduction { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public int UserAccountId { get; set; }

        public virtual UserAccount UserAccount { get; set; }
        public virtual ICollection<Paragraph> Paragraphs { get; set; }
        public virtual ICollection<FilePath> FilePaths { get; set; }


    }
}
