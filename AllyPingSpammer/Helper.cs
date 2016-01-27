// ReSharper disable InconsistentNaming
// ReSharper disable once CheckNamespace
namespace SparkTech.Helpers
{
    using System;
    using System.Text.RegularExpressions;

    using LeagueSharp;
    using LeagueSharp.SDK.Core.UI.IMenu;
    using LeagueSharp.SDK.Core.UI.IMenu.Values;

    using SharpDX;

    /// <summary>
    /// Provides miscallenous methods and extensions
    /// </summary>
    public static class Helper
    {
        private static Random Random => AllyPingSpammer.Program.Random;

        private static ushort i;

        /// <summary>
        /// Gets the real champion name
        /// </summary>
        /// <param name="champ"><see cref="Obj_AI_Hero"/> instance</param>
        /// <returns></returns>
        public static string ChampionName(this Obj_AI_Hero champ)
        {
            var name = champ.ChampionName;

            switch (name.ToLower())
            {
                case "chogath":
                    return "Cho'Gath";
                case "drmundo":
                    return "Dr. Mundo";
                case "khazix":
                    return "Kha'Zix";
                case "kogmaw":
                    return "Kog'Maw";
                case "reksai":
                    return "Rek'Sai";
                case "velkoz":
                    return "Vel'Koz";
                default:
                    return name.Space();
            }
        }

        /// <summary>
        /// Spaces the input string
        /// </summary>
        /// <param name="input">The string to be spaced</param>
        /// <param name="ignoreAcronyms">If <c>true</c>, ignore acronyms</param>
        /// <returns></returns>
        public static string Space(this string input, bool ignoreAcronyms = true)
        {
            return ignoreAcronyms
                       ? Regex.Replace(input, "((?<=\\p{Ll})\\p{Lu})|((?!\\A)\\p{Lu}(?>\\p{Ll}))", " $0")
                       : Regex.Replace(input, "(?<!^)([A-Z])", " $1");
        }

        /// <summary>
        /// Gets the menu separator text
        /// </summary>
        public static string SeparatorText => $"st_separator_{++i}";

        /// <summary>
        /// Adds the separator to the specified <see cref="Menu"/> instance
        /// </summary>
        /// <param name="menu"><see cref="Menu"/> instance</param>
        /// <param name="text">The custom text to be displayed</param>
        public static void AddSeparator(this Menu menu, string text = null)
        {
            menu.Add(new MenuSeparator(SeparatorText, text ?? string.Empty));
        }

        /// <summary>
        /// Randomizes a <see cref="Vector3"/>
        /// </summary>
        /// <param name="input">The specified input</param>
        /// <param name="maxDiff">The maximal difference</param>
        /// <returns></returns>
        public static Vector3 Randomize(Vector3 input, int maxDiff)
        {
            input.X = Randomize(input.X, maxDiff);
            input.Y = Randomize(input.Y, maxDiff);
            input.Z = Randomize(input.Z, maxDiff);

            return input;
        }

        /// <summary>
        /// Randomizes a <see cref="float"/>
        /// </summary>
        /// <param name="input">The specified input</param>
        /// <param name="maxDiff">The maximal difference</param>
        /// <returns></returns>
        public static float Randomize(float input, int maxDiff)
        {
            var b = Random.Next(0, 1) == 0;
            var random = Random.Next(0, maxDiff);

            return b ? input + random : input - random;
        }
    }
}