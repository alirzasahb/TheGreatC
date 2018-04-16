using System;
using System.Collections.Generic;

namespace TheGreatC.Commands
{
    /// <summary>
    /// Contains the implementations of the built-in commands.
    /// </summary>
    public static class Default
    {
        public static List<string> Credits()
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
                @" The Name 'The Great C' Was Inspired By A Short Story With Same Exact   ",
                @" Name Written By 'Philip K. Dick'.                                      ",
                @"                                                                        ",
                @" Developer: http://github.com/alirzasahb                                ",
                @"                                                                        ",
                @" Extracted And Developed From 'ConsoleApplicationBase' Project,         ",
                @" Original Author John Atten:                                            ",
                @" https://github.com/TypecastException/ConsoleApplicationBase            ",
                @"                                                                        ",
                @" Ascii Arts (http://www.ascii-art.de/):                                 ",
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
