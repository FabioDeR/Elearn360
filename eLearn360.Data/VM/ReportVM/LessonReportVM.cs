using eLearn360.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.VM
{
    public class LessonReportVM
    {
        public string Name { get; set; } = string.Empty;
        public List<ReportPathVM> ReportPaths { get; set; }
    }
    public class ReportPathVM
    {
        public PathVM PathVM { get; set; }
        public bool PathSeen { get; set; } = false;
        public int PercentComplete { get; set; }
        public List<ReportCourseVM> ReportCourses { get; set; }
    }

    public class ReportCourseVM
    {
        public CourseVM2 CourseVM { get; set; }
        public bool CourseSeen { get; set; } = false;
        public int PercentComplete { get; set; }
        public List<ReportSectionVM> ReportSections { get; set; } 
    }

    public class ReportSectionVM
    {
        public SectionVM2 SectionVM { get; set; }
        public bool SectionSeen { get; set; } = false;
        public List<ReportLessonVM> RapportLessons { get; set;}
    }

    public class ReportLessonVM
    {
        public LessonVM2 LessonVM { get; set; }
        public bool LessonSeen { get; set; } = false;
    }

    public class PathVM
    {
        public Guid PathId { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Img { get; set; } = string.Empty;

    }

    public class CourseVM2
    {
        public Guid CourseId { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Img { get; set; } = string.Empty;
    }

    public class SectionVM2
    {
        public Guid SectionId { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class LessonVM2
    {
        public Guid LessonId { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
