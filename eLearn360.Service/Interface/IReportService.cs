using eLearn360.Data.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Service.Interface
{
    public interface IReportService
    {
        Task<LessonReportVM> StudentCourseReport(Guid userid, Guid groupid);
    }
}
