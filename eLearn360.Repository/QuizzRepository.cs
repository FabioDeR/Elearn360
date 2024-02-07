using elearn.Data.ViewModel.QuestionVM;
using eLearn360.Data.VM.ReportVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository
{
    public class QuizzRepository : Repository<Guid, Quizz>, IQuizzRepository
    {
        private readonly DbSet<Quizz> _entities;
        private readonly DbSet<Question> _entitiesQuestion;
        private readonly DbSet<SectionHasLesson> _entitiesSectionHasLesson;
        private readonly DbSet<CourseHasSection> _entitiesCourseHasSection;
        private readonly DbSet<PathWayHasCourse> _entitiesPathWayHasCourse;
        private readonly DbSet<UserHasGroup> _entitiesUserHasGroup;
        private readonly Elearn360DBContext _context;

        public QuizzRepository(Elearn360DBContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<Quizz>();
            _entitiesQuestion = _context.Set<Question>();
            _entitiesSectionHasLesson = _context.Set<SectionHasLesson>();
            _entitiesCourseHasSection = _context.Set<CourseHasSection>();
            _entitiesPathWayHasCourse = _context.Set<PathWayHasCourse>();
            _entitiesUserHasGroup = _context.Set<UserHasGroup>();
        }

        public override async Task<IEnumerable<Quizz>> Get()
        {
            try
            {
                IEnumerable<Quizz> quizzs = await _entities.Where(x => x.DeleteDate == null).ToListAsync();
                return quizzs;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #region Get By Id
        public override async Task<Quizz> GetById(Guid id)
        {
            try
            {
                Quizz quizz = await _entities.Where(x => x.DeleteDate == null && x.Id == id).FirstAsync();
                return quizz;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Quizz> GetByIdWithInclude(Guid id)
        {
            try
            {
                Quizz quizz = await _entities.Where(x => x.DeleteDate == null && x.Id == id).Include(x => x.QuizzHasQuestions)
                                                                                        .ThenInclude(x => x.Question)
                                                                                        .AsNoTracking()
                                                                                        .FirstOrDefaultAsync();
                return quizz;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region Get Report by UserId
        public async Task<List<QuizzReportVM>> GetReportByUserId(Guid userid)
        {
            try
            {
                
                List<QuizzReportVM> quizzReportVMs = new();
                List<Quizz> quizzs = await _entities.Where(x => x.DeleteDate == null && x.UserId == userid).Include(x => x.QuizzHasLessons).ThenInclude(x => x.Lesson).ThenInclude(x => x.Questions).ThenInclude(x => x.Level)
                                                                                                           .Include(x => x.QuizzHasSections).ThenInclude(x => x.Section).ThenInclude(x => x.Questions).ThenInclude(x => x.Level)
                                                                                                           .Include(x => x.QuizzHasCourses).ThenInclude(x => x.Course).ThenInclude(x => x.Questions).ThenInclude(x => x.Level)
                                                                                                           .Include(x => x.QuizzHasPathWays).ThenInclude(x => x.PathWay).ThenInclude(x => x.Questions).ThenInclude(x => x.Level)
                                                                                                           .Include(x => x.User)
                                                                                                           .AsNoTracking()
                                                                                                           .OrderBy(x => x.CreationDate)
                                                                                                           .ToListAsync();

                //Lesson
                List<QuizzReportVM> quizzReportVMLessons = quizzs.SelectMany(a => a.QuizzHasLessons).Where(x => x.DeleteDate == null).SelectMany(b => new List<QuizzReportVM>() { new QuizzReportVM()
                {
                    CreationDate = b.CreationDate,
                    QuizzId = b.QuizzId,
                    ItemId = b.LessonId,
                    ItemName = b.Lesson.Name,
                    Rating = (int)b.Quizz.Rating,
                    UserId = b.Quizz.UserId,
                    UserName = b.Quizz.User.FirstName,
                    levelName = b.Lesson.Questions.Select(s => s.Level.Name).FirstOrDefault(),
                    TypeEnum = Data.Enum.TypeEnum.Leçon
                } }).ToList();
                quizzReportVMs.AddRange(quizzReportVMLessons);

                //Section
                List<QuizzReportVM> quizzReportVMSections = quizzs.SelectMany(a => a.QuizzHasSections).Where(x => x.DeleteDate == null).SelectMany(b => new List<QuizzReportVM>() { new QuizzReportVM()
                {
                    CreationDate = b.CreationDate,
                    QuizzId = b.QuizzId,
                    ItemId = b.SectionId,
                    ItemName = b.Section.Name,
                    Rating = (int)b.Quizz.Rating,
                    UserId = b.Quizz.UserId,
                    UserName = b.Quizz.User.FirstName,
                    levelName = b.Section.Questions.Select(s => s.Level.Name).FirstOrDefault(),
                    TypeEnum = Data.Enum.TypeEnum.Section
                } }).ToList();
                quizzReportVMs.AddRange(quizzReportVMSections);

                //Course
                List<QuizzReportVM> quizzReportVMCourses = quizzs.SelectMany(a => a.QuizzHasCourses).Where(x => x.DeleteDate == null).SelectMany(b => new List<QuizzReportVM>() { new QuizzReportVM()
                {
                    CreationDate = b.CreationDate,
                    QuizzId = b.QuizzId,
                    ItemId = b.CourseId,
                    ItemName = b.Course.Name,
                    Rating = (int)b.Quizz.Rating,
                    UserId = b.Quizz.UserId,
                    UserName = b.Quizz.User.FirstName,
                    levelName = b.Course.Questions.Select(s => s.Level.Name).FirstOrDefault(),
                    TypeEnum = Data.Enum.TypeEnum.Cours
                } }).ToList();
                quizzReportVMs.AddRange(quizzReportVMCourses);


                List<QuizzReportVM> quizzReportVMPathWays = quizzs.SelectMany(a => a.QuizzHasPathWays).Where(x => x.DeleteDate == null).SelectMany(b => new List<QuizzReportVM>() { new QuizzReportVM()
                {
                    CreationDate = b.CreationDate,
                    QuizzId = b.QuizzId,
                    ItemId = b.PathWayId,
                    ItemName = b.PathWay.Name,
                    Rating = (int)b.Quizz.Rating,
                    UserId = b.Quizz.UserId,
                    UserName = b.Quizz.User.FirstName,
                    levelName = b.PathWay.Questions.Select(s => s.Level.Name).FirstOrDefault(),
                    TypeEnum = Data.Enum.TypeEnum.Parcours
            } }).ToList();
                quizzReportVMs.AddRange(quizzReportVMPathWays);


                return quizzReportVMs;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region Get Report by GroupId
        public async Task<List<QuizzReportVM>> GetReportByGroupId(Guid groupid)
        {
            try
            {
                //Get UserId In Group
                List<Guid> UserInGroupGuid = await _entitiesUserHasGroup.Where(x => x.DeleteDate == null && x.GroupId == groupid).Select(x => x.UserId).Distinct().ToListAsync();

                //Declare List of quizzReportVMs
                List<QuizzReportVM> quizzReportVMs = new();

                //For All User
                foreach(var userid in UserInGroupGuid)
                {
                    List<Quizz> quizzs = await _entities.Where(x => x.DeleteDate == null && x.UserId == userid).Include(x => x.QuizzHasLessons).ThenInclude(x => x.Lesson).ThenInclude(x => x.Questions).ThenInclude(x => x.Level)
                                                                                                           .Include(x => x.QuizzHasSections).ThenInclude(x => x.Section).ThenInclude(x => x.Questions).ThenInclude(x => x.Level)
                                                                                                           .Include(x => x.QuizzHasCourses).ThenInclude(x => x.Course).ThenInclude(x => x.Questions).ThenInclude(x => x.Level)
                                                                                                           .Include(x => x.QuizzHasPathWays).ThenInclude(x => x.PathWay).ThenInclude(x => x.Questions).ThenInclude(x => x.Level)
                                                                                                           .Include(x => x.User)
                                                                                                           .AsNoTracking()
                                                                                                           .OrderBy(x => x.CreationDate)
                                                                                                           .ToListAsync();

                    //Lesson
                    List<QuizzReportVM> quizzReportVMLessons = quizzs.SelectMany(a => a.QuizzHasLessons).Where(x => x.DeleteDate == null).SelectMany(b => new List<QuizzReportVM>() { new QuizzReportVM()
                {
                    CreationDate = b.CreationDate,
                    QuizzId = b.QuizzId,
                    ItemId = b.LessonId,
                    ItemName = b.Lesson.Name,
                    Rating = (int)b.Quizz.Rating,
                    UserId = b.Quizz.UserId,
                    UserName = b.Quizz.User.FirstName,
                    levelName = b.Lesson.Questions.Select(s => s.Level.Name).FirstOrDefault(),
                    TypeEnum = Data.Enum.TypeEnum.Leçon
                } }).ToList();
                    quizzReportVMs.AddRange(quizzReportVMLessons);

                    //Section
                    List<QuizzReportVM> quizzReportVMSections = quizzs.SelectMany(a => a.QuizzHasSections).Where(x => x.DeleteDate == null).SelectMany(b => new List<QuizzReportVM>() { new QuizzReportVM()
                {
                    CreationDate = b.CreationDate,
                    QuizzId = b.QuizzId,
                    ItemId = b.SectionId,
                    ItemName = b.Section.Name,
                    Rating = (int)b.Quizz.Rating,
                    UserId = b.Quizz.UserId,
                    UserName = b.Quizz.User.FirstName,
                    levelName = b.Section.Questions.Select(s => s.Level.Name).FirstOrDefault(),
                    TypeEnum = Data.Enum.TypeEnum.Section
                } }).ToList();
                    quizzReportVMs.AddRange(quizzReportVMSections);

                    //Course
                    List<QuizzReportVM> quizzReportVMCourses = quizzs.SelectMany(a => a.QuizzHasCourses).Where(x => x.DeleteDate == null).SelectMany(b => new List<QuizzReportVM>() { new QuizzReportVM()
                {
                    CreationDate = b.CreationDate,
                    QuizzId = b.QuizzId,
                    ItemId = b.CourseId,
                    ItemName = b.Course.Name,
                    Rating = (int)b.Quizz.Rating,
                    UserId = b.Quizz.UserId,
                    UserName = b.Quizz.User.FirstName,
                    levelName = b.Course.Questions.Select(s => s.Level.Name).FirstOrDefault(),
                    TypeEnum = Data.Enum.TypeEnum.Cours
                } }).ToList();
                    quizzReportVMs.AddRange(quizzReportVMCourses);


                    List<QuizzReportVM> quizzReportVMPathWays = quizzs.SelectMany(a => a.QuizzHasPathWays).Where(x => x.DeleteDate == null).SelectMany(b => new List<QuizzReportVM>() { new QuizzReportVM()
                {
                    CreationDate = b.CreationDate,
                    QuizzId = b.QuizzId,
                    ItemId = b.PathWayId,
                    ItemName = b.PathWay.Name,
                    Rating = (int)b.Quizz.Rating,
                    UserId = b.Quizz.UserId,
                    UserName = b.Quizz.User.FirstName,
                    levelName = b.PathWay.Questions.Select(s => s.Level.Name).FirstOrDefault(),
                    TypeEnum = Data.Enum.TypeEnum.Parcours
            } }).ToList();
                    quizzReportVMs.AddRange(quizzReportVMPathWays);
                }
                
                return quizzReportVMs;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region Delete Quizz Has Lesson
        public async Task RemoveQuizzHasLesson(QuizzHasLesson quizzHasLesson)
        {
            try
            {
                Quizz quizz = await _entities.Where(s => s.Id == quizzHasLesson.QuizzId).Include(qhl => qhl.QuizzHasLessons).SingleAsync();
                quizz.QuizzHasLessons.RemoveAll(x => x.LessonId == quizzHasLesson.LessonId && x.QuizzId == quizzHasLesson.QuizzId);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Delete Quizz Has Section
        public async Task RemoveQuizzHasSection(QuizzHasSection quizzHasSection)
        {
            try
            {
                Quizz quizz = await _entities.Where(s => s.Id == quizzHasSection.QuizzId).Include(qhs => qhs.QuizzHasSections).SingleAsync();
                quizz.QuizzHasSections.RemoveAll(x => x.SectionId == quizzHasSection.SectionId && x.QuizzId == quizzHasSection.QuizzId);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Delete Quizz Has Course
        public async Task RemoveQuizzHasCourse(QuizzHasCourse quizzHasCourse)
        {
            try
            {
                Quizz quizz = await _entities.Where(s => s.Id == quizzHasCourse.QuizzId).Include(qhc => qhc.QuizzHasCourses).SingleAsync();
                quizz.QuizzHasCourses.RemoveAll(x => x.CourseId == quizzHasCourse.CourseId && x.QuizzId == quizzHasCourse.QuizzId);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Delete Quizz Has PathWay
        public async Task RemoveQuizzHasPathWay(QuizzHasPathWay quizzHasPathWay)
        {
            try
            {
                Quizz quizz = await _entities.Where(s => s.Id == quizzHasPathWay.QuizzId).Include(qhp => qhp.QuizzHasPathWays).SingleAsync();
                quizz.QuizzHasPathWays.RemoveAll(x => x.PathWayId == quizzHasPathWay.PathWayId && x.QuizzId == quizzHasPathWay.QuizzId);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Quizz Generation
        public async Task<QuizzVM> QuizzGeneration(QuizzVM quizzVM)
        {
            try
            {
                List<Question> questions = new();
                List<Question> questionLessons = new();
                List<Question> questionSections = new();
                List<Question> questionCourses = new();
                List<Question> questionPaths = new();
                List<Guid> courseGuids = new();
                List<Guid> sectionGuids = new();
                List<Guid> lessonGuids = new();
                Quizz quizz = new()
                {
                    Id = Guid.NewGuid(),
                    Rating = 0,
                    UserId = quizzVM.UserId
                };

                //Create a record in the desired relationship table (enum)
                switch ((int)quizzVM.TypeEnum)
                {
                    case 1:

                        quizz.QuizzHasLessons = new();
                        QuizzHasLesson quizzHasLesson = new()
                        {
                            QuizzId = quizz.Id,
                            LessonId = quizzVM.Idx
                        };
                        quizz.QuizzHasLessons.Add(quizzHasLesson);
                        questionLessons = await _entitiesQuestion.Where(x => x.DeleteDate == null && x.Lessons.Any(x => x.Id == quizzVM.Idx) && x.LevelId == quizzVM.LevelId).Include(x => x.Answers).AsNoTracking().ToListAsync();

                        break;

                    case 2:

                        quizz.QuizzHasSections = new();
                        QuizzHasSection quizzHasSection = new()
                        {
                            QuizzId = quizz.Id,
                            SectionId = quizzVM.Idx
                        };
                        quizz.QuizzHasSections.Add(quizzHasSection);

                        //List of Question With Section Id
                        questionSections = await _entitiesQuestion.Where(x => x.DeleteDate == null && x.Sections.Any(x => x.Id == quizzVM.Idx) && x.LevelId == quizzVM.LevelId).Include(x => x.Answers).AsNoTracking().ToListAsync();

                        //List of all lessonid by section Id
                        lessonGuids = await _entitiesSectionHasLesson.Where(x => x.DeleteDate == null && x.SectionId == quizzVM.Idx).Select(x => x.LessonId).ToListAsync();
                        //List of question By lessonid
                        foreach (var lessonid in lessonGuids)
                        {
                            questionLessons.AddRange(await _entitiesQuestion.Where(x => x.Lessons.Any(x => x.Id == lessonid)).Include(x => x.Answers).AsNoTracking().ToListAsync());
                        }
                        break;

                    case 3:

                        quizz.QuizzHasCourses = new();
                        QuizzHasCourse quizzHasCourse = new()
                        {
                            QuizzId = quizz.Id,
                            CourseId = quizzVM.Idx
                        };
                        quizz.QuizzHasCourses.Add(quizzHasCourse);

                        //List of Question with Course ID
                        questionCourses = await _entitiesQuestion.Where(x => x.DeleteDate == null && x.Courses.Any(x => x.Id == quizzVM.Idx) && x.LevelId == quizzVM.LevelId).Include(x => x.Answers).AsNoTracking().ToListAsync();
                        //List of all section of courses
                        sectionGuids = await _entitiesCourseHasSection.Where(x => x.DeleteDate == null && x.CourseId == quizzVM.Idx).Select(x => x.SectionId).ToListAsync();
                        foreach (var sectionid in sectionGuids)
                        {
                            questionSections.AddRange(await _entitiesQuestion.Where(x => x.Sections.Any(x => x.Id == sectionid)).Include(x => x.Answers).AsNoTracking().ToListAsync());

                            List<Guid> LessonGuids = await _entitiesSectionHasLesson.Where(x => x.DeleteDate == null && x.SectionId == sectionid).Select(x => x.LessonId).ToListAsync();
                            foreach (var lessonid in LessonGuids)
                            {
                                questionLessons.AddRange(await _entitiesQuestion.Where(x => x.Lessons.Any(x => x.Id == lessonid)).Include(x => x.Answers).AsNoTracking().ToListAsync());
                            }
                        }
                        break;

                    case 4:
                        quizz.QuizzHasPathWays = new();
                        QuizzHasPathWay quizzHasPathWay = new()
                        {
                            QuizzId = quizz.Id,
                            PathWayId = quizzVM.Idx
                        };
                        quizz.QuizzHasPathWays.Add(quizzHasPathWay);
                        await _entities.AddAsync(quizz);

                        //List of Question With PathId
                        questionPaths = await _entitiesQuestion.Where(x => x.DeleteDate == null && x.PathWays.Any(x => x.Id == quizzVM.Idx) && x.LevelId == quizzVM.LevelId).Include(x => x.Answers).AsNoTracking().ToListAsync();
                        //List of all Course By PathId
                        courseGuids = await _entitiesPathWayHasCourse.Where(x => x.DeleteDate == null && x.PathWayId == quizzVM.Idx).Select(x => x.CourseId).ToListAsync();
                        foreach (var courseid in courseGuids)
                        {
                            questionCourses.AddRange(await _entitiesQuestion.Where(x => x.DeleteDate == null && x.Courses.Any(x => x.Id == courseid)).Include(x => x.Answers).AsNoTracking().ToListAsync());
                            //List of all section of courses
                            sectionGuids = await _entitiesCourseHasSection.Where(x => x.DeleteDate == null && x.CourseId == courseid).Select(x => x.SectionId).ToListAsync();
                            foreach (var sectionid in sectionGuids)
                            {
                                questionSections.AddRange(await _entitiesQuestion.Where(x => x.Sections.Any(x => x.Id == sectionid)).Include(x => x.Answers).AsNoTracking().ToListAsync());

                                List<Guid> LessonGuids = await _entitiesSectionHasLesson.Where(x => x.DeleteDate == null && x.SectionId == sectionid).Select(x => x.LessonId).ToListAsync();
                                foreach (var lessonid in LessonGuids)
                                {
                                    questionLessons.AddRange(await _entitiesQuestion.Where(x => x.Lessons.Any(x => x.Id == lessonid)).Include(x => x.Answers).AsNoTracking().ToListAsync());
                                }
                            }
                        }
                        break;
                }

                //Save quizz(without questions) if: enough questions, else : return quizzVM without save
                int NbTotalQuestions = questionCourses.Count() + questionPaths.Count() + questionLessons.Count() + questionSections.Count();
                if (NbTotalQuestions >= quizzVM.nbQuestion)
                {
                    await _entities.AddAsync(quizz);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return quizzVM;
                }

                //List of questions with Rnd and Probabilities
                questions = await GetPropabilities((int)quizzVM.TypeEnum, quizzVM.nbQuestion, questionLessons, questionSections, questionCourses, questionPaths);
                questions = await ShuffleList(questions);
                quizz.Questions = new();
                quizz.Questions = questions;
                quizzVM = await TransformToVM(quizz, quizzVM);

                return quizzVM;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #region Get Probabilities
        public async Task<List<Question>> GetPropabilities(int type, int nbQuestion, List<Question> questionLessons, List<Question> questionSections, List<Question> questionCourses, List<Question> questionPaths)
        {
            try
            {
                Dictionary<string, double> dictioProbality = new();
                switch (type)
                {
                    case 1:
                        dictioProbality.Add("Lesson", 1);

                        if (dictioProbality["Lesson"] <= 1)
                        {
                            int nbQuestionLesson = (int)Math.Round(nbQuestion * 1.0, MidpointRounding.AwayFromZero);
                            questionLessons = await GetRandomQuestion(questionLessons, nbQuestionLesson);
                        }
                        break;

                    case 2:
                        dictioProbality.Add("Section", 0.7);
                        dictioProbality.Add("Lesson", 0.3);

                        if (dictioProbality["Section"] <= 0.7)
                        {
                            int nbQuestionSection = (int)Math.Round(nbQuestion * 0.7, MidpointRounding.AwayFromZero);
                            questionSections = await GetRandomQuestion(questionSections, nbQuestionSection);

                        }
                        if (dictioProbality["Lesson"] <= 0.3)
                        {
                            int nbQuestionLesson = (int)Math.Round(nbQuestion * 0.3, MidpointRounding.AwayFromZero);
                            questionLessons = await GetRandomQuestion(questionLessons, nbQuestionLesson);
                        }
                        break;

                    case 3:
                        dictioProbality.Add("Course", 0.5);
                        dictioProbality.Add("Section", 0.3);
                        dictioProbality.Add("Lesson", 0.2);

                        if (dictioProbality["Course"] <= 0.5)
                        {
                            int nbQuestionCourse = (int)Math.Round(nbQuestion * 0.5, MidpointRounding.AwayFromZero);
                            questionCourses = await GetRandomQuestion(questionCourses, nbQuestionCourse);

                        }
                        if (dictioProbality["Section"] <= 0.3)
                        {
                            int nbQuestionSection = (int)Math.Round(nbQuestion * 0.3, MidpointRounding.AwayFromZero);
                            questionSections = await GetRandomQuestion(questionSections, nbQuestionSection);

                        }
                        if (dictioProbality["Lesson"] <= 0.2)
                        {
                            int nbQuestionLesson = (int)Math.Round(nbQuestion * 0.2, MidpointRounding.AwayFromZero);
                            questionLessons = await GetRandomQuestion(questionLessons, nbQuestionLesson);
                        }
                        break;

                    case 4:
                        dictioProbality.Add("Path", 0.4);
                        dictioProbality.Add("Course", 0.3);
                        dictioProbality.Add("Section", 0.2);
                        dictioProbality.Add("Lesson", 0.1);

                        if (dictioProbality["Path"] <= 0.4)
                        {
                            int nbQuestionPath = (int)Math.Round(nbQuestion * 0.4, MidpointRounding.AwayFromZero);
                            questionPaths = await GetRandomQuestion(questionPaths, nbQuestionPath);
                        }
                        if (dictioProbality["Course"] <= 0.3)
                        {
                            int nbQuestionCourse = (int)Math.Round(nbQuestion * 0.3, MidpointRounding.AwayFromZero);
                            questionCourses = await GetRandomQuestion(questionCourses, nbQuestionCourse);
                        }
                        if (dictioProbality["Section"] <= 0.2)
                        {
                            int nbQuestionSection = (int)Math.Round(nbQuestion * 0.2, MidpointRounding.AwayFromZero);
                            questionSections = await GetRandomQuestion(questionSections, nbQuestionSection);
                        }
                        if (dictioProbality["Lesson"] <= 0.1)
                        {
                            int nbQuestionLesson = (int)Math.Round(nbQuestion * 0.1, MidpointRounding.AwayFromZero);
                            questionLessons = await GetRandomQuestion(questionLessons, nbQuestionLesson);
                        }
                        break;
                }

                //Add All List
                List<Question> finalList = new();
                finalList.AddRange(questionLessons);
                finalList.AddRange(questionSections);
                finalList.AddRange(questionCourses);
                finalList.AddRange(questionPaths);
                return finalList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region Get RandomQuestion
        public async Task<List<Question>> GetRandomQuestion(List<Question> questions, int nbQuestion)
        {
            try
            {
                List<Question> NewQuestionList = new();

                if (questions.Count < nbQuestion)
                {
                    nbQuestion = questions.Count;
                }

                for (int i = 0; i < nbQuestion; i++)
                {
                    var rnd = new Random();
                    List<Question> listRnd = questions.OrderBy(item => rnd.Next()).ToList();
                    int rndindex = rnd.Next(listRnd.Count);
                    Question question = listRnd[rndindex];
                    NewQuestionList.Add(question);
                    questions.Remove(question);
                }

                return NewQuestionList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #region Shuffle List
        public async Task<List<Question>> ShuffleList(List<Question> questions)
        {
            var rnd = new Random();
            List<Question> listRnd = questions.OrderBy(item => rnd.Next()).ToList();
            return listRnd;
        }
        #endregion

        #region Transfom To VM
        public async Task<QuizzVM> TransformToVM(Quizz quizz, QuizzVM quizzVM)
        {
            try
            {
                //Determine QuizzId in VM
                quizzVM.QuizzId = quizz.Id;

                //Generate Question and Answers in VM
                List<QuestionVM> questionsVms = new();
                foreach (var q in quizz.Questions)
                {
                    QuestionVM questionVM = new()
                    {
                        Content = q.Content,
                        Explanation = q.Explanation,
                        ImageUrl = q.ImageUrl,
                        QuestionId = q.Id,
                        Title = q.Title
                    };

                    List<AnswerVM> ansers = new();
                    foreach (var a in q.Answers)
                    {
                        AnswerVM answerVM = new()
                        {
                            AnswerId = a.Id,
                            Content = a.Content,
                            Position = a.Position,
                            IsSelected = false,
                            IsCorrect = a.IsCorrect
                        };
                        ansers.Add(answerVM);
                    }
                    questionVM.AnswerVMs = ansers;
                    questionsVms.Add(questionVM);
                }

                quizzVM.QuestionVMs = questionsVms;

                return quizzVM;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        #endregion

        #region Update Quizz Rating
        public async Task<QuizzVM> UpdateQuizzRating(QuizzVM quizzvm)
        {
            try
            {
                List<QuizzHasQuestion> qhqs = new();
                List<Question> questions = new();
                List<BadAnswerVM> badAnswerVMs = new();
                Quizz quizz = await _entities.Where(x => x.DeleteDate == null && x.Id == quizzvm.QuizzId && x.UserId == quizzvm.UserId).FirstOrDefaultAsync();
                bool isCorrect = false;
                int GoodAnswerThisQuestion = 0;
                int GoodAnswerThisQuestionStudent = 0;
                double totalGoodAnswers = 0;
                double totalGoodAnswersStudent = 0;

                foreach (var q in quizzvm.QuestionVMs)
                {
                    //Count total Response(s) in this question + counter goodAnswerStudent for loop
                    GoodAnswerThisQuestion = q.AnswerVMs.Where(x => x.IsCorrect).Count();
                    GoodAnswerThisQuestionStudent = 0;
                    
                    //Create Question For add in quizz
                    Question question = await _entitiesQuestion.Where(x => x.DeleteDate == null && x.Id == q.QuestionId).AsNoTracking().FirstOrDefaultAsync();
                    questions.Add(question);

                    foreach (var a in q.AnswerVMs)
                    {
                        
                        //If Good Answer, increment counter
                        if (a.IsCorrect && a.IsSelected)
                        {
                            GoodAnswerThisQuestionStudent++;

                        }

                        //If bad answer GoodAnswerThisQuestionStudent = 0 & exit answers loop
                        if (a.IsSelected && a.IsCorrect == false)
                        {
                            GoodAnswerThisQuestionStudent = 0;
                            break;
                        }
                    }

                    //For this question,
                    //if != beetweenn GoodAnswerThisQuestion != GoodAnswerThisQuestionStudent, create Bad Answer to VM + Add false in QuizzHasQuestion
                    //else Add true in QuizzHasQuestion
                    if (GoodAnswerThisQuestion != GoodAnswerThisQuestionStudent)
                    {
                        BadAnswerVM badAnswerVM = new()
                        {
                            Explaination = q.Explanation,
                            GoodAnswer = q.AnswerVMs.Where(x => x.IsCorrect == true).Select(x => x.Content).ToList(),
                            Question = q.Content,
                            StudentAnswer = q.AnswerVMs.Where(x => x.IsSelected == true).Select(x => x.Content).ToList(),
                        };

                        badAnswerVMs.Add(badAnswerVM);
                        isCorrect = false;
                    }
                    else
                    {
                        isCorrect = true;
                    }

                    //Create QuizzHasQuestion to insert in Quizz
                    QuizzHasQuestion qhq = new()
                    {
                        IsCorrectAnswer = isCorrect,
                        QuestionId = q.QuestionId,
                        QuizzId = q.QuestionId
                    };

                    qhqs.Add(qhq);

                    //After each question, increment the counters
                    totalGoodAnswers = totalGoodAnswers + GoodAnswerThisQuestion;
                    totalGoodAnswersStudent = totalGoodAnswersStudent + GoodAnswerThisQuestionStudent;
                }

                //Update Quizz
                //Arround +1
                quizz.Rating = (int)Math.Ceiling((100 / totalGoodAnswers) * totalGoodAnswersStudent);
                if(quizz.Rating <= 0)
                {
                    quizz.Rating = 0;
                }

                //Update Question with QuizzHasQuestionList
                quizz.Questions = questions;
                quizz.QuizzHasQuestions = qhqs;
                _entities.Update(quizz);
                await _context.SaveChangesAsync();

                //Update VM For get Rating in Front
                quizzvm.Rating = (int)quizz.Rating;
                quizzvm.BadAnswerVMs = badAnswerVMs;

                return quizzvm;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion
    }

}
