# ICUK Broadband Availability Checker

Integrate the ICUK broadband availability checker into your C# web application

## Usage

This library is general use and takes in data and includes 2 classes, 1 called BroadbandAvailbilityCheckerProxy which is a function that takes in
postcodes/addresses/phone numbers and returns availability data and another called BroadbandAvailabilityChecker which outputs html, css and javascript.

To use this library an endpoint should be setup that points data towards BroadbandAvailabilityCheckerProxy through the parameter: 'apidata'
You can then layout the availability checker on the webpage using BroadbandAvailabilityChecker which will output javascript which will interact with the proxy
endpoint specified.

Be careful to take into account that this library does not integrate with CRSF protection of the web framework being used so protection may have to be disabled on proxy endpoint.

An example application has been included below using ASP.net Razor Pages

### ApiProxy.cshtml

This file will allow the user to make requests to the broadband availability api. You should in this format but substitute the credentials with your own and
you can and should use your own method of authentication beforehand to prevent unauthorised access to the endpoint.

```cshtml
@page
@model ApiModel
@{
    BroadbandAvailabilityCheckerProxy proxy = new BroadbandAvailabilityCheckerProxy("ExampleApiUsername", "ExampleApipassword");
    Layout = null;
}

@Html.Raw(proxy.Api(Request.Form["apidata"]))
```

### Index.cshtml

This is a very simple example that shows how a page that presents the user with the broadband avaiability checker may be used.
The /ApiProxy endpoint is purely an example and that should just be a path to your Api endpoint

The RenderStyles line is also optional and you can apply your own stylesheet to the table, it also important to note that the results
panel will not appear until a search takes place.

```cshtml
@page
@model IndexModel
@{
    ViewData["Title"] = "Home page";
    BroadbandAvailabilityChecker checker = new BroadbandAvailabilityChecker();

    // You will need to find a way to include the style data if not using your own stylesheet
    ViewData["head"] = checker.RenderStyles(new SearchStyleSettings(), new ResultsStyleSettings(), new AddressSelectStyleSettings());
}

@Html.Raw(checker.RenderSearch("/ApiProxy"))
@Html.Raw(checker.RenderAddressSelect())
@Html.Raw(checker.RenderResults())
@Html.Raw(checker.RenderScripts())
```

![Example of what the above code should result in](https://github.com/BoronBGP/icuk-broadband-checker-php/blob/master/assets/default_example.png "Should result in this")

### Edit error message
To swap out the error message that occurs when an availability check fails you can use the error message as a parameter of render_search
```cshtml
@Html.Raw(checker.RenderSearch("/ApiProxy", "Example Error!"))
```

## Custom Styles
### Using Style Configuration
You can use the SearchStyleSettings and ResultsStyleSettings classes to edit the colour schemes of the search and results modules.
The example below shows using this to make the search button a red to green fade and the background of the results fields blue.

```csharp
var sstyle = new SearchStyleSettings();
var rstyle = new ResultsStyleSettings();
var astyle = new AddressSelectStyleSettings();

sstyle.ButtonGradientLow = "#f00";
sstyle.ButtonGradientHigh = "#00ff00";
rstyle.HeadBackgroundColour = "rgb(0, 0, 255)";
astyle.BackgroundColour = "rgb(0, 255, 0)";

checker.RenderStyles(new SearchStyleSettings(), new ResultsStyleSettings(), new AddressSelectStyleSettings());
```

![Example of what the above code should result in](https://github.com/BoronBGP/icuk-broadband-checker-php/blob/master/assets/style_example.png "Should result in this")

Currently the rgb, rgba, hex, hsl, hsla, CIELab, and xyz colour formats are supported, while colour names such as "purple" or "darkgreen" are invalid.

**ResultsStyleSettings Properties**
* **BackgroundColour**  : background colour of the results table
* **HeadBackgroundColour** : background colour of the head of the results table
* **LeftBackgroundColour** : background colour of the left side of the results table
* **SeperatorsColour** : colour of the seperators between cells
* **TextColour** : colour of the text of the results
* **HeadTextColour** : colour of the text of the head of the results table
* **LeftTextColour** : colour of the text of the left of the results table
* **AvailableLabelColour** : colour of the available label
* **NotAvailableLabelColour** : colour of the not available label
* **AvailableTextColour** : colour of the text on the available label
* **NotAvailableTextColour** : colour of the text on the not available label
* **LoadingCirclePrimaryColour** : the colour of the spinner on the loading circle
* **LoadingCircleSecondaryColour** : the colour of static background of the loading circle
* **HideResults** : hide the results of the result table before a search occurs

**SearchStyleSettings Properties**
 * **ButtonGradientLow** : colour of the button at the bottom of the gradient
 * **ButtonGradientHigh** : colour of the button at the top of the gradient
 * **ButtonGradientLowHover** : colour of the button while its being hovered over at the bottom of the gradient
 * **ButtonGradientHighHover** : colour of the button while its being hovered over at the top of the gradient
 * **ButtonTextColour** : colour of the text on the button
 * **InputBgColour** : colour of the the input box
 * **InputTextColour** : colour of the text on the input box
 * **InputHoverFadeColour** : colour of the fade around the input box while it is selected
 * **ErrorMessageColour** : colour of the error message.

**AddressSelectStyleSettings Properties**
* **BackgroundColour**  : background colour of the address table
* **HeadBackgroundColour** : background colour of the head of the results table
* **FilterBoxBackgroundColour** : background colour of the filter/search box
* **AddressTextColour** : colour of the seperators between cells
* **FilterBoxTextColour** : colour of the text in the filter/search box
* **HeadTextColour** : colour of the text of the head of the address table
* **NadTextColour** : colour of the text of NAD in the address table
* **BorderColour** : colour of the border of the address table
* **HeadBorderColour** : colour of the border of the head of the address table
* **FilterBoxBorderColour** : colour of the border of the filter/search box
* **FilterBoxHoverFadeColour** : colour of filter box when pressed down. Repeat the process with this

### Custom Stylesheet
You can develop your own stylesheet with relative ease as each module's elements are very simple to identify with their id's for example the button in the search
uses the id "broadband-availability-search-submit" and the input box uses "broadband-availability-search-input".
You can use the style template file used by this library [here](Templates/Styles.sbn) as a reference
