### Xamarin Mobile App
  
* In Visual Studio, **File** > **New Project**
  * Select **Cross Platform** under Visual C# and choose **Cross Platform App (Xamarin)**
  * Name your project `OfficeDogs.Mobile` and your solution `OfficeDogs`

  * <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/new_forms_project.png?raw=true" Height="600" Margin="40" />

  * Choose **Xamarin Forms** and **Portable Class Library**
* Make sure the platform you'll be working with (iOS or Android) is set as the Startup Project
* Add another project to your solution > **Visual C#** > **Class Library (Shared)**
  * Name this project `OfficeDogs.Common`
* Add a reference to `OfficeDogs.Common` to `OfficeDogs.Mobile`
  * Look within the References in `OfficeDogs.Mobile` project > right-click to **Add References** > look under **Shared Projects** to find `OfficeDogs.Common`
* Open `MainPage.xaml` and add the following code as your page content: [code](https://github.com/rob-derosa/Q2Hackathon/blob/master/src/OfficeDogs/OfficeDogs.Mobile/OfficeDogs.Mobile/MainPage.xaml#L7-L45)
* Add a new folder to `OfficeDogs.Mobile` called `ViewModels`
  * Within `ViewModels`, add a new C# class called `DogViewModel`
* Add a new folder to `OfficeDogs.Common` called `Models`
  * Add a new file within `Models` called `Dog`
* Add the **Newtonsoft.Json** nuget package to `OfficeDogs.Mobile`
  * (Right-click the project and click **Manage NuGet Packages**.  The default list of packages shown will be the installed packages — make sure to click **“Browse”** to see a full list of available Nuget packages)
* Add the **HttpClient** nuget package to `OfficeDogs.Mobile`
* Add some properties you want to persist to the `Dog` model (name, age, breed, gender, etc) i.e. `public string Breed { get; set; }`
* Ensure you have an `string id` (lowercase) - property this is required for DocumentDB
* In `DogViewModel`
  * ensure your class derives from `BindableObject` (if you see an error, you’ll need to add a using reference to **Xamarin.Forms**)
  * Add 2 `INotifyPropertyChanged` properties: `Dog` and `IsBusy`
    * Instantiate `Dog` to a new instance
* Add this `Extensions.cs` file to your `OfficeDogs.Mobile` project: [code](https://github.com/rob-derosa/Q2Hackathon/blob/master/src/OfficeDogs/OfficeDogs.Mobile/OfficeDogs.Mobile/Extensions.cs)
* Add the following to `DogViewModel`
  * a method called `SaveDog` that returns `Task<bool>`
    * inside, add a `try/catch` block that awaits a 2 second delay and `Debug.WriteLine` the dog's name, all encompassed with an `IsBusy` flag (this is placeholder code)
  * a property called `DogAge` that simply wraps `Dog.Age` and calls `OnPropertyChanged()` in the setter  
* Back in `MainPage.xaml.cs`
  * Add a `DogViewModel` property and instantiate it to a new instance
    * Set this property as the page's `BindingContext` - do this before `InitializeComponent`
  * Add a click event handler for the save button and call the `SaveDog` method on the ViewModel
  * At this point, your solution should look similar to this
  * <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/solution_tree.png?raw=true" width="350" />
* Build your project and run
  * Your app should look similar to this
  * <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/android_screen.png?raw=true" width="350" />
  * Complete the form and click `Save` - look for the dog's name in the debug output pane
  * Pat self on back


### Azure Functions + Cosmos
* Add a new project to your solution > Cloud > Azure Functions and call it `OfficeDogs.Backend`
* <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/new_functions_project.png?raw=true" height="600" />
* Add a reference to `OfficeDogs.Common` to `OfficeDogs.Backend`
* Add a new file > **Azure Function** and call it `SaveDog`
  * Select `HttpTrigger` as the type and set `Access rights` to `Anonymous`
  * <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/new%20_function.png?raw=true" height="600" />
* Add the **Newtonsoft.Json** nuget package to `OfficeDogs.Backend`
* Add the **Microsoft.Azure.DocumentDB** nuget package to `OfficeDogs.Backend`
* Add this `Keys.cs` file to your `OfficeDogs.Common` project: [code](https://github.com/rob-derosa/Q2Hackathon/blob/master/src/OfficeDogs/OfficeDogs.Common/Keys.cs)

* In the Azure Portal (portal.azure.com)
  * Create a new resource group and call it `officedogs`
  * <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/new_resource_group.png?raw=true" width="300" />
  * Within this resource group
    * Create a new Functions App and call it `officedogs-XXXX` (globally unique name) and click `Create`
    * <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/portal_new_function.png?raw=true" width="300" />
    * Copy the URL for the functions app and add it to the `Keys.cs` file (example: https://officedogs.azurewebsites.net)
* Right click on `Office.Backend` and select `Publish`
  * Choose `Select Existing` and select the Functions App created in the previous step
  * <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/publish_settings.png?raw=true" height="600" />
  * When publish completes, click on the URL link to launch the browser
    * Add `/api/SaveDog?name=John` and ensure you see the XML string `Hello John` as the response
    * Back in the portal, you should see your function
  * Within the your resource group
    * Add a Cosmos DB service
    * Enter a unique name for the database and choose `SQL (DocumentDB)` as the API type and click `Create`
    * <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/portal_new_cosmos.png?raw=true" width="300">
    * Once the database is created, click on `Overview` and copy the URI and add it to the `Keys.cs` file (example: https://officedogs.documents.azure.com:443/)
    * <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/cosmos_overview.png?raw=true" width="700">
    * Navigate to the `Keys` section and copy the `PRIMARY KEY` and add it to the `Keys.cs` file (example: 74jdg284jsxc9AQxNXdUCnsh63XqHi28jgg1rhgabaWze33jb7lFOxMqIgvVdz5uKkq0W9NfS9HAUEqQ==)
    * <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/documentdb_keys.png?raw=true" height="500" />

* Back in Visual Studio, add the `DocumentDbService.cs` file to your `OfficeDogs.Backend` project: [code](https://github.com/rob-derosa/Q2Hackathon/blob/master/src/OfficeDogs/OfficeDogs.Backend/DocumentDbService.cs)
* In the `SaveDog.cs` file, remove the contents of the function and remove `get` as an action
  * Read the body contents of the post into a string by calling `req.Content.ReadAsStringAsync()`
  * Using Newtonsoft, deserialize the json content into a `Dog` object
  * If the object's `id` value is null, save the document, otherwise update it
  * Return the the Document you saved/updated in `req.CreateResponse` as part of the response
* Publish the backend function
* In `OfficeDogs.Mobile.`, we need to add some code to our ViewModel to send up the `Dog` object
  * Open `DogViewModel` and remove the delayed task and add the following:
    * instantiate a HttpClient if it is null
    * call `SendPostRequest` (located in Extensions.cs) and pass in the function name and the `Dog` object
    * set the value of `Dog` to the return value if it's not null


* Launch the mobile app, complete the form and click the `Save` button
* Note that after a successful save, you should see a label with the document ID
  * You can also now view the JSON document in the Cosmos Data Previewer tool
* Update a value of the form and click the `Save` button once more - this time your document should update


#### Bonus Points
* Bring in the XFGloss nuget pacakge and apply a gradient background color to your page
