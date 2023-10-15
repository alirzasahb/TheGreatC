using System;
using System.Collections.Generic;

namespace TheGreatC.Commands
{
    /// <summary>
    /// Contains the implementations of the built in commands.
    /// </summary>
    public static class BuiltIn
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
    }
}