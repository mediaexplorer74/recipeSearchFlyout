﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace recipeSearchFlyout.ViewModels
{
	[QueryProperty(nameof(SearchFilter), nameof(SearchFilter))]
	public class SearchResultsViewModel : BaseViewModel
	{
        RestService _restService;

        RecipeData _recipeData;
        string _searchQuery;
        string _noResultsLabel;
        bool _noResultsLabelVisible;
        bool _searchResultsVisible;

        public Command<Hit> ItemTapped { get; }
        public Command SearchCommand { get; }

        public SearchResultsViewModel()
        {
            Title = "Search all recipes";
            _restService = new RestService();
            NoResultsLabelVisible = false;
            SearchResultsVisible = true;

            ItemTapped = new Command<Hit>(OnItemSelected);
            SearchCommand = new Command(async () => await OnSearch());

            MessagingCenter.Subscribe<SearchResultsViewModel, string>(this, "ShowRecipeHits", (sender, searchQuery) =>
            {
                SearchQuery = searchQuery;
            });

        }

        public RecipeData RecipeData
        {
            get => _recipeData;
            set => SetProperty(ref _recipeData, value);
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set => SetProperty(ref _searchQuery, value);
        }

        public string SearchFilter { get; set; }

        public string NoResultsLabel
        {
            get => _noResultsLabel;
            set => SetProperty(ref _noResultsLabel, value);
        }

        public bool NoResultsLabelVisible
        {
            get => _noResultsLabelVisible;
            set => SetProperty(ref _noResultsLabelVisible, value);
        }

        public bool SearchResultsVisible
        {
            get => _searchResultsVisible;
            set => SetProperty(ref _searchResultsVisible, value);
        }

        async Task OnSearch()
        {
            NoResultsLabelVisible = false;

            if (!string.IsNullOrWhiteSpace(SearchQuery) || !string.IsNullOrWhiteSpace(SearchFilter))
            {
                RecipeData recipeData = await _restService.GetRecipeDataAsync(GenerateRequestUri(Constants.EdamamEndpoint));

                if (recipeData == null || recipeData.Hits.Length == 0)
                {
                    NoResultsLabel = $"Sorry - we couldn't find any recipes for {SearchQuery} :(";
                    NoResultsLabelVisible = true;
                    SearchResultsVisible = false;
                }
                else
                {
                    NoResultsLabelVisible = false;
                    SearchResultsVisible = true;

                    for (int i = 0; i < recipeData.Hits.Length; i++)
                    {
                        recipeData.Hits[i].Id = i;
                    }

                    RecipeData = recipeData;
                    App.Data = RecipeData;
                    // AppShell.Data = RecipeData;

                    // OnPropertyChanged(nameof(RecipeData)); // tells Xaml view to update
                }

                // OnPropertyChanged(SearchQuery);
            }
        }

        string GenerateRequestUri(string endpoint)
        {
            string searchFilterName = SearchFilter.Substring(SearchFilter.IndexOf("=") + 1);

            if (string.IsNullOrEmpty(SearchQuery))
            {
                SearchQuery = searchFilterName;
            }

            string requestUri = endpoint;
            requestUri += $"?q={SearchQuery}";
            requestUri += $"&app_id={Constants.EdamamAppId}";
            requestUri += $"&app_key={Constants.EdamamAppKey}";
            requestUri += $"&to=100";

            if (!string.IsNullOrEmpty(SearchFilter))
            {
                Title = $"Search {searchFilterName} recipes";
                requestUri += $"&{SearchFilter}";
            }

            return requestUri;
        }

        async void OnItemSelected(Hit hit)
        {
            if (hit == null)
                return;

            // This will push the HitDetailPage onto the navigation stack
            // await Shell.Current.GoToAsync($"{nameof(HitDetailPage)}?HitId={hit.Id}");
        }
    }
}