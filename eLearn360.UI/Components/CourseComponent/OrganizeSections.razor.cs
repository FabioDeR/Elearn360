namespace eLearn360.UI.Components.CourseComponent
{
    public partial class OrganizeSections
    {
		[Inject] public NavigationManager NavigationManager { get; set; }
		[Inject] public ICourseService CourseService { get; set; }
		[Inject] protected IToaster Toaster { get; set; }
		[CascadingParameter] protected Guid CourseId { get; set; }

		public List<CourseHasSection> CourseHasSections { get; set; } = new();
		public List<CourseHasSection> Updated { get; set; } = new();
		public CourseHasSection Deleted { get; set; } = new CourseHasSection();

		protected int Counter { get; set; }
		public bool DeleteDialogOpen { get; set; }
		protected Guid DeleteId { get; set; }
        private bool _isLoading = false;

        protected override async Task OnInitializedAsync()
		{
            _isLoading = true;
            await LoadList();
            _isLoading = false;
        }
		#region UpdatePosition
		protected async Task UpdatePosition()
		{
			try
			{
				Updated = CourseHasSections;				
				await CourseService.UpdateOrDelete(Updated);				
				NavigationManager.NavigateTo("/courselistoverview");
			}
			catch (Exception ex)
			{				
				Console.WriteLine(ex.Message);
			}
		}
		#endregion

		#region LoadList
		public async Task LoadList()
		{
			CourseHasSections = await CourseService.GetIncludeCourseHasSection(CourseId);
			Counter = CourseHasSections.Count;
			StateHasChanged();
		}
		#endregion

		#region Delete
		protected async Task Delete(Guid courseHasSectionId)
		{
			try
			{
				Deleted = CourseHasSections.Where(i => i.Id == courseHasSectionId).FirstOrDefault();				
				await CourseService.RemoveCourseHasSection(Deleted);
				await LoadList();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

		}
		#endregion

		#region Modal
		protected async Task OnDeleteDialogClose(bool accepted)
		{
			if (accepted)
			{

				await Delete(DeleteId);
			}

			DeleteDialogOpen = false;
			StateHasChanged();
		}

		protected void OpenDeleteDialog(Guid sectionid)
		{
			DeleteId = sectionid;
			DeleteDialogOpen = true;

			StateHasChanged();
		}
		#endregion
	}
}
