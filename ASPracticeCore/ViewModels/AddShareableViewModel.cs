using ASPracticeCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPracticeCore.ViewModels
{
    public class AddShareableViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please set a title.")]
        [MaxLength(100, ErrorMessage = "Maximum of 100 characters for Title.")]
        public string Title { get; set; }

        [MaxLength(1000, ErrorMessage = "Maximum of 1000 characters for Introduction")]
        public string Introduction { get; set; }

        public virtual List<Paragraph> Paragraphs { get; set; }
        
        [Required]
        public int UserAccountId { get; set; }


    }
}
