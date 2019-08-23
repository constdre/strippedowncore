using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPracticeCore.Models
{
    public class Shareable:EntityBase
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Introduction { get; set; }
        public List<Paragraph> Paragraphs { get; set; }
        public DateTime DateTimeStamp { get; set; }

        public class Paragraph:EntityBase
        {
            public int ShareableId { get; set; }
            public string Text { get; set; }
            
        }
    }
}
