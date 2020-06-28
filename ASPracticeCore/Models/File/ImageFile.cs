using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPracticeCore.Models
{
    public class ImageFile: IEntity
    {
        public int Id { get; set; }
        public bool? IsDisplayPic { get; set; }
        public long ImageSize { get; set; }
        public virtual FilePath FilePath { get; set; }

    }
}
