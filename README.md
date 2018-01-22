[logo]: https://raw.githubusercontent.com/Geeksltd/{Plugin.Name}/master/Shared/NuGet/Icon.png "{Plugin.Name}"


## Zebble.PopOver

![logo]

PopOver is a plugin for Zebble applications which make developers able to show a small hint to the user. This hint could be a simple text or a styled content.


[![NuGet](https://img.shields.io/nuget/v/Zebble.PopOver.svg?label=NuGet)](https://www.nuget.org/packages/Zebble.PopOver/)


### Setup
* Available on NuGet: [https://www.nuget.org/packages/Zebble.PopOver/](https://www.nuget.org/packages/Zebble.PopOver/)
* Install in your platform client projects.
* Available for iOS, Android and UWP.
* After installing the Nuget copy this [SCSS](https://github.com/Geeksltd/Zebble.PopOver/blob/master/Shared/PopOver.scss/) file to your project.
* Import the SCSS file to `common.scss` file.
```scss
@import "PopOver.scss";
```
* Call its mixin in the `common.scss` just like the other mixins.
```scss
@include pop-over($navbar-height);
```
<br>


### Api Usage

Display a PopOver over the a view.

```csharp
var popOver = await aViewOnThePage.ShowPopOver("A piece of advice.");
```

Although it adds a cross to provide the close functionality, you may need to close it programmatically.

```csharp
await popOver.Hide();
```

<br>


### Events
| Event             | Type          | Android | iOS | Windows |
| :-----------      | :-----------  | :------ | :-- | :------ |
| OnShown           | AsyncEvent    | x       | x   | x       |
| OnHide            | AsyncEvent    | x       | x   | x       |


<br>


### Methods
| Method        | Return Type  | Parameters                          | Android | iOS | Windows |
| :-----------  | :----------- | :-----------                        | :------ | :-- | :------ |
| ShowPopOver * | Task<PopOver>| help -> string                      | x       | x   | x       |
| ShowPopOver * | Task<PopOver>| content -> Zebble.View              | x       | x   | x       |
| Hide          | Task<PopOver>| help -> string                      | x       | x   | x       |
  
\* Extension methods on Zebble.View objects.
