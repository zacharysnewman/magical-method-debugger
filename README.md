# Magical Method Debugger

A Unity package that adds a debug interface to MonoBehaviour inspectors, allowing you to invoke methods directly from the editor.

## Features

- Invoke public and private methods directly from the Unity Inspector
- Support for parameters of various types (primitives, Unity types, enums, etc.)
- Continuous method invocation for methods that need to be called repeatedly
- Categorization of methods (Public, Private, Inherited, Unity methods)
- Option to show unsupported methods
- Open methods in your code editor

## Installation

### Via Unity Package Manager (Git URL)

1. Open Unity Package Manager (Window > Package Manager)
2. Click the "+" button and select "Add package from git URL"
3. Enter the following URL: `https://github.com/zacharysnewman/magical-method-debugger.git`
4. Click "Add"

### Manual Installation

1. Download or clone the repository
2. Copy the `Packages/com.zacharysnewman.mmd` folder to your Unity project's `Packages` folder

## Usage

Once installed, the Magical Method Debugger will automatically appear in the Inspector for any MonoBehaviour.

1. Select a GameObject with a MonoBehaviour script attached
2. In the Inspector for any MonoBehaviour derived component, scroll down to see the "Method Debugger" section at the bottom of each component
3. Expand the categories (Public, Private, Inherited, MonoBehaviour) to see available methods
4. For methods with parameters, expand the method foldout to set parameter values
5. Click ▶¹ to call the method once, or click ▶ to enable continuous invocation (■ when active)
6. Click → to jump to the method definition in your code editor

## Supported Parameter Types

The debugger supports the following parameter types:

- Primitive types: `int`, `float`, `bool`, `string`, `decimal`
- Unity types: `Vector2`, `Vector3`, `Vector4`, `Quaternion`, `Color`, `Color32`, `Rect`, `Bounds`, `AnimationCurve`, `Gradient`, `LayerMask`
- Unity Objects: Any type inheriting from `Object` (e.g., `GameObject`, `Transform`, `Material`)
- Enums: Any enum type
- Arrays and Lists: Of supported element types (displayed as text fields)

## Requirements

- Unity 2020.3 or later
- Works only in the Unity Editor

## Notes

- Methods with unsupported parameter types are hidden by default but can be shown by toggling "Show Unsupported"
- Continuous invocation runs once for each game update frame, useful for methods that need to be called frequently during development
- Parameter values are remembered per method and persist between editor sessions
- Methods can only be invoked while in Play mode; buttons are disabled otherwise

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
