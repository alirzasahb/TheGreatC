using System.Collections.Generic;
using System.Text.RegularExpressions;
using TheGreatC.Common;

namespace TheGreatC.Domain.Models
{
    public class Command
    {
        public string Name { get; set; }
        public string LibraryClassName { get; set; }

        private readonly List<string> _arguments;
        public IEnumerable<string> Arguments => _arguments;

        public Command(string input)
        {
            // regex to split string on spaces, but preserve quoted text intact:
            var stringArray = Regex.Split(input,
                "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

            _arguments = new List<string>();
            for (var i = 0; i < stringArray.Length; i++)
            {
                // The first element is always the command:
                if (i == 0)
                {
                    Name = stringArray[i];

                    // Set the default:
                    LibraryClassName = ConfigurationManager.SharedConfigurations["Settings:InternalLibraryClassName"];
                    var s = stringArray[0].Split('.');
                    if (s.Length != 2) continue;
                    LibraryClassName = s[0];
                    Name = s[1];
                }
                else
                {
                    var inputArgument = stringArray[i];
                    var argument = inputArgument;

                    // Is the argument a quoted text string?
                    var regex = new Regex("\"(.*?)\"", RegexOptions.Singleline);
                    var match = regex.Match(inputArgument);

                    if (match.Captures.Count > 0)
                    {
                        // Get the unquoted text:
                        var captureQuotedText = new Regex("[^\"]*[^\"]");
                        var quoted = captureQuotedText.Match(match.Captures[0].Value);
                        argument = quoted.Captures[0].Value;
                    }
                    _arguments.Add(argument);
                }
            }
        }
    }
}