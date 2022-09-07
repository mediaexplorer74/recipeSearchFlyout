﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using recipeSearchFlyout.Models;

namespace recipeSearchFlyout.Services
{
	public class MockDataStore : IDataStore<Item>
	{
        readonly List<Item> recipes;

        public MockDataStore()
        {
            recipes = new List<Item>()
            {
                new Item
                {
                    Id = Guid.NewGuid().ToString(),
                    RecipeName = "달걀말이 (Korean Egg Roll)",
                    ImageUrl = "https://www.koreanbapsang.com/wp-content/uploads/2019/10/DSC_1183-2.jpg",
                    Ingredients = "1 scallion\n8 eggs\ncanola oil\n1 tbsp salt\n1 tbsp pepper",
                    RecipeBody = "1. Grab a bowl, and add the scallion (chopped), eggs (cracked), salt, pepper\n" +
                    "2. Thoroughly beat the egg mixture\n" +
                    "3. Heat the pan on the stove, and add 3 turns of oil\n" +
                    "4. Once pan is ready/warm, pour in the egg mixture; if using 8 eggs, pour in only half\n" +
                    "5. Tilt the pan around so egg mixture fills the pan.\n" +
                    "6. Begin folding the egg over from the edge of the pan\n" +
                    "7. Continually fold egg over while pushing uncooked egg to the other side of the pan and shaping it\n" +
                    "8. Once completely folded, remove from pan\n" +
                    "9. Repeat steps 3 - 8 if there is remaining egg mixture\n" +
                    "10. Cut the 달걀말이 into pieces"
                },
                new Item
                {
                    Id = Guid.NewGuid().ToString(),
                    RecipeName = "된장찌개 (Korean Soybean Paste Stew)",
                    ImageUrl = "https://mblogthumb-phinf.pstatic.net/MjAxNzEyMjlfMTQ0/MDAxNTE0NTM2MTcwMzM5.feATDxTPqCzmnlXqheAC87Fk0pMo_9uz3fj8FDu1zgwg.qdar-w_Xggvqp9IB8bPMwGMRaCt_CvGgDfqFCwbt6Zog.JPEG.sundoong2/image_1532650841514535869603.jpg?type=w800",
                    Ingredients = "1 bag 다시 (Dasi anchovy soup stock)\n3 tbsp 된장 (soybean paste)\n1.5 tbsp 미소 된장 (miso soybean paste)\n1 tbsp 고추가루 (Korean red pepper powder)\npotato\ncarrot\nonion\n호박 (Korean squash)\n소고기 (beef / thin brisket) (optional)\ntofu (optional)",
                    RecipeBody = "1. Pour water into the pot so that it fills about about 1/3 of the pot. Bring the water to a boil\n" +
                    "2. Put the bagged 다시 into the pot\n" +
                    "3. 10 minutes later, add the 된장 and 미소 된장 into the pot\n" +
                    "4. Stir the ingredients in the soup mixture, as it continues to boil\n" +
                    "5. Add in the 고추가루\n" +
                    "6. Add in the potato, carrots, onion chunks. These veggies should be chopped in advance into 1/2” - 1” pieces\n" +
                    "7. Add in any additional ingredients you have! (i.e. beef, tofu, etc.)\n" +
                    "8. If you have, add in the Korean squash! This should also have been chopped in advance into 1/2” - 1” pieces\n" +
                    "9. Continue to boil until ready to serve. Use ladle to remove any froth floating on the soup’s surface"
                },
                new Item
                {
                    Id = Guid.NewGuid().ToString(),
                    RecipeName = "Salmon Bake",
                    ImageUrl = "https://www.inspiredtaste.net/wp-content/uploads/2018/09/Easy-Oven-Baked-Salmon-Recipe-2-1200.jpg",
                    Ingredients = "1 pack (frozen) salmon (i.e. costco)\nmasago\nmayo\nolive oil\nsalt\npepper\nscallion (optional)",
                    RecipeBody = "1. Thaw frozen salmon for at least 2 hours.\n" +
                    "2. Salt and pepper the salmon on both sides\n" +
                    "3. Oil the pan.\n" +
                    "4. Place the seasoned salmon into the pan, covering the bottom with a single layer\n" +
                    "5. Mix masago, mayo, and scallion (chopped) together in a small bowl\n" +
                    "6. Spread a thin layer of the mixture onto the salmon\n" +
                    "7. Preheat the oven to 350ºF\n" +
                    "8. Cover the pan with foil and poke holes with a fork\n" +
                    "9. Bake the salmon! (combination fast bake ~30 minutes)"
                }
            };
        }

        public async Task<bool> AddRecipeAsync(Item recipe)
        {
            recipes.Add(recipe);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateRecipeAsync(Item recipe)
        {
            var oldRecipe = recipes.Where((Item arg) => arg.Id == recipe.Id).FirstOrDefault();
            recipes.Remove(oldRecipe);
            recipes.Add(recipe);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteRecipeAsync(string id)
        {
            var oldItem = recipes.Where((Item arg) => arg.Id == id).FirstOrDefault();
            recipes.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetRecipeAsync(string id)
        {
            return await Task.FromResult(recipes.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetRecipesAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(recipes);
        }
    }
}