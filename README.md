[logo]: https://raw.githubusercontent.com/Geeksltd/Zebble.PopOver/master/Shared/NuGet/Icon.png "Zebble.PopOver"


## Zebble.PopOver

![logo]

A Zebble plugin that adds small overlay content to any view for housing extra information. The PopOver content can be a simple text or a complex heirarchy of views.


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

##### Show a PopOver

```csharp
var popOver = await SomeView.PopOver("A piece of advice.");
//Or
var popOver = await SomeView.PopOver(new Canvas());
```


##### Hide a PopOver
PopOver adds a close button to provide the hide functionality, you may need to close it programmatically.

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
| PopOver *     | Task<PopOver>| help -> string                      | x       | x   | x       |
| PopOver *     | Task<PopOver>| content -> Zebble.View              | x       | x   | x       |
| Hide          | Task<PopOver>| help -> string                      | x       | x   | x       |
  
\* Extension methods on Zebble.View objects.
