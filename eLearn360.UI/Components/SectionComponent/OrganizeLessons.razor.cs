namespace eLearn360.UI.Components.SectionComponent
{
    public partial class OrganizeLessons
    {
		[CascadingParameter] protected Guid SectionId { get; set; }
		[Inject] protected IToaster Toaster { get; set; }
		[Inject] public NavigationManager NavigationManager { get; set; }
		[Inject] public ISectionService SectionService { get; set; }

        public List<SectionHasLesson> SectionHasLessons { get; set; } = new List<SectionHasLesson>();
        public List<SectionHasLesson> Updated { get; set; } = new List<SectionHasLesson>();
        public SectionHasLesson Deleted { get; set; } = new SectionHasLesson();

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
				Updated = SectionHasLessons;				
				await SectionService.UpdateOrDelete(SectionHasLessons);			
				NavigationManager.NavigateTo("/sectionlistoverview");
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
			SectionHasLessons = await SectionService.GetIncludeSectionHasLesson(SectionId);
			Counter = SectionHasLessons.Count;
			StateHasChanged();
		}
		#endregion

		#region Delete
		protected async Task Delete(Guid sectionHasLessonId)
		{
            try
            {				
				Deleted = SectionHasLessons.Where(i => i.Id == sectionHasLessonId).FirstOrDefault();			
				await SectionService.RemoveSectionHasLesson(Deleted);
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

		protected void OpenDeleteDialog(Guid courseId)
		{
			DeleteId = courseId;
			DeleteDialogOpen = true;

			StateHasChanged();
		}
		#endregion
	}
}
