namespace eLearn360.UI.Pages.AdminPage.GroupPage
{
    public partial class OccupationModal
    {
        [Parameter] public string Title { get; set; }
        [Parameter] public Guid GroupId { get; set; }
        [Parameter] public Guid UserId { get; set; }
        [Parameter] public Guid OrganizationId { get; set; }
        [Parameter] public EventCallback<bool> OnClose { get; set; }
   
        [Inject] public IGroupService GroupService { get; set; }
        [Inject] public IStaffHasOccupationService StaffHasOccupationService { get; set; }

        protected List<StaffOccupation> StaffOccupationList { get; set; } = new();
        public List<Guid> UserOccupationList { get; set; } = new();
        protected List<StaffHasOccupationHasGroup> StaffHasOccupationHasGroups { get; set; } = new();
        protected StaffHasOccupationHasGroup StaffHasOccupationHasGroup { get; set; } = new();
        protected List<UserHasGroup> UserHasGroups { get; set; } = new();
        protected UserHasGroup UserHasGroup { get; set; } = new();
        protected Group Group { get; set; } = new();

        private bool _isLoading = false;
        protected bool isHeadTeacher = false;
        private DateTime _today = DateTime.Now;

        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            await LoadList();
            _isLoading = false;
        }

        private async Task LoadList()
        {

            Group = await GroupService.GetById(GroupId);
            StaffOccupationList = await StaffHasOccupationService.GetByOrganizationId(OrganizationId);
        }

        private Task ModalCancel()
        {
            return OnClose.InvokeAsync(false);
        }

        private async Task<Task> ModalOk()
        {
            UserHasGroup = new()
            {
                GroupId = GroupId,
                UserId = UserId,
                StartDate = _today,
                IsHeadTeacher = isHeadTeacher,
                StaffHasOccupationHasGroups = new List<StaffHasOccupationHasGroup>(StaffHasOccupationHasGroups)
            };

            foreach (var occupationGuid in UserOccupationList)
            {
                StaffHasOccupationHasGroup = new()
                {
                    StaffOccupationId = occupationGuid
                };

                UserHasGroup.StaffHasOccupationHasGroups.Add(StaffHasOccupationHasGroup);
            }
            
            UserHasGroups.Add(UserHasGroup);

            Group.UserHasGroups = new List<UserHasGroup>(UserHasGroups);

            await UpdateList();

            return OnClose.InvokeAsync(true);
        }

        private async Task UpdateList()
        {
            await GroupService.Update(Group);
        }
    }
}
