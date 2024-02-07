using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository.Interfaces
{
    public interface ISectionRepository : IRepository<Guid, Section>
    {
        Task<List<Section>> GetPrivateSection(Guid sectionid);
        Task<List<Section>> GetPublicSection(Guid sectionid);
        Task DuplicateSection(Guid userid, Guid sectionid);
        Task UpdateOrDeleted(List<SectionHasLesson> sectionHasLessons);
        Task RemoveSectionHaslesson(SectionHasLesson sectionHasLesson);
        Task<List<SectionHasLesson>> GetIncludeSectionHasLesson(Guid sectionid);
        Task<List<Guid>> GetLessonGuid(Guid sectionid);
        Task<List<Section>> GetSectionByPathid(Guid pathId, Guid userId);
    }
}
