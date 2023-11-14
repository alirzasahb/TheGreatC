# The Great C
> Command Line Interface (CLI) Creation Unlimited! 💻

## Usage

Unleash Your Creativity:

* Way of the Innovator: Define Your Commands in AlliedMastercomputer.cs, located inside 'TheGreatC.Commands' Project. AlliedMastercomputer, the ultimate command library, allows you to customize your commands even further. Expand the possibilities!

* Forge Your Own Path: Create Command Libraries of your own! Start by generating a 'Class Library' Project. In the sharedconfiguration.config file of the 'TheGreatC.Common' Project, modify the 'CommandsNameSpace' value. Next, ensure your Project DLL finds its new home in the 'TheGreatC' Bin Folder. Embrace the boundless potential of your unique command library class(es). No limits, only possibilities!

**How To Define My Command?**
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

**How To Excute My Command?**
</br>
</br>
***Default Command Library***
```
→ DoSomething 55 "My Data"
```
***Your Own Command Library***
```
→ myCommandsLib.DoSomething 55 "My Data"
```

## Credits 📚🔮
Brought forth from the depths of the 'ConsoleApplicationBase' project, emerges a captivating piece of work crafted by the ingenious mind of the mastermind, John Atten.
</br>
 https://github.com/TypecastException/ConsoleApplicationBase
</br>
</br>
Ascii Arts Extracted From: http://www.ascii-art.de/
</br>
Desert - Bob Allison
</br>
UFO - unknown
</br>
Universe - unknown
