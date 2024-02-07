namespace eLearn360.UI.Pages.ProfessorPage.GroupPage
{
    public partial class StudentListOverview
    {
        [Inject] public IGroupService GroupService { get; set; }
        [Parameter] public Guid GroupId { get; set; }

        public Group StudentGroup { get; set; } = new();
        public UserHasGroup UserHasGroup { get; set; } = new();

        private bool _isLoading;

        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            await LoadList();
            _isLoading = false;
        }

        protected async Task LoadList()
        {
            StudentGroup = await GroupService.GetStudentByGroupId(GroupId);
            StateHasChanged();
        }

    }
}
