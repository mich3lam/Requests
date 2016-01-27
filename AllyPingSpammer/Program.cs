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
    
    internal class Program
    {
        /// <summary>
        /// The entry point of the assembly
        /// </summary>
        /// <param name="args">The empty string array</param>
        private static void Main(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            Events.OnLoad += delegate
                {
                    const string Header = "st_ally_ping";
                    var shouldRandomize1 = false;
                    var shouldRandomize2 = false;
                    var shouldAssign = false;
                    var random = new Random(Variables.TickCount);
                    var root = new Menu(Header, "[ST] Ally Ping Spammer", true).Attach();
                    var active = root.Add(new MenuBool(Header + "_active", "Active"));
                    var hero = root.Add(new MenuList<string>(Header + "_hero", "Ally to be spammed", GameObjects.AllyHeroes.Where(ally => !ally.IsMe).Select(ally => ally.ChampionName())));
                    var delay = root.Add(new MenuSlider(Header + "_delay", "Delay between attempts", 1200, 200, 5000));
                    var randomizer1 = root.Add(new MenuBool(Header + "_randomize1", "^ Randomize delay"));
                    var ping = root.Add(new MenuList<PingCategory>(Header + "_pingtype", nameof(PingCategory)));
                    var randomizer2 = root.Add(new MenuBool(Header + "_randomize2", "^ Randomize " + nameof(PingCategory)));
                    root.AddSeparator("Made by Spark");

                    var operation = new TickOperation(
                        delay.Value,
                        delegate
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

                                if (randomizer1.Value)
                                {
                                    shouldRandomize1 = true;
                                }

                                if (randomizer2.Value)
                                {
                                    shouldRandomize2 = true;
                                }
                            });

                    delay.ValueChanged += delegate
                        { shouldAssign = true; };

                    Game.OnUpdate += delegate
                        {
                            if (shouldRandomize1)
                            {
                                shouldRandomize1 = false;
                                operation.TickDelay = delay.Value = random.Next(200, 5000);
                            }

                            if (shouldRandomize2)
                            {
                                shouldRandomize2 = false;
                                var array = Enum.GetValues(typeof(PingCategory));
                                ping.SelectedValue = (PingCategory)array.GetValue(random.Next(0, array.Length - 1));
                            }

                            if (!shouldAssign)
                            {
                                return;
                            }

                            shouldAssign = false;
                            operation.TickDelay = delay.Value;
                        };
                };
        }
    }
}