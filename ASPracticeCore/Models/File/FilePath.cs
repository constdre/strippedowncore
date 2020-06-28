using System;


namespace ASPracticeCore.Models
{
    public class FilePath : IEntity
    {
        public int Id { get; set; }
        public int ShareableId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public long FileSize { get; set; }
        public string Path { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual Shareable Shareable { get; set; }
        public virtual ImageFile ImageFile { get; set; }


    }
}
