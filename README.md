# The Great C
> A Simple Solution To Create And Extend Your Own Command Line Interface In C#
> DotNET Core Edition

## Usage
**Assumptions:**
</br>
Commands Can Be Defined In Diffrent Ways:
</br>
</br>
First Way:
You Can Define Your Commands In `Commands.cs` Inside 'TheGreatC.Commands' Project.
</br>
The 'Default' Class As You Probably Thought Is Default Command Library,
You Can Define More Commands In 'Default' Command Library Class.
</br>
</br>
Second Way:
You Can Create Your Own Command Library Class(es) By Creating A 'Class Libraray' Project And
</br>
Change 'CommandsNameSpace' Value In `App.config` From 'TheGreatC' Project. Then Copy Project
</br>
DLL To 'TheGreatC' Bin Folder.
</br>

**Wait, How To Define My Command?**
</br>
</br>
***Default Commands Library***
</br>

```csharp
public static string DoSomething(int id, string data="sampleData")
{
    return string.Format(ConsoleFormatting.Indent(2) + 
        "This Is Something {0} And Now Save This Data '{1}'", id, data);
}
```

***Your Own Commands Library***
</br>

```csharp
// First Define Command Library Class
public class myCommandsLib{
    // Your Command
    public static string DoSomething(int id, string data="sampleData")
    {
        return string.Format(ConsoleFormatting.Indent(2) + 
            "This Is Something {0} And Now Save This Data '{1}'", id, data);
    }
}
```

**Umm, How To Excute My Command?**
</br>
</br>
***Default Command Library***
```
-> DoSomething 55 "My Data"
```
***Your Own Command Library***
```
-> myCommandsLib.DoSomething 55 "My Data"
```

## Credits 📚
Extracted And Developed From 'ConsoleApplicationBase' Project, Original Author John Atten:
</br>
 https://github.com/TypecastException/ConsoleApplicationBase
