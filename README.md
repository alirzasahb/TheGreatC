# The Great C
>  A Simple Solution To Create And Extend Your Own Command Line Interface In C#  🖥️

## Usage 🔮

There are two primary methods for defining your commands:

I. You can define your commands directly within the AlliedMastercomputer.cs file, located in the 'TheGreatC.Commands' project. As you may know, the AlliedMastercomputer class serves as the default command library, and you are welcome to add additional commands within this class.

II. Alternatively, you have the option to create your own command library classes by establishing a 'Class Library' project. After doing so, update the CommandsNameSpace value in the sharedconfiguration.config file from the 'TheGreatC.Common' project. Once you've completed these steps, make sure to copy the project DLL into the 'TheGreatC' bin folder.

### How To Define Commands?

***Default Commands Library (AlliedMastercomputer.cs File)***
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

### How To Excute My Command?

***Default Command Library (AlliedMastercomputer.cs File)***
```
→ DoSomething 55 "My Data"
```
***Your Own Command Library***
```
→ myCommandsLib.DoSomething 55 "My Data"
```

## Credits 📚
Brought forth from the depths of the ['ConsoleApplicationBase'](https://github.com/TypecastException/ConsoleApplicationBase) project, emerges a captivating piece of work crafted by the ingenious mind of the mastermind, [John Atten](https://github.com/TypecastException/).
</br>

Ascii Arts Extracted From [ascii-art.de](http://www.ascii-art.de/)
* Desert - Bob Allison
* UFO - unknown
* Universe - unknown
