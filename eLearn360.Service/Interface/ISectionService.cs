using eLearn360.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Service.Interface
{
    public interface ISectionService
    {
        Task<Guid> Add(Section section);
        Task<HttpResponseMessage> DuplicateSection(Guid sectionid, Guid userid);
        Task<Section> GetById(Guid id);
        Task<List<Section>> GetPrivateSection(Guid userId);
        Task<List<Section>> GetPublicSection(Guid userId);
        Task Update(Section section);
        Task<List<Guid>> GetLessonGuid(Guid sectionId);
        Task<List<SectionHasLesson>> GetIncludeSectionHasLesson(Guid sectionId);
        Task UpdateOrDelete(List<SectionHasLesson> sectionHasLessons);
        Task RemoveSectionHasLesson(SectionHasLesson sectionHasLesson);
        Task<List<Section>> GetSectionByPathId(Guid pathId, Guid userId);
    }
}
