# Unity - Tailwind CSS

Tailwind CSS for Unity allows developers to integrate the powerful Tailwind CSS framework with Unity's UI Toolkit. This tool enables Unity users to leverage Tailwind's utility-first CSS to style UXML components effortlessly. By converting Tailwind's rem and arbitrary value classes to Unity-supported formats and simplifying class structures, this tool facilitates seamless UI styling for games and apps. This tool automatically monitors changes in UXML and C# files, triggering CSS regeneration when needed, and can be added globally to reduce the need for manual updates, significantly enhancing workflow efficiency.

Unity’s USS (Unity Style Sheets) is more restrictive with selectors compared to standard CSS used by Tailwind. While this integration brings many benefits of Tailwind into Unity, there are key differences, such as the lack of support for certain selectors and the need to adapt class naming conventions (e.g., avoiding / in class names). We highly recommend checking the example code provided to understand how Tailwind classes are applied in Unity and to explore the differences between traditional Tailwind and this Unity-adapted version.

This project is currently a work in progress and is likely to change. Expect updates, improvements, and possible breaking changes as development continues!

## Support My Work

If you've found my projects helpful and want to support future developments, consider buying me a coffee! Your support helps me continue creating awesome open-source tools and content. 😊

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/B0B014JJ1U)

## Requirements:

* Unity Version: 6000.0 or later
* Node.js: Installed for using Tailwind's build process (npx).

## Installation

### Option 1

Go to the Releases section on GitHub and download the Unity package. Then, with your project open, simply run the file to install it.

### Option 2

Add the Git Repository to Unity: Add the package to Unity’s Package Manager by modifying the Packages/manifest.json:

```json
{
  "dependencies": {
    "com.ngc-corp.unity-tailwindcss": "git+https://github.com/ngc-corp/unity-tailwindcss.git?path=/Packages/com.ngc-corp.unity-tailwindcss"
  }
}
```

## Initialize

Navigate to `Tools/Tailwind/Init Tailwind`.

This will create a TailwindCSS folder in your Assets directory with the necessary files. However, since no paths are being monitored yet, the tailwind.uss file will remain empty.

## Configure monitored paths

Navigate to `Tools/Tailwind/Configure Tailwind`.

![Tailwind Config Editor](./Packages/com.ngc-corp.unity-tailwindcss/Documentation/image.png)

Here, you can set paths to be monitored for changes in `.uxml` and `.cs` files. If any file in these directories changes, tailwind.uss will be rebuilt.

## Add tailwind.uss to your theme

For the final step to use Tailwind classes in Unity, you need to add the `tailwind.uss` stylesheet to your runtime theme, as shown in the screenshot. By default, your theme is located at `UI Toolkit/UnityThemes/UnityDefaultRuntimeTheme`.

![Adding tailwind.uss to your Theme Style Sheet](./Packages/com.ngc-corp.unity-tailwindcss/Documentation/image2.png)

If you don't want to use Unity's default styles, you can remove the default theme. If you're interested in how Unity styles its components, like buttons, you can check here: UnityEngine.UIElements.uss. This is useful when removing the default theme and wanting to replicate Unity’s default styles with Tailwind classes.

## Example

Let's assume you have an `Overlay.uxml` file and a script called `UIOverlay.cs` that adds buttons for the main menu.

```xml
<engine:UXML xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
  xmlns:engine="UnityEngine.UIElements"
  xmlns:editor="UnityEditor.UIElements"
  noNamespaceSchemaLocation="../../UIElementsSchema/UIElements.xsd"
  editor-extension-mode="False"
>
  <engine:VisualElement name="overlay-wrapper" class="grow justify-center items-center" />
</engine:UXML>
```

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Zom {
  public class UIOverlay : MonoBehaviour {
    public void Show(Dictionary<string, Action> actions) {
      UIDocument uIDocument = GetComponent<UIDocument>();
      VisualElement visualElement = uIDocument.rootVisualElement.Q<VisualElement>("overlay-wrapper");

      foreach (KeyValuePair<string, Action> pair in actions) {
        Button button = new();

        button.AddToClassList("px-3");
        button.AddToClassList("py-2");
        button.AddToClassList("text-base");
        button.AddToClassList("rounded-xl");
        button.AddToClassList("border-0");

        // Unity does not support / in class names, so we've implemented a USS-compatible logic to ensure opacity can be set for color, background-color, and border-color.
        button.AddToClassList("bg-stone-950-opacity-50"); // instead of bg-stone-950/50
        button.AddToClassList("text-white-opacity-40"); // instead of text-white/40
        button.AddToClassList("border-white-opacity-80"); // instead of border-white/80

        // Text alignment is implemented through a custom plugin because Unity uses a unique format for text alignment (-unity-text-align)
        button.AddToClassList("text-middle-center");

        // Unity doesn't support ":" as the default separator, so we use "_" instead.
        // For example, use `hover_bg-white` instead of `hover:bg-white`.
        button.AddToClassList("hover_bg-white");
        button.AddToClassList("hover_text-black");

        button.text = pair.Key;
        button.RegisterCallback<ClickEvent>(ev => pair.Value());

        visualElement.Add(button);
      }
    }
  }
}
```

If these files are within the configured monitored folders, the USS file will be generated in `Assets/TailwindCSS/tailwind.uss`.

Which results in the following UI.

![Example Main Menu](./Packages/com.ngc-corp.unity-tailwindcss/Documentation/example-result.png)

## Core Plugins

USS supports these core plugins out of the box. However, there are some limitations with certain plugins. For example, `border-e-green-800` won't work, but `border-green-800` will. Currently, there are no converters for unsupported core plugins, but this is subject to change as the library evolves during its use in an active project. If you need a specific core plugin for your project, feel free to open an issue, and I'll do my best to make it compatible.

Properties marked as custom differ from the default Tailwind behavior. All custom properties are explained in the example.

| Property                 | Supported |
|--------------------------|-----------|
| accentColor              | ❌         |
| accessibility            | ❌         |
| alignContent             | ✅         |
| alignItems               | ✅         |
| alignSelf                | ✅         |
| animation                | ❌         |
| appearance               | ❌         |
| aspectRatio              | ❌         |
| backdropBlur             | ❌         |
| backdropBrightness       | ❌         |
| backdropContrast         | ❌         |
| backdropFilter           | ❌         |
| backdropGrayscale        | ❌         |
| backdropHueRotate        | ❌         |
| backdropInvert           | ❌         |
| backdropOpacity          | ❌         |
| backdropSaturate         | ❌         |
| backdropSepia            | ❌         |
| backgroundAttachment     | ❌         |
| backgroundBlendMode      | ❌         |
| backgroundClip           | ❌         |
| backgroundColor          | ✅         |
| backgroundImage          | ❌         |
| backgroundOpacity        | ❌         |
| backgroundOrigin         | ❌         |
| backgroundPosition       | ✅         |
| backgroundRepeat         | ✅         |
| backgroundSize           | ✅         |
| blur                     | ❌         |
| borderCollapse           | ❌         |
| borderColor              | ✅         |
| borderOpacity            | ❌         |
| borderRadius             | ✅         |
| borderSpacing            | ❌         |
| borderStyle              | ❌         |
| borderWidth              | ✅         |
| boxDecorationBreak       | ❌         |
| boxShadow                | ❌         |
| boxShadowColor           | ✅         |
| boxSizing                | ❌         |
| breakAfter               | ❌         |
| breakBefore              | ❌         |
| breakInside              | ❌         |
| brightness               | ❌         |
| captionSide              | ❌         |
| caretColor               | ❌         |
| clear                    | ❌         |
| columns                  | ❌         |
| contain                  | ❌         |
| container                | ✅         |
| content                  | ❌         |
| contrast                 | ❌         |
| cursor                   | ✅         |
| display                  | ✅         |
| divideColor              | ❌         |
| divideOpacity            | ❌         |
| divideStyle              | ❌         |
| divideWidth              | ❌         |
| dropShadow               | ❌         |
| fill                     | ❌         |
| filter                   | ❌         |
| flex                     | ✅         |
| flexBasis                | ✅         |
| flexDirection            | ✅         |
| flexGrow                 | ✅         |
| flexShrink               | ✅         |
| flexWrap                 | ✅         |
| float                    | ❌         |
| fontFamily               | ❌         |
| fontSize                 | ✅         |
| fontSmoothing            | ❌         |
| fontStyle                | ❌         |
| fontVariantNumeric       | ❌         |
| fontWeight               | ❌         |
| forcedColorAdjust        | ❌         |
| gap                      | ❌         |
| gradientColorStops       | ❌         |
| grayscale                | ❌         |
| gridAutoColumns          | ❌         |
| gridAutoFlow             | ❌         |
| gridAutoRows             | ❌         |
| gridColumn               | ❌         |
| gridColumnEnd            | ❌         |
| gridColumnStart          | ❌         |
| gridRow                  | ❌         |
| gridRowEnd               | ❌         |
| gridRowStart             | ❌         |
| gridTemplateColumns      | ❌         |
| gridTemplateRows         | ❌         |
| height                   | ✅         |
| hueRotate                | ❌         |
| hyphens                  | ❌         |
| inset                    | ❌         |
| invert                   | ❌         |
| isolation                | ❌         |
| justifyContent           | ✅         |
| justifyItems             | ✅         |
| justifySelf              | ✅         |
| letterSpacing            | ✅         |
| lineClamp                | ❌         |
| lineHeight               | ❌         |
| listStyleImage           | ❌         |
| listStylePosition        | ❌         |
| listStyleType            | ❌         |
| margin                   | ✅         |
| maxHeight                | ✅         |
| maxWidth                 | ✅         |
| minHeight                | ✅         |
| minWidth                 | ✅         |
| mixBlendMode             | ❌         |
| objectFit                | ❌         |
| objectPosition           | ❌         |
| opacity                  | ✅         |
| order                    | ❌         |
| outlineColor             | ❌         |
| outlineOffset            | ❌         |
| outlineStyle             | ❌         |
| outlineWidth             | ❌         |
| overflow                 | ✅         |
| overscrollBehavior       | ❌         |
| padding                  | ✅         |
| placeContent             | ❌         |
| placeItems               | ❌         |
| placeSelf                | ❌         |
| placeholderColor         | ❌         |
| placeholderOpacity       | ❌         |
| pointerEvents            | ❌         |
| position                 | ✅         |
| preflight                | ❌         |
| resize                   | ❌         |
| ringColor                | ❌         |
| ringOffsetColor          | ❌         |
| ringOffsetWidth          | ❌         |
| ringOpacity              | ❌         |
| ringWidth                | ❌         |
| rotate                   | ❌         |
| saturate                 | ❌         |
| scale                    | ❌         |
| scrollBehavior           | ❌         |
| scrollMargin             | ❌         |
| scrollPadding            | ❌         |
| scrollSnapAlign          | ❌         |
| scrollSnapStop           | ❌         |
| scrollSnapType           | ❌         |
| sepia                    | ❌         |
| size                     | ✅         |
| skew                     | ❌         |
| space                    | ❌         |
| stroke                   | ❌         |
| strokeWidth              | ❌         |
| tableLayout              | ❌         |
| textAlign (custom)       | ✅         |
| textColor                | ✅         |
| textDecoration           | ❌         |
| textDecorationColor      | ❌         |
| textDecorationStyle      | ❌         |
| textDecorationThickness  | ❌         |
| textIndent               | ❌         |
| textOpacity              | ❌         |
| textOverflow             | ✅         |
| textTransform            | ❌         |
| textUnderlineOffset      | ❌         |
| textWrap                 | ❌         |
| touchAction              | ❌         |
| transform                | ❌         |
| transformOrigin          | ✅         |
| transitionDelay          | ✅         |
| transitionDuration       | ✅         |
| transitionProperty       | ✅         |
| transitionTimingFunction | ✅         |
| translate                | ❌         |
| userSelect               | ❌         |
| verticalAlign            | ❌         |
| visibility               | ✅         |
| whitespace               | ✅         |
| width                    | ✅         |
| willChange               | ❌         |
| wordBreak                | ❌         |
| zIndex                   | ❌         |
