﻿using System;
using System.Windows.Input;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Essentials;
using recipeSearchFlyout.Models;

namespace recipeSearchFlyout.ViewModels
{
    public class HitDetailViewModel : BaseViewModel
    {
        public Hit Hit { get; set; }

        string _hitId;
        string _recipeName;
        string _imageUrl;
        string _ingredients;
        string _recipeBody;
        FormattedString _recipeUrl;

        public ICommand TapCommand { get; }
        public Command AddItemCommand { get; }

        public HitDetailViewModel()
        {
            // Launcher.OpenAsync is provided by Xamarin.Essentials.
            TapCommand = new Command<string>(async (url) => await Launcher.OpenAsync(url));
            AddItemCommand = new Command(OnAddItem);
        }

        public string RecipeName
        {
            get => _recipeName;
            set => SetProperty(ref _recipeName, value);
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }

        public string Ingredients
        {
            get => _ingredients;
            set => SetProperty(ref _ingredients, value);
        }

        public string RecipeBody
        {
            get => _recipeBody;
            set => SetProperty(ref _recipeBody, value);
        }

        public FormattedString RecipeUrl
        {
            get => _recipeUrl;
            set => SetProperty(ref _recipeUrl, value);
        }

        private void OnAddItem()
        {
            Item newItem = new()
            {
                Id = HitId,
                RecipeName = RecipeName,
                ImageUrl = ImageUrl,
                Ingredients = Ingredients,
                RecipeBody = RecipeBody,
                RecipeUrl = RecipeUrl
            };

            DataStore.AddRecipeAsync(newItem);
        }

        public string HitId
        {
            get
            {
                return _hitId;
            }
            set
            {
                _hitId = value;
                LoadHitId(value);
            }
        }

        public void LoadHitId(string hitId)
        {
            try
            {
                Hit = App.Data.Hits.FirstOrDefault(h => h.Id == int.Parse(hitId));
                OnPropertyChanged(nameof(Hit));
                LoadHitDetails(Hit);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Hit");
            }
        }

        public void LoadHitDetails(Hit hit)
        {
            Title = Hit.Recipe.RecipeName;

            RecipeName = Hit.Recipe.RecipeName;
            ImageUrl = Hit.Recipe.ImageUrl;
            Ingredients = string.Join(Environment.NewLine, Hit.Recipe.Ingredients);

            var recipeBodyFormattedString = new FormattedString();
            recipeBodyFormattedString.Spans.Add(new Span { Text = "Click " });

            var recipeUrlFormattedString = new Span { Text = "here", TextColor = Color.Blue, TextDecorations = TextDecorations.Underline };
            recipeUrlFormattedString.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = TapCommand,
                CommandParameter = Hit.Recipe.RecipeUrl
            });
            recipeBodyFormattedString.Spans.Add(recipeUrlFormattedString);

            recipeBodyFormattedString.Spans.Add(new Span { Text = " to view full recipe." });
            RecipeUrl = recipeBodyFormattedString;
        }
    }
}