using System;
using System.Collections.Generic;
using System.Linq;
using TheGreatC.Common.Internal.Utilities;

namespace TheGreatC.Commands.Internal
{
    /// <summary>
    /// Contains the implementations of the built-in commands.
    /// Referenced After Allied Mastercomputer From I Have No Mouth, and I Must Scream By Harlan Ellison
    /// </summary>
    public static class AlliedMastercomputer
    {
        public static List<string> About()
        {
            Console.Clear();
            var text = new List<string>
            {
                @"   .              +   .                .   . .     .  .      .   .      ",
                @"                     .                    .       .     *         . .   ",
                @"    .       *                        . . . .  .   .  + .        .  . .  ",
                @"              'You Are Here'            .   .  +  . . .   *          *  ",
                @"  .                 |             .  .   .    .    . .                  ",
                @"                    |           .     .     . +.    +  .      +       . ",
                @"                   \|/            .       .   . .                       ",
                @"          . .       V          .    * . . .  .  +   .   .  .  *      .  ",
                @"             +      .           .   .      +          .  . .     *      ",
                @"                              .       . +  .+. .      .   .    .     .  ",
                @"    .                      .     . + .  . .     .      .          *     ",
                @"             .      .    .     . .   . . .    .    ! /     .   . .   .  ",
                @"        *             .    . .  +    .  .    .   - O -             *    ",
                @"            .     .    .  +   . .  *  .    .   . / |        +           ",
                @"                 . + .  .  .  .. +  .                  *         .  .   ",
                @"  .      .  .  .  *   .  *  . +..  .      +      *             *        ",
                @"   .      .   . .   .   .   . .  +   .    .            +    . . .       ",
                @"                                                                        ",
                @"'The Great C' is a science fiction short story by American writer       ",
                @" Philip K. Dick, first published in Cosmos Science Fiction and Fantasy  ",
                @" Magazine in 1953.                                                      ",
                @"                                                                        ",
                @" Developer: http://github.com/alirzasahb                                ",
                @"                                                                        ",
                @" Extracted And Developed From 'ConsoleApplicationBase' Project,         ",
                @" Original Author John Atten:                                            ",
                @" https://github.com/TypecastException/ConsoleApplicationBase            ",
                @"                                                                        ",
                @" ASCII Arts (http://www.ascii-art.de/):                                 ",
                @" Desert - Bob Allison                                ",
                @" UFO - unknown                                       ",
                @" Universe - unknown                                  "

            };

            return text;
        }

        /// <summary>
        /// Clear Console Screen
        /// </summary>
        public static void Clear()
        {
            Console.Clear();
        }

        /// <summary>
        /// Exit Application
        /// </summary>
        public static void Exit()
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// The Questions That Tim Meredith Asked From The Great C in the Ruins of Federal Research Station 7
        /// </summary>
        public static void Ask()
        {
            var questions = new List<string>() { "Where does the rain come from?", "What keeps the sun moving through the sky?", "How did the world begin?" };

            SpectreConsoleWriter.Write(SpectreConsoleWriter.SpectreWritingType.Info, questions.OrderBy(s => Guid.NewGuid()).First());
        }
    }
}