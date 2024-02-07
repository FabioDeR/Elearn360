using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Elearn360DBContext _context;
        private readonly UserManager<User> _usermanager;
        private ICategoryRepository _categoryRepository;
        private ICourseRepository _courseRepository;
        private IFamilyLinkRepository _familylinkRepository;
        private IFilialRepository _filialRepository;
        private IGenderRepository _genderRepository;
        private IGroupRepository _groupRepository;
        private ILessonRepository _lessonRepository;
        private ILevelRepository _levelRepository;
        private IOccupationRepository _occupationRepository;
        private IOrganizationRepository _organizationRepository;
        private IUserRepository _userRepository;
        private IPathWayRepository _pathWayRepository;
        private ISectionRepository _sectionRepository;
        private IStaffOccupationRepository _staffOccupationRepository;
        private IQuestionRepository _questionRepository;
        private IAnswerRepository _answerRepository;
        private IQuizzRepository _quizzRepository;

        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (this._categoryRepository == null)
                {
                    this._categoryRepository = new CategoryRepository(_context);
                }
                return this._categoryRepository;
            }
        }

        public ICourseRepository CourseRepository
        {
            get
            {
                if (this._courseRepository == null)
                {
                    this._courseRepository = new CourseRepository(_context);
                }
                return this._courseRepository;
            }
        }

        public IFamilyLinkRepository FamilyLinkRepository
        {

            get
            {
                if (this._familylinkRepository == null)
                {
                    this._familylinkRepository = new FamilyLinkRepository(_context);
                }
                return this._familylinkRepository;
            }
        }

        public IFilialRepository FilialRepository
        {

            get
            {
                if (this._filialRepository == null)
                {
                    this._filialRepository = new FilialRepository(_context);
                }
                return this._filialRepository;
            }
        }
        public IGenderRepository GenderRepository
        {
            get
            {
                if (this._genderRepository == null)
                {
                    this._genderRepository = new GenderRepository(_context);
                }
                return this._genderRepository;
            }
        }

        public IGroupRepository GroupRepository
        {
            get
            {
                if (this._groupRepository == null)
                {
                    this._groupRepository = new GroupRepository(_context);
                }
                return this._groupRepository;
            }
        }
        public ILessonRepository LessonRepository
        {
            get
            {
                if (this._lessonRepository == null)
                {
                    this._lessonRepository = new LessonRepository(_context);
                }
                return this._lessonRepository;
            }
        }

        public ILevelRepository LevelRepository
        {
            get
            {
                if (this._levelRepository == null)
                {
                    this._levelRepository = new LevelRepository(_context);
                }
                return this._levelRepository;
            }
        }

        public IOccupationRepository OccupationRepository
        {
            get
            {
                if (this._occupationRepository == null)
                {
                    this._occupationRepository = new OccupationRepository(_context);
                }
                return this._occupationRepository;
            }
        }

        public IOrganizationRepository OrganizationRepository
        {
            get
            {
                if (this._organizationRepository == null)
                {
                    this._organizationRepository = new OrganizationRepository(_context,_usermanager);
                }
                return this._organizationRepository;
            }
        }

        public IPathWayRepository PathWayRepository
        {
            get
            {
                if (this._pathWayRepository == null)
                {
                    this._pathWayRepository = new PathWayRepository(_context);
                }
                return this._pathWayRepository;
            }
        }

        public ISectionRepository SectionRepository
        {
            get
            {
                if (this._sectionRepository == null)
                {
                    this._sectionRepository = new SectionRepository(_context);
                }
                return this._sectionRepository;
            }
        }



        public IUserRepository UserRepository
        {
            get
            {
                if (this._userRepository == null)
                {
                    this._userRepository = new UserRepository(_context,_usermanager);
                }
                return this._userRepository;
            }
        }

        public IStaffOccupationRepository StaffOccupationRepository
        {
            get
            {
                if (this._staffOccupationRepository == null)
                {
                    this._staffOccupationRepository = new StaffOccupationRepository(_context);
                }
                return this._staffOccupationRepository;
            }
        }

        public IQuestionRepository QuestionRepository
        {
            get
            {
                if (this._questionRepository == null)
                {
                    this._questionRepository = new QuestionRepository(_context);
                }
                return this._questionRepository;
            }
        }

        public IAnswerRepository AnswerRepository
        {
            get
            {
                if (this._answerRepository == null)
                {
                    this._answerRepository = new AnswerRepository(_context);
                }
                return this._answerRepository;
            }
        }

        public IQuizzRepository QuizzRepository
        {
            get
            {
                if (this._quizzRepository == null)
                {
                    this._quizzRepository = new QuizzRepository(_context);
                }
                return this._quizzRepository;
            }
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public UnitOfWork(Elearn360DBContext context,UserManager<User> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }
    }
}
