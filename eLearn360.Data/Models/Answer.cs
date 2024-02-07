using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class Answer : BaseModel
    {
        [Required(ErrorMessage = "Le contenu de la réponse est obligatoire")]
        [DataType(DataType.Text)]
        public string Content { get; set; } = String.Empty;

        [Range(0, 9, ErrorMessage = "Positions autorisées : 0-9")]
        public int Position { get; set; }

        public Boolean IsCorrect { get; set; } = false;

        [ForeignKey("QuestionId")]
        public Guid QuestionId { get; set; }

        public Question Question { get; set; }
    }
}
