using System.ComponentModel.DataAnnotations.Schema;

namespace eLearn360.Data.Models
{
    public class CourseHasSection : BaseModel
    {
        public int Position { get; set; }

        [ForeignKey("CourseId")]
        public Guid CourseId { get; set; }
        public Course Course { get; set; }

        [ForeignKey("SectionId")]
        public Guid SectionId { get; set; }

        public Section Section { get; set; }
    }
}