using System.ComponentModel.DataAnnotations.Schema;

namespace eLearn360.Data.Models
{
    public class SectionHasLesson : BaseModel
    {
        public int Position { get; set; }

        [ForeignKey("LessonId")]
        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; }

        [ForeignKey("SectionId")]
        public Guid SectionId { get; set; }

        public Section Section { get; set; }
    }
}