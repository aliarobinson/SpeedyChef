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

		//hard coded data
		private static JsonValue steps146_US = JsonValue.Parse("[{\"Taskid\":1,\"Taskname\":\"Preheat Oven\",\"Mealname\":null,\"Taskdesc\":\"Preheat oven to 450°F with a large shallow baking pan in upper third.\",\"Mealsize\":0,\"Tasktime\":900,\"Recid\":8,\"Taskid1\":0,\"Mealid\":145,\"Recid1\":0},{\"Taskid\":2,\"Taskname\":\"Mince Garlic\",\"Mealname\":null,\"Taskdesc\":\"Mince and mash garlic to a paste with 1/2 teaspoon salt, then stir together with butter and parsley.\",\"Mealsize\":0,\"Tasktime\":180,\"Recid\":8,\"Taskid1\":0,\"Mealid\":145,\"Recid1\":0},{\"Taskid\":3,\"Taskname\":\"Boil Lobsters\",\"Mealname\":null,\"Taskdesc\":\"Plunge lobsters headfirst into a large pot of boiling salted water (3 tablespoons salt for 6 quarts water) and cook, covered, 3 minutes from time they enter water.\",\"Mealsize\":0,\"Tasktime\":180,\"Recid\":8,\"Taskid1\":0,\"Mealid\":145,\"Recid1\":0},{\"Taskid\":4,\"Taskname\":\"Let Stand\",\"Mealname\":null,\"Taskdesc\":\"Transfer with tongs to a plate and let stand 5 minutes. (Lobsters will not be fully cooked.)\",\"Mealsize\":0,\"Tasktime\":300,\"Recid\":8,\"Taskid1\":0,\"Mealid\":145,\"Recid1\":0},{\"Taskid\":5,\"Taskname\":\"Open Lobsters\",\"Mealname\":null,\"Taskdesc\":\"Lightly crack claws, then split lobsters lengthwise and discard innards from body cavity.\",\"Mealsize\":0,\"Tasktime\":240,\"Recid\":8,\"Taskid1\":0,\"Mealid\":145,\"Recid1\":0},{\"Taskid\":7,\"Taskname\":\"Add Garlic Butter\",\"Mealname\":null,\"Taskdesc\":\"Remove tail meat from 1 lobster and cut crosswise into 8 pieces. Fill the empty half shells with butter. Repeat with remaining lobster.\",\"Mealsize\":0,\"Tasktime\":240,\"Recid\":8,\"Taskid1\":0,\"Mealid\":145,\"Recid1\":0}]");
		private static JsonValue tasks8_US = JsonValue.Parse("[{\"Taskid\":1,\"Recname\":null,\"Taskname\":\"Preheat Oven\",\"Taskdesc\":\"Preheat oven to 450°F with a large shallow baking pan in upper third.\",\"Tasktime\":900,\"Recid\":8,\"Taskid1\":0},{\"Taskid\":2,\"Recname\":null,\"Taskname\":\"Mince Garlic\",\"Taskdesc\":\"Mince and mash garlic to a paste with 1/2 teaspoon salt, then stir together with butter and parsley.\",\"Tasktime\":180,\"Recid\":8,\"Taskid1\":0},{\"Taskid\":3,\"Recname\":null,\"Taskname\":\"Boil Lobsters\",\"Taskdesc\":\"Plunge lobsters headfirst into a large pot of boiling salted water (3 tablespoons salt for 6 quarts water) and cook, covered, 3 minutes from time they enter water.\",\"Tasktime\":180,\"Recid\":8,\"Taskid1\":0},{\"Taskid\":4,\"Recname\":null,\"Taskname\":\"Let Stand\",\"Taskdesc\":\"Transfer with tongs to a plate and let stand 5 minutes. (Lobsters will not be fully cooked.)\",\"Tasktime\":300,\"Recid\":8,\"Taskid1\":0},{\"Taskid\":5,\"Recname\":null,\"Taskname\":\"Open Lobsters\",\"Taskdesc\":\"Lightly crack claws, then split lobsters lengthwise and discard innards from body cavity.\",\"Tasktime\":240,\"Recid\":8,\"Taskid1\":0},{\"Taskid\":7,\"Recname\":null,\"Taskname\":\"Add Garlic Butter\",\"Taskdesc\":\"Remove tail meat from 1 lobster and cut crosswise into 8 pieces. Fill the empty half shells with butter. Repeat with remaining lobster.\",\"Tasktime\":240,\"Recid\":8,\"Taskid1\":0}]");
		private static JsonValue ingredients8_US = JsonValue.Parse("[{\"Foodname\":\"2 small garlic cloves\",\"Member_Allergens\":[],\"Member_Allergens1\":[],\"Task_Food_Items\":[]},{\"Foodname\":\"2 sticks unsalted butter\",\"Member_Allergens\":[],\"Member_Allergens1\":[],\"Task_Food_Items\":[]},{\"Foodname\":\"1 1/2 teaspoons finely chopped flat-leaf parsley\",\"Member_Allergens\":[],\"Member_Allergens1\":[],\"Task_Food_Items\":[]},{\"Foodname\":\"2 (1 1/4-pound) live lobsters\",\"Member_Allergens\":[],\"Member_Allergens1\":[],\"Task_Food_Items\":[]}]");

		private static JsonValue steps146_M = JsonValue.Parse("[{\"Taskid\":1,\"Taskname\":\"Preheat Oven\",\"Mealname\":null,\"Taskdesc\":\"Preheat oven to 232°C with a large shallow baking pan in upper third.\",\"Mealsize\":0,\"Tasktime\":900,\"Recid\":8,\"Taskid1\":0,\"Mealid\":145,\"Recid1\":0},{\"Taskid\":2,\"Taskname\":\"Mince Garlic\",\"Mealname\":null,\"Taskdesc\":\"Mince and mash garlic to a paste with 2 1/2 ml salt, then stir together with butter and parsley.\",\"Mealsize\":0,\"Tasktime\":180,\"Recid\":8,\"Taskid1\":0,\"Mealid\":145,\"Recid1\":0},{\"Taskid\":3,\"Taskname\":\"Boil Lobsters\",\"Mealname\":null,\"Taskdesc\":\"Plunge lobsters headfirst into a large pot of boiling salted water (45 ml salt for 5.7 L water) and cook, covered, 3 minutes from time they enter water.\",\"Mealsize\":0,\"Tasktime\":180,\"Recid\":8,\"Taskid1\":0,\"Mealid\":145,\"Recid1\":0},{\"Taskid\":4,\"Taskname\":\"Let Stand\",\"Mealname\":null,\"Taskdesc\":\"Transfer with tongs to a plate and let stand 5 minutes. (Lobsters will not be fully cooked.)\",\"Mealsize\":0,\"Tasktime\":300,\"Recid\":8,\"Taskid1\":0,\"Mealid\":145,\"Recid1\":0},{\"Taskid\":5,\"Taskname\":\"Open Lobsters\",\"Mealname\":null,\"Taskdesc\":\"Lightly crack claws, then split lobsters lengthwise and discard innards from body cavity.\",\"Mealsize\":0,\"Tasktime\":240,\"Recid\":8,\"Taskid1\":0,\"Mealid\":145,\"Recid1\":0},{\"Taskid\":7,\"Taskname\":\"Add Garlic Butter\",\"Mealname\":null,\"Taskdesc\":\"Remove tail meat from 1 lobster and cut crosswise into 8 pieces. Fill the empty half shells with butter. Repeat with remaining lobster.\",\"Mealsize\":0,\"Tasktime\":240,\"Recid\":8,\"Taskid1\":0,\"Mealid\":145,\"Recid1\":0}]");
		private static JsonValue tasks8_M = JsonValue.Parse("[{\"Taskid\":1,\"Recname\":null,\"Taskname\":\"Preheat Oven\",\"Taskdesc\":\"Preheat oven to 232°C with a large shallow baking pan in upper third.\",\"Tasktime\":900,\"Recid\":8,\"Taskid1\":0},{\"Taskid\":2,\"Recname\":null,\"Taskname\":\"Mince Garlic\",\"Taskdesc\":\"Mince and mash garlic to a paste with 2 1/2 ml salt, then stir together with butter and parsley.\",\"Tasktime\":180,\"Recid\":8,\"Taskid1\":0},{\"Taskid\":3,\"Recname\":null,\"Taskname\":\"Boil Lobsters\",\"Taskdesc\":\"Plunge lobsters headfirst into a large pot of boiling salted water (45 ml salt for 5.7 L water) and cook, covered, 3 minutes from time they enter water.\",\"Tasktime\":180,\"Recid\":8,\"Taskid1\":0},{\"Taskid\":4,\"Recname\":null,\"Taskname\":\"Let Stand\",\"Taskdesc\":\"Transfer with tongs to a plate and let stand 5 minutes. (Lobsters will not be fully cooked.)\",\"Tasktime\":300,\"Recid\":8,\"Taskid1\":0},{\"Taskid\":5,\"Recname\":null,\"Taskname\":\"Open Lobsters\",\"Taskdesc\":\"Lightly crack claws, then split lobsters lengthwise and discard innards from body cavity.\",\"Tasktime\":240,\"Recid\":8,\"Taskid1\":0},{\"Taskid\":7,\"Recname\":null,\"Taskname\":\"Add Garlic Butter\",\"Taskdesc\":\"Remove tail meat from 1 lobster and cut crosswise into 8 pieces. Fill the empty half shells with butter. Repeat with remaining lobster.\",\"Tasktime\":240,\"Recid\":8,\"Taskid1\":0}]");
		private static JsonValue ingredients8_M = JsonValue.Parse("[{\"Foodname\":\"2 small garlic cloves\",\"Member_Allergens\":[],\"Member_Allergens1\":[],\"Task_Food_Items\":[]},{\"Foodname\":\"2 sticks unsalted butter\",\"Member_Allergens\":[],\"Member_Allergens1\":[],\"Task_Food_Items\":[]},{\"Foodname\":\"7 1/2 ml finely chopped flat-leaf parsley\",\"Member_Allergens\":[],\"Member_Allergens1\":[],\"Task_Food_Items\":[]},{\"Foodname\":\"2 (567 1/2 g) live lobsters\",\"Member_Allergens\":[],\"Member_Allergens1\":[],\"Task_Food_Items\":[]}]");

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
			JsonValue returnedSteps;
			if (mealId == 146 || mealId == 148) {
				if (CachedData.Instance.unitSystem == "U.S.") {
					returnedSteps = steps146_US;
				} else {
					returnedSteps = steps146_M;
				}
			} else {
				returnedSteps = getJSONResponse ("/Steps?mealid=" + mealId);
			}

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
			JsonValue recipeTasks;
			JsonValue recipeIngredients;
			if (recId == 8) {
				if (CachedData.Instance.unitSystem == "U.S.") {
					recipeTasks = tasks8_US;
					recipeIngredients = ingredients8_US;
				} else {
					recipeTasks = tasks8_M;
					recipeIngredients = ingredients8_M;
				}
			} else {
				recipeTasks = getJSONResponse ("/RecipeInfo/RecipeTasks?recid=" + recId);
				recipeIngredients = getJSONResponse ("/RecipeInfo/RecipeIngredients?recid=" + recId);
			}

			Recipe r = new Recipe ();
			r.title = recipeInfo ["Recname"];
			//r.desc = recipeInfo ["Recdesc"];
			//r.time = recipeInfo ["Rectime"];
			//r.diff = recipeInfo ["Recdiff"];
			string[] ingredients = new string[recipeIngredients.Count];
			string[] tasks = new string[recipeTasks.Count];
			for (int i = 0; i < recipeIngredients.Count; i++) {
				ingredients [i] = recipeIngredients[i]["Foodname"];
			}

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

