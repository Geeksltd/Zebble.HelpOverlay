# PopOver

PopOver is a plugin for Zebble applications which make developers able to show a small hint to the user. This hint could be a simple text or a styled content.


### Methods

#### Extension methods:
PopOver adds an extension method on views to the Zebble namespace (two overloads).
```csharp
public static Task<PopOver> ShowPopOver(this View owner, string help);
public static Task<PopOver> ShowPopOver(this View owner, View content);
```

#### Hide:
Although it adds a cross to provide the close functionality, you may need to close it programmatically.

```csharp
var popOver = await aViewOnThePage.ShowPopOver("A piece of advice.");
...
await popOver.Hide();
```
