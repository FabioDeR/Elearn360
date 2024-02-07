using Microsoft.AspNetCore.Components.Authorization;

namespace eLearn360.UI.Pages.SearchPage
{
    public partial class SearchResultOverview
    {
        [Parameter] public string Filter { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        //[Inject] public IResearchService ResearchService { get; set; }

        //public List<ResearchVM> ResultList { get; set; } = new();

        private bool _isLoading = false;
        //private string traduction;

        protected override async Task OnInitializedAsync()
        {
            _isLoading = true;
            var authenticationState = (await AuthenticationStateTask).User;
            if (!authenticationState.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/login");
            }
            else
            {
                await LoadList();
            }
            _isLoading = false;
        }

        private async Task LoadList()
        {
            //ResultList = (await ResearchService.GetAllWithFilter(Filter)).ToList();
            //StateHasChanged();
        }

        //protected string GetTraduction(string type)
        //{
        //    switch (type)
        //    {
        //        case "Lesson":
        //            traduction = "Leçon";
        //            break;
        //        case "Section":
        //            traduction = "Section";
        //            break;
        //        case "Course":
        //            traduction = "Cours";
        //            break;
        //        case "Path":
        //            traduction = "Parcours";
        //            break;
        //    }

        //    return traduction;
        //}
    }
}
