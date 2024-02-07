using elearn.Data.ViewModel.QuestionVM;
using eLearn360.Data.Enum;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Security.Claims;

namespace eLearn360.UI.Pages.StudentPage
{
    public partial class QuizzEdit
    {
        [Inject] public IQuizzService QuizzService { get; set; }
        [Inject] public ILessonService LessonService { get; set; }
        [Inject] public ISectionService SectionService { get; set; }
        [Inject] public ICourseService CourseService { get; set; }
        [Inject] public IPathWayService PathService { get; set; }
        [Inject] public ILevelService LevelService { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        [Parameter] public string PathId { get; set; }
        public QuizzVM QuizzVM { get; set; } = new();

        //Listes
        public List<Course> Courses { get; set; } = new();
        public List<Section> Sections { get; set; } = new();
        public List<Lesson> Lessons { get; set; } = new();
        public List<Level> Levels { get; set; } = new();
        public List<PathWay> PathWays { get; set; } = new();
        public List<TypeEnum> TypeEnum { get; set; } = new();



        protected string Message { get; set; } = string.Empty;

        //Forms
        protected bool FormIsVisible = true;
        protected bool ScoreIsVisible { get; set; } = false;
        protected bool QuizzFinish { get; set; } = false;
        protected string ScoreClassColor { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = (await AuthenticationStateTask).User;
            if (!authenticationState.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/login");
            }
            else
            {
                await LoadList();
            }
        }

        private async Task LoadList()
        {
            TypeEnum = Enum.GetValues(typeof(TypeEnum)).Cast<TypeEnum>().ToList();
            var user = (await AuthenticationStateTask).User;
            var userId = user.FindFirst(e => e.Type == ClaimTypes.NameIdentifier)?.Value;
            QuizzVM.UserId = Guid.Parse(userId);

            Levels = (await LevelService.GetAll()).ToList();
            StateHasChanged();
        }

        #region OnSelectType
        protected async void OnSelectType(ChangeEventArgs e)
        {
            QuizzVM.TypeEnum = (TypeEnum)Convert.ToInt32(e.Value.ToString());
            Message = string.Empty;
            switch ((int)QuizzVM.TypeEnum)
            {
                case 1:
                    Lessons = (await LessonService.GetLessonByPathId(Guid.Parse(PathId), QuizzVM.UserId)).ToList();
                    break;
                case 2:
                    Sections = (await SectionService.GetSectionByPathId(Guid.Parse(PathId), QuizzVM.UserId)).ToList();
                    break;
                case 3:
                    Courses = (await CourseService.GetCourseByPathAndUserId(Guid.Parse(PathId), QuizzVM.UserId)).ToList();
                    break;
                case 4:
                    PathWays = (await PathService.GetPathByPathid(Guid.Parse(PathId), QuizzVM.UserId)).ToList();
                    break;
            }

            StateHasChanged();
        }
        #endregion

        #region GenerateQuizz
        protected async Task GenerateQuizz()
        {

            QuizzVM = await QuizzService.GenerateQuizz(QuizzVM);

            //Recuperer les questions
            QuizzVM = JsonConvert.DeserializeObject<QuizzVM>(JsonConvert.SerializeObject(QuizzVM));

            if (QuizzVM.QuestionVMs == null)
            {
                FormIsVisible = true;
                Message = "Il n'y pas de question relative à ce choix, veuillez changer votre sélection";
            }
            else
            {
                FormIsVisible = false;
            }

            StateHasChanged();
        }
        #endregion

        #region GetScore
        protected async void GetScore()
        {
            ScoreIsVisible = true;

            //Return on the top of the page
            //await JSRuntime.InvokeVoidAsync("OnScrollEvent");

            QuizzVM = await QuizzService.UpdateRating(QuizzVM);
            QuizzVM = JsonConvert.DeserializeObject<QuizzVM>(JsonConvert.SerializeObject(QuizzVM));

            //Faire apparaitre le formulaire de rating
            QuizzFinish = true;

            switch (QuizzVM.Rating)
            {
                case (<= 50):
                    ScoreClassColor = "text-danger";
                    break;
                case (<= 85):
                    ScoreClassColor = "text-warning";
                    break;
                case (<= 100):
                    ScoreClassColor = "text-success";
                    break;
            }
            StateHasChanged();
        }
        #endregion

        #region ReloadQuizz
        protected void ReloadQuizz()
        {
            FormIsVisible = true;
            ScoreIsVisible = false;
            QuizzFinish = false;
            QuizzVM = new();
            LoadList();
        }

        #endregion

        #region Cancel
        protected void Cancel()
        {
            NavigationManager.NavigateTo($"/");
        }

        #endregion


    }
}
