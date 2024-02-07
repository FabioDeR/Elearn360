namespace eLearn360.UI.Pages.ProfessorPage.PathWayPage
{
    public partial class OrganizePathWay
    {
		[Parameter] public Guid PathWayId { get; set; }
		[Parameter] public string Role { get; set; }
		[Inject] public IPathWayService PathWayService { get; set; }
		[Inject] protected IToaster Toaster { get; set; }
		[Inject] public NavigationManager NavigationManager { get; set; }

		public List<PathWayHasCourse> PathWayHasCourses { get; set; } = new();
		public List<PathWayHasCourse> Updated { get; set; } = new();
		public PathWayHasCourse Deleted { get; set; } = new PathWayHasCourse();

		protected int Counter { get; set; }
		public bool DeleteDialogOpen { get; set; }
		protected Guid DeleteId { get; set; }
		private bool _isLoading = false;
		protected string histoNav = string.Empty;

		protected override async Task OnInitializedAsync()
		{
			_isLoading = true;
			await LoadList();
			histoNav = (Role == "Admin" ? "orgapathadmin" : "orgapathteacher"); 
			_isLoading = false;
		}

		#region UpdatePosition
		protected async Task UpdatePosition()
		{
			try
			{
				Updated = PathWayHasCourses;
				await PathWayService.UpdateOrDelete(Updated);
				Toaster.Success("Modification enregistrée !");
				NavigationManager.NavigateTo("/pathlistoverview");
			}
			catch (Exception ex)
			{
				Toaster.Error("Une erreur est apparue !");
				Console.WriteLine(ex.Message);
			}
		}
		#endregion

		#region LoadList
		public async Task LoadList()
		{
			PathWayHasCourses = await PathWayService.GetIncludePathWayHasCourse(PathWayId);
			Counter = PathWayHasCourses.Count;
			StateHasChanged();
		}
		#endregion

		#region Delete
		protected async Task Delete(Guid pathWayId)
		{
			try
			{
				Deleted = PathWayHasCourses.Where(i => i.Id == pathWayId).FirstOrDefault();
				await PathWayService.RemovePathWayHasCourse(Deleted);
				await LoadList();
			}
			catch (Exception ex)
			{
				Toaster.Error("Une erreur est apparue !");
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
