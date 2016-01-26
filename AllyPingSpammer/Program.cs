namespace AllyPingSpammer
{
    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.SDK;
    using LeagueSharp.SDK.Core.UI.IMenu;
    using LeagueSharp.SDK.Core.UI.IMenu.Values;
    using LeagueSharp.SDK.Core.Utils;

    using SparkTech.Helpers;

    /// <summary>
    /// The <see cref="Program"/> class
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The beginning for every menu item
        /// </summary>
        private const string Header = "st_ally_ping";

        /// <summary>
        /// The <see cref="Random"/> instance
        /// </summary>
        private static readonly Random Random = new Random(Variables.TickCount);

        /// <summary>
        /// The entry point
        /// </summary>
        /// <param name="args">The empty string <see cref="Array"/></param>
        private static void Main(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            Events.OnLoad += delegate
                {
                    bool shouldRandomize = false, shouldAssign = false;

                    var root = new Menu(Header, "[ST] Ally Ping Spammer", true).Attach();

                    var active = root.Add(new MenuBool(Header + "_active", "Active"));

                    var hero =
                        root.Add(
                            new MenuList<string>(
                                Header + "_hero",
                                "Ally to be spammed",
                                GameObjects.AllyHeroes.Where(ally => !ally.IsMe).Select(ally => ally.ChampionName())));

                    var delay = root.Add(new MenuSlider(Header + "_delay", "Delay between attempts", 800, 200, 5000));

                    var randomizer = root.Add(new MenuBool(Header + "_randomize", "^ Randomize delay"));

                    var ping = root.Add(new MenuList<PingCategory>(Header + "_pingtype", nameof(PingCategory)));

                    root.AddSeparator("Made by Spark");

                    var operation = new TickOperation(delay.Value, delegate
                            {
                                if (!active.Value)
                                {
                                    return;
                                }

                                var position =
                                    GameObjects.AllyHeroes.FirstOrDefault(
                                        champ => champ.ChampionName() == hero.SelectedValue && !champ.IsMe)?.Position;

                                if (position.HasValue)
                                {
                                    Game.SendPing(ping.SelectedValue, position.Value);
                                }
                                
                                if (randomizer.Value)
                                {
                                    shouldRandomize = true;
                                }
                            });

                    delay.ValueChanged += delegate
                        { shouldAssign = true; };

                    Game.OnUpdate += delegate
                        {
                            if (shouldRandomize)
                            {
                                operation.TickDelay = delay.Value = Random.Next(200, 5000);
                                shouldRandomize = false;
                            }

                            if (!shouldAssign)
                            {
                                return;
                            }

                            operation.TickDelay = delay.Value;
                            shouldAssign = false;
                        };
                };
        }
    }
}