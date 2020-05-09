using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPracticeCore.Models
{
    public class Paragraph : IEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int ShareableId { get; set; }
        public virtual Shareable Shareable { get; set; }

    }
}
