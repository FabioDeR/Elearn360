namespace eLearn360.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository CategoryRepository { get; }
        ICourseRepository CourseRepository { get; }
        IFamilyLinkRepository FamilyLinkRepository { get; }
        IFilialRepository FilialRepository { get; }
        IGenderRepository GenderRepository { get; }
        IGroupRepository GroupRepository { get; }
        ILessonRepository LessonRepository { get; }
        ILevelRepository LevelRepository { get; }
        IOccupationRepository OccupationRepository { get; }
        IOrganizationRepository OrganizationRepository { get; }
        IPathWayRepository PathWayRepository { get; }
        ISectionRepository SectionRepository { get; }  
        IStaffOccupationRepository StaffOccupationRepository { get; }
        IQuestionRepository QuestionRepository { get; }
        IAnswerRepository AnswerRepository { get; }
        IQuizzRepository QuizzRepository { get; }

        IUserRepository UserRepository { get; }
        Task<int> Complete();
        
    }
}