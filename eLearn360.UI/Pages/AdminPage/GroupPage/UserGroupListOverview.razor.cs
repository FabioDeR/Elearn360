namespace eLearn360.UI.Pages.AdminPage.GroupPage
{
    public partial class UserGroupListOverview
    {
        [Inject] public IGroupService GroupService { get; set; }
        [Parameter] public Guid GroupId { get; set; }

        public Group StudentGroup { get; set; } = new();
        public Group TeacherGroup { get; set; } = new();
        public UserHasGroup UserHasGroup { get; set; } = new();
        private DateTime _today = DateTime.Now;
        private bool _loading;

        protected override async Task OnInitializedAsync()
        {
            _loading = true;
            await LoadList();
            _loading = false;
        }

        protected async Task LoadList()
        {
            StudentGroup = await GroupService.GetStudentByGroupId(GroupId);
            TeacherGroup = await GroupService.GetTeacherByGroupId(GroupId);
            StateHasChanged();
        }

        protected async Task RemoveUser(Guid userId)
        {
            UserHasGroup = new()
            {
                UserId = userId,
                GroupId = GroupId,
                EndDate = _today
            };

            var res = await GroupService.RemoveUserHasGroup(UserHasGroup);
            if (res.IsSuccessStatusCode)
            {
                await LoadList();                
            }

        }
    }
}
