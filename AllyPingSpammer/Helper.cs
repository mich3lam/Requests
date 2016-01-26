// ReSharper disable InconsistentNaming
// ReSharper disable once CheckNamespace
namespace SparkTech.Helpers
{
    using System.Linq;
    using System.Text.RegularExpressions;

    using LeagueSharp;
    using LeagueSharp.SDK.Core.UI.IMenu;
    using LeagueSharp.SDK.Core.UI.IMenu.Abstracts;
    using LeagueSharp.SDK.Core.UI.IMenu.Values;

    /// <summary>
    /// Provides miscallenous methods and extensions
    /// </summary>
    public static class Helper
    {
        private static ushort i;

        /// <summary>
        /// The array containing AD Carry names
        /// </summary>
        private static readonly string[] ADCs =
        {
            "Caitlyn", "Corki", "Draven", "Ashe", "Ezreal", "Graves", "Jinx",
            "Kalista", "Kog'Maw", "Lucian", "Miss Fortune", "Quinn", "Sivir",
            "Teemo", "Tristana", "Twitch", "Varus", "Vayne"
        };

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
        /// Determines whether this <see cref="Obj_AI_Hero"/> instance is an AD Carry
        /// </summary>
        /// <param name="hero"><see cref="Obj_AI_Hero"/> instance</param>
        /// <returns></returns>
        public static bool IsADC(this Obj_AI_Hero hero)
        {
            return ADCs.Contains(hero.ChampionName());
        }

        /// <summary>
        /// Converts this string instance usable for the menu
        /// </summary>
        /// <param name="input">The <see cref="string"/> instance</param>
        /// <returns></returns>
        public static string ToMenuUse(this string input)
        {
            return input.Space().ToLower().Replace('\'', ' ').Trim().Replace(' ', '_');
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
        /// Determined whether the game is funning in FullHD
        /// </summary>
        /// <returns></returns>
        public static bool FullHD()
        {
            return Drawing.Height == 1920 && Drawing.Width == 1080;
        }

        /// <summary>
        /// Gets the <see cref="AMenuComponent"/> globally
        /// </summary>
        /// <typeparam name="TMenuComponent">The desired type</typeparam>
        /// <param name="name">The <see cref="AMenuComponent"/> name</param>
        /// <returns></returns>
        public static TMenuComponent GetGlobally<TMenuComponent>(string name) where TMenuComponent : AMenuComponent
        {
            return
                (TMenuComponent)
                MenuManager.Instance.Menus.Where(menu => menu.Root)
                    .SelectMany(rootmenu => rootmenu.Components.Values) //.ToHashSet()
                    .FirstOrDefault(component => component.Name == name && component is TMenuComponent);
        }
    }
}