using System;
using System.Net;
using System.IO;
using System.Json;
using System.Threading.Tasks;

namespace SpeedyChef
{
	public static class WebUtils
	{
		//The base url for all requests. This may be subject to change
		private static string baseURI = "http://speedychef.azurewebsites.net";

		public static JsonValue getJSONResponse(string requestUrl) {
			var request = HttpWebRequest.Create (baseURI + requestUrl);
			request.ContentType = "application/json";
			request.Method = "GET";

			var response = request.GetResponse ();
			return JsonValue.Load (response.GetResponseStream ());
		}

		public static Task<JsonValue> getJSONResponseAsync (string requestUrl)
		{
			// Create an HTTP web request using the URL:
			var request = HttpWebRequest.Create (baseURI + requestUrl);
			request.ContentType = "application/json";
			request.Method = "GET";
			// Send the request to the server and wait for the response:
			using (WebResponse response =  request.GetResponse()) {
				// Get a stream representation of the HTTP web response:
				using (Stream stream = response.GetResponseStream ()) {
					// Use this stream to build a JSON document object:
					Task<JsonValue> jsonDoc =  Task.Run (() => JsonObject.Load (stream));
					// Return the JSON document:
					return jsonDoc;
				}
			}
		}

		public static void sendRequest (string requestUrl) {
			var request = HttpWebRequest.Create (baseURI + requestUrl);
			request.ContentType = "application/json";
			request.Method = "GET";
			request.GetResponse ();
			request = null;
		}

		public static RecipeStep[] getRecipeSteps(int mealId) {
			JsonValue returnedSteps = getJSONResponse ("/Steps?mealid=" + mealId);
			RecipeStep[] steps = new RecipeStep[returnedSteps.Count];
			for (int i = 0; i < returnedSteps.Count; i++) {
				JsonValue currentItem = returnedSteps [i];
				RecipeStep currentStep = new RecipeStep ();
				currentStep.title = currentItem ["Taskname"];
				currentStep.desc = currentItem["Taskdesc"];
				currentStep.time = currentItem ["Tasktime"];
				//currentStep.timeable = currentItem ["Tasktimeable"];
				if(i > 1 && i < 4) //For testing
					currentStep.timeable = true;
				if (currentStep.timeable)
					currentStep.timerHandler = new RecipeStepTimerHandler (currentStep.title, currentStep.time);
				steps [i] = currentStep;
			}
			return steps;
		}

		public static Recipe getRecipeViewInfo(int recId) {
			JsonValue recipeInfo = getJSONResponse ("/RecipeInfo/RecipeInfo?recid=" + recId);
			recipeInfo = recipeInfo [0];
			JsonValue recipeTasks = getJSONResponse ("/RecipeInfo/RecipeTasks?recid=" + recId);
			JsonValue recipeIngredients = getJSONResponse ("/RecipeInfo/RecipeIngredients?recid=" + recId);
			Recipe r = new Recipe ();
			r.title = recipeInfo ["Recname"];
			//r.desc = recipeInfo ["Recdesc"];
			//r.time = recipeInfo ["Rectime"];
			//r.diff = recipeInfo ["Recdiff"];
			string[] ingredients = new string[recipeIngredients.Count];
			string[] tasks = new string[recipeTasks.Count];
			/*for (int i = 0; i < recipeIngredients.Count; i++) {
				//ingredients [i] = "Ingredient " + i;
				string ingredient = recipeIngredients[i]["Foodname"];
				if (recipeIngredients [i] ["FoodAmount"] != null)
					ingredient += ", " + recipeIngredients [i] ["FoodAmount"];
				if (recipeIngredients [i] ["FoodAmountUnit"] != null)
					ingredient += " " + recipeIngredients [i] ["FoodAmountUnit"];
				
				ingredients [i] = ingredient;
			}*/

			int recTime = 0;
			for (int i = 0; i < recipeTasks.Count; i++) {
				tasks [i] = recipeTasks [i] ["Taskdesc"];
				recTime += recipeTasks [i] ["Tasktime"];
			}
			r.ingredients = ingredients;
			r.tasks = tasks;
			return r;
		}

		public static Task<JsonValue> addMeal(String userId, String mealName, String date, int mealSize) {
			return getJSONResponseAsync("CalendarScreen/AddMeal?user=" + userId + "&mealname=" +
				mealName + "&date=" + date + "&size=" + mealSize);
		}
			
	}
}

