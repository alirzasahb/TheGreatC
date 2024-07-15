# The Great C
>  A Simple Solution To Create And Extend Your Own Command Line Interface In C#  🖥️

## Usage 🔮

Unleash Your Creativity:

* Way of the Innovator: Define Your Commands in AlliedMastercomputer.cs, located inside 'TheGreatC.Commands' Project. AlliedMastercomputer, the ultimate command library, allows you to customize your commands even further. Expand the possibilities!

* Forge Your Own Path: Create Command Libraries of your own! Start by generating a 'Class Library' Project. In the sharedconfiguration.config file of the 'TheGreatC.Common' Project, modify the 'CommandsNameSpace' value. Next, ensure your Project DLL finds its new home in the 'TheGreatC' Bin Folder. Embrace the boundless potential of your unique command library class(es). No limits, only possibilities!

### How To Define My Command?

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
