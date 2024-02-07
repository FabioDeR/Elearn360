using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.VM.CourseVM
{
    public class ViewContentVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Description { get; set; } 
        public string ImageUrl { get; set; }

        public TypeOfContent TypeOfContent { get; set; }
    }

    public enum TypeOfContent
    {
        Course,
        Section,
        Lesson
    }
}
