# AttachedProperties ![Build](https://github.com/nwendel/attachedproperties/actions/workflows/build.yml/badge.svg) [![Coverage](https://codecov.io/gh/nwendel/attachedproperties/branch/main/graph/badge.svg?token=BMNOSIWUMV)](https://codecov.io/gh/nwendel/attachedproperties)

A small library for dynamically adding attached properties to any existing instance without creating a derived type.  It works similar in nature to WPF's attached dependency properties however it can be used on any type and not just types deriving from DependencyObject.

### NuGet Package

```
Install-Package AttachedProperties
```

### Example
```csharp
public class Example
{

    public AttachedProperty<string, Color> ForegroundColor =
        new AttachedProperty<string, Color>(nameof(ForegroundColor));
    
    public void SomeMethod()
    {
    	var text = "Some value";
        text.SetAttachedValue(ForegroundColor, Color.White);

        WriteLineWithColor(text);
    }
    
    public void WriteLineWithColor(string text)
    {
        var foregroundColor = text.GetAttachedValue(ForegroundColor);
        Console.ForegoundColor = foregroundColor;
        Console.WriteLine(text);    	
    }

}
```

### When to use AttachedProperties?

There are some situations when there is a need to add additional state to an existing instance.  In most cases it is possible to add a property either directtly to the class or in a derived class.  But this is not always possible, for example:
* Class defintion is sealed and/or in a third party library (inclding .NET Framework) and cannot be changed.
* The creation of the instance where additional state is needed is not possible to change and there are other dependencies which are not expecting changes to the instance.
* Where there is no wish to change the class since the attached property is just used in a very specific part of the solution.
