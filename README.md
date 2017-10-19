### Xamarin Mobile App
  
1. In Visual Studio, **File** > **New Project**
   1. Select **Cross Platform** under Visual C# and choose **Cross Platform App (Xamarin)**
   1. Choose **Blank App**
   1. Name your project `OfficeDogs.Mobile` and your solution `OfficeDogs`
   <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/new_forms_project.png?raw=true" Height="600" Margin="40" />
   1. Choose **Xamarin Forms** and **Portable Class Library**
   <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/forms_project_config.png?raw=true" Height="350" Margin="40" />

1. Make sure the platform you'll be working with (iOS or Android) is set as the Startup Project
1. Add another project to your solution > **Visual C#** > **Shared Project**
   1. Name this project `OfficeDogs.Common`
1. Add a reference to `OfficeDogs.Common` to `OfficeDogs.Mobile`
   1. Look within the References in `OfficeDogs.Mobile` project > right-click to **Add References** > look under **Shared Projects** to find `OfficeDogs.Common`
1. Open `MainPage.xaml` and add the following code as your page content: [code](https://github.com/rob-derosa/Q2Hackathon/blob/master/src/OfficeDogs/OfficeDogs.Mobile/OfficeDogs.Mobile/MainPage.xaml#L7-L45)
1. Add a new folder to `OfficeDogs.Mobile` called `ViewModels`
   1. Within `ViewModels`, add a new C# class called `DogViewModel`
1. Add a new folder to `OfficeDogs.Common` called `Models`
   1. Add a new file within `Models` called `Dog.cs` which will serve as the `Dog` model
1. Add the **Newtonsoft.Json** nuget package to `OfficeDogs.Mobile`
   1. (Right-click the project and click **Manage NuGet Packages**.  The default list of packages shown will be the installed packages — make sure to click **“Browse”** to see a full list of available Nuget packages)
1. Add the **System.Net.Http** by Microsoft nuget package to `OfficeDogs.Mobile`
1. Add some properties you want to persist to the `Dog` model (name, age, breed, gender, etc) i.e. `public string Breed { get; set; }`
1. Ensure you have an `string id` (lowercase) - property this is required for DocumentDB
1. In `DogViewModel`
   1. ensure your class derives from `BindableObject` (if you see an error, you’ll need to add a using reference to **Xamarin.Forms**)
   1. Add 2 `INotifyPropertyChanged` properties: `Dog` and `IsBusy`
   1. Instantiate `Dog` to a new instance
1. Add this `Extensions.cs` file to your `OfficeDogs.Mobile` project: [code](https://github.com/rob-derosa/Q2Hackathon/blob/master/src/OfficeDogs/OfficeDogs.Mobile/OfficeDogs.Mobile/Extensions.cs)
1. Add the following to `DogViewModel`
   1. a method called `SaveDog` that returns `Task<bool>`
   1. inside, add a `try/catch` block that awaits a 2 second delay and `Debug.WriteLine` the dog's name, all encompassed with an `IsBusy` flag (this is placeholder code)
   1. a property called `DogAge` that simply wraps `Dog.Age` and calls `OnPropertyChanged()` in the setter  
1. Back in `MainPage.xaml.cs`
   1. Add a `DogViewModel` property and instantiate it to a new instance
   1. Set this property as the page's `BindingContext` - do this before `InitializeComponent`
   1. Add a click event handler for the save button and call the `SaveDog` method on the ViewModel
   1. At this point, your solution should look similar to this
 
   <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/solution_tree.png?raw=true" width="350" />

1. Build your project and run
   1. Your app should look similar to this

   <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/android_screen.png?raw=true" width="350" />
   
   1. Complete the form and click `Save` - look for the dog's name in the debug output pane
   1. Pat self on back


### Azure Functions + Cosmos
1. Add a new project to your solution > Cloud > Azure Functions and call it `OfficeDogs.Backend`
<img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/new_functions_project.png?raw=true" height="600" />
1. Add a reference to `OfficeDogs.Common` to `OfficeDogs.Backend`
1. Add a new file > **Azure Function** and call it `SaveDog`
   1. Select `HttpTrigger` as the type and set `Access rights` to `Anonymous`
   
   <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/new%20_function.png?raw=true" height="600" />

1. Add the **Newtonsoft.Json** nuget package to `OfficeDogs.Backend`
1. Add the **Microsoft.Azure.DocumentDB** nuget package to `OfficeDogs.Backend`
1. Add this `Keys.cs` file to your `OfficeDogs.Common` project: [code](https://github.com/rob-derosa/Q2Hackathon/blob/master/src/OfficeDogs/OfficeDogs.Common/Keys.cs)

1. In the Azure Portal (portal.azure.com)
   1. Create a new resource group and call it `officedogs`

   <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/new_resource_group.png?raw=true" width="300" />
   
   1. Within this resource group
     1. Create a new Functions App and call it `officedogs-XXXX` (globally unique name) and click `Create`

     <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/portal_new_function.png?raw=true" width="300" />

     1. Copy the URL for the functions app and add it to the `Keys.cs` file (example: https://officedogs.azurewebsites.net)
1. Right click on `Office.Backend` and select `Publish`
   1. Choose `Select Existing` and select the Functions App created in the previous step

   <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/publish_settings.png?raw=true" height="600" />

   1. When publish completes, click on the URL link to launch the browser
     1. Add `/api/SaveDog?name=John` and ensure you see the XML string `Hello John` as the response
     1. Back in the portal, you should see your function
   1. Within the your resource group
     1. Add a Cosmos DB service
     1. Enter a unique name for the database and choose `SQL (DocumentDB)` as the API type and click `Create`

     <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/portal_new_cosmos.png?raw=true" width="300">

     1. Once the database is created, click on `Overview` and copy the URI and add it to the `Keys.cs` file (example: https://officedogs.documents.azure.com:443/)

     <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/cosmos_overview.png?raw=true" width="800">

     1. Navigate to the `Keys` section and copy the `PRIMARY KEY` and add it to the `Keys.cs` file (example: 74jdg284jsxc9AQxNXdUCnsh63XqHi28jgg1rhgabaWze33jb7lFOxMqIgvVdz5uKkq0W9NfS9HAUEqQ==)

     <img src="https://github.com/rob-derosa/Q2Hackathon/blob/master/guides/images/documentdb_keys.png?raw=true" height="400" />

1. Back in Visual Studio, add the `DocumentDbService.cs` file to your `OfficeDogs.Backend` project: [code](https://github.com/rob-derosa/Q2Hackathon/blob/master/src/OfficeDogs/OfficeDogs.Backend/DocumentDbService.cs)
1. In the `SaveDog.cs` file, remove the contents of the function and remove `get` as an action
   1. Read the body contents of the post into a string by calling `req.Content.ReadAsStringAsync()`
   1. Using Newtonsoft, deserialize the json content into a `Dog` object
   1. If the object's `id` value is null, save the document, otherwise update it
   1. Return the the Document you saved/updated in `req.CreateResponse` as part of the response
1. Publish the backend function
1. In `OfficeDogs.Mobile.`, we need to add some code to our ViewModel to send up the `Dog` object
   1. Open `DogViewModel` and remove the delayed task and add the following:
     1. instantiate a HttpClient if it is null
     1. call `SendPostRequest` (located in Extensions.cs) and pass in the function name and the `Dog` object
     1. set the value of `Dog` to the return value if it's not null


1. Launch the mobile app, complete the form and click the `Save` button
1. Note that after a successful save, you should see a label with the document ID
   1. You can also now view the JSON document in the Cosmos Data Previewer tool
1. Update a value of the form and click the `Save` button once more - this time your document should update


#### Bonus Points
1. Bring in the XFGloss nuget pacakge and apply a gradient background color to your page
