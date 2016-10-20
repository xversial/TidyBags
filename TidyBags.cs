namespace PluginTidyBags3
{
    using Styx;
    using Styx.Common;
    using Styx.Common.Helpers;
    using Styx.CommonBot;
    using Styx.CommonBot.Frames;
    using Styx.CommonBot.Inventory;
    using Styx.CommonBot.Profiles;
    using Styx.Helpers;
    using Styx.Pathing;
    using Styx.Plugins;
    using Styx.WoWInternals;
    using Styx.WoWInternals.Misc;
    using Styx.WoWInternals.World;
    using Styx.WoWInternals.WoWObjects;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Windows.Media;
    using System.Xml.Linq;

    public class TidyBags3 : HBPlugin
    {
        public override string Name { get { return "Tidy Bags 4"; } }
        public override string Author { get { return "Xversial"; } }
        public override Version Version { get { return new Version(3, 6, 4, 4); } }
        public bool InventoryCheck = false;
        private bool _init;

        private void LootFinished(object sender, LuaEventArgs args)
        {
            if (InventoryCheck == false)
            {
                InventoryCheck = true;
            }
        }

        private void MailboxFinished(object sender, LuaEventArgs args)
        {
            if (InventoryCheck == false)
            {
                InventoryCheck = true;
            }
        }

        private HashSet<uint> _artifactPowerItems = new HashSet<uint>()
        {
            141888, // Discarded Aristocrat's Censer
            127999, // Shard of Potentiation
            128021, // Scroll of Enlightenment
            128022, // Treasured Coin
            128026, // Trembling Phylactery
            134118, // Cluster of Potentiation
            134133, // Jewel of Brilliance
            138726, // Shard of Potentiation
            138732, // History of the Blade
            138781, // Brief History of the Aeons
            138782, // Brief History of the Ages
            138783, // Glittering Memento
            138784, // Questor's Glory
            138785, // Adventurer's Resounding Glory
            138786, // Talisman of Victory
            138812, // Adventurer's Wisdom
            138813, // Adventurer's Resounding Renown
            138814, // Adventurer's Renown
            138816, // Adventurer's Glory
            138839, // Valiant's Glory
            138864, // Skirmisher's Advantage
            138865, // Gladiator's Triumph
            138880, // Soldier's Grit
            138881, // Soldier's Glory
            138885, // Treasure of the Ages
            138886, // Favor of Valajar
            139413, // Greater Questor's Glory
            139506, // Greater Glory of the Order
            139507, // Cracked Vrykul Insignia
            139508, // Dried Worldtree Seeds
            139509, // Worldtree Bloom
            139510, // Black Rook Soldier's Insignia
            139511, // Hallowed Runestone
            139512, // Sigilstone of Tribute
            139608, // Brittle Spelltome
            139609, // Depleted Cadet's Wand
            139610, // Musty Azsharan Grimoire
            139611, // Primitive Roggtotem
            139612, // Highmountain Mystic's Totem
            139613, // Tale-Teller's Staff
            139614, // Azsharan Manapearl
            139615, // Untapped Mana Gem
            139616, // Dropper of Nightwell Liquid
            139617, // Ancient Warden Manacles
            140176, // Accolade of Victory
            140237, // Iadreth's Enchanted Birthstone
            140238, // Scavenged Felstone
            140241, // Enchanted Moonfall Text
            140244, // Jandvick Jarl's Pendant Stone
            140247, // Mornath's Enchanted Statue
            140250, // Ingested Legion Stabilizer
            140251, // Purified Satyr Totem
            140252, // Tel'anor Ancestral Tablet
            140254, // The Seawarden's Beacon
            140255, // Enchanted Nightborne Coin
            140304, // Activated Essence
            140305, // Brimming Essence
            140306, // Mark of the Valorous
            140307, // Heart of Zin-Azshari
            140310, // Crude Statuette
            140322, // Trainer's Insight
            140349, // Spare Arcane Ward
            140372, // Ancient Artificer's Manipulator
            140381, // Jandvick Jarl's Ring, and Finger
            140384, // Azsharan Court Scepter
            140386, // Inquisitor's Shadow Orb
            140388, // Falanaar Gemstone
            140396, // Friendly Brawler's Wager
            140409, // Tome of Dimensional Awareness
            140410, // Mark of the Rogues
            140421, // Ancient Qiraji Idol
            140422, // Moonglow Idol
            140444, // Dream Tear
            140445, // Arcfruit
            140517, // Glory of the Order
            140685, // Enchanted Sunrunner Kidney
            140847, // Ancient Workshop Focusing Crystal
            141023, // Seal of Victory
            141024, // Seal of Leadership
            141310, // Falanaar Crescent
            141313, // Manafused Fal'dorei Egg Sac
            141314, // Treemender's Beacon
            141383, // Crystallized Moon Drop
            141384, // Emblem of the Dark Covenant
            141385, // Tidestone Sliver
            141386, // Giant Pearl Scepter
            141387, // Emerald Bloom
            141388, // Warden's Boon
            141389, // Stareye Gem
            141390, // The Corruptor's Totem
            141391, // Ashildir's Unending Courage
            141392, // Fragment of the Soulcage
            141393, // Onyx Arrowhead
            141394, // Plume of the Great Eagle
            141395, // Stonedark's Pledge
            141396, // The River's Blessing
            141397, // The Spiritwalker's Wisdom
            141398, // Blessing of the Watchers
            141399, // Overcharged Stormscale
            141400, // Underking's Fist
            141401, // Renewed Lifeblood
            141402, // Odyn's Watchful Gaze
            141403, // Tablet of Tyr
            141404, // Insignia of the Second Command
            141405, // Senegos' Favor
            141638, // Falanaar Scepter
            141639, // Falanaar Orb
            141667, // Ancient Keeper's Brooch
            141668, // The Arcanist's Codex
            141669, // Fel-Touched Tome
            141670, // Arcane Trap Power Core
            141671, // Moon Guard Focusing Stone
            141672, // Insignia of the Nightborne Commander
            141673, // Love-Laced Arrow
            141674, // Brand of a Blood Brother
            141675, // Deepwater Blossom
            141676, // The Valewatcher's Boon
            141677, // Key to the Bazaar
            141678, // Night Devint: The Perfection of Arcwine
            141679, // Cobalt Amber Crystal
            141680, // Titan-Forged Locket
            141681, // Valewalker Talisman
            141682, // Free Floating Ley Spark
            141683, // Mana-Injected Chronarch Power Core
            141684, // Residual Manastorm Energy
            141685, // The Valewalker's Blessing
            141689, // Jewel of Victory
            141690, // Symbol of Victory
            141699, // Boon of the Companion
            141701, // Selfless Glory
            141702, // Spoiled Manawine Dregs
            141703, // Witch-Harpy Talon
            141704, // Forgotten Offering
            141705, // Disorganized Ravings
            141706, // Carved Oaken Windchimes
            141707, // Smuggled Magical Supplies
            141708, // Curio of Neltharion
            141709, // Ancient Champion Effigy
            141710, // Discontinued Suramar City Key
            141711, // Ancient Druidic Carving
            141852, // Accolade of Heroism
            141853, // Accolade of Myth
            141854, // Accolade of Achievement
            141855, // History of the Aeons
            141856, // History of the Ages
            141857, // Soldier's Exertion
            141858, // Soldier's Worth
            141859, // Soldier's Splendor
            141863, // Daglop's Precious
            141872, // Artisan's Handiwork
            141876, // Soul-Powered Containment Unit
            141877, // Coura's Ancient Scepter
            141883, // Azsharan Keepsake
            141886, // Crackling Dragonscale
            141887, // Lucky Brulstone
            128000, // Crystal of Ensoulment
            141889, // Glory of the Melee
            141890, // Petrified Acorn
            141921, // Dessicated Blue Dragonscale
            141922, // Brulstone Fishing Sinker
            141923, // Petrified Axe Haft
            141924, // Broken Control Mechanism
            141925, // Pruned Nightmare Shoot
            141926, // Druidic Molting
            141927, // Burrowing Worm Mandible
            141928, // Reaver's Harpoon Head
            141929, // Hippogryph Plumage
            141930, // Smolderhide Spirit Beads
            141931, // Tattered Farondis Heraldry
            141932, // Shard of Compacted Energy
            141933, // Citrine Telemancy Index
            141934, // Partially Enchanted Nightborne Coin
            141935, // Enchrgled Mlrgmlrg of Enderglment
            141936, // Petrified Fel-Heart
            141937, // Eredari Ignition Crystalkd
            141940, // Starsong's Bauble
            141941, // Crystallized Sablehorn Antler
            141942, // Managazer's Petrifying Eye
            141943, // Moon Guard Power Gem
            141944, // Empowered Half-Shell
            141945, // Magically-Fortified Vial
            141946, // Trident of Sashj'tar
            141947, // Mark of Lunastre
            141948, // Token of a Master Cultivator
            141949, // Everburning Arcane Glowlamp
            141950, // Arcane Seed Case
            141951, // Spellbound Jewelry Box
            141952, // Delving Deeper by Arcanist Perclanea
            141953, // Nightglow Energy Vessel
            141954, // 'Borrowed' Highborne Magi's Chalice
            141955, // Corupted Duskmere Crest
            141956, // Rotten Spellbook
            142001, // Antler of Cenarius
            142002, // Dragonscale of the Earth Aspect
            142003, // Talisman of the Ascended
            142004, // Nar'thalas Research Tome
            142005, // Vial of Diluted Nightwell Liquid
            142006, // Ceremonial Warden Glaive
            142007, // Omnibus: The Schools of Arcane Magic
            142054, // Enchanted Nightborne Coin
            132361, // Petrified Arkhana
            130144, // Crystallized Fey Darter Egg
            130159, // Ravencrest Shield
            130160, // Vial of Pure Moonrest Water
            130165, // Heathrow Keepsake
            131728, // Urn of Malgalor's Blood
            131753, // Prayers to the Earthmother
            131758, // Oversized Acorn
            131778, // Woodcarved Rabbit
            131784, // Left Half of a Locket
            131785, // Right Half of a Locket
            131789, // Handmade Mobile
            131808, // Engraved Bloodtotem Armlet
            130153, // Godafoss Essence
            132923, // Hrydshal Etching
            130149, // Carved Smolderhide Figurines
            130152, // Condensed Light of Elune
            131763, // Bundle of Trueshot Arrows
            131795, // Nar'thalasian Corsage
            131802, // Offering to Ram'Pag
            131751, // Fractured Portal Shard
            132950, // Petrified Snake
            141891, // Branch of Shaladrassil
            141892, // Gilbert's Finest
            141896, // Nashal's Spyglass
            132897, // Mandate of the Watchers
            138480, // Black Harvest Tome
            138487, // Shinfel's Staff of Torment
            140357, // Fel Lava Rock
            140358, // Eredar Armor Clasp
            140359, // Darkened Eyeball
            140361, // Pulsating Runestone
            140364, // Frostwyrm Bone Fragment
            140365, // Dried Stratholme Lily
            140366, // Scarlet Hymnal
            140367, // Tattered Sheet Music
            140368, // Tarnished Engagement Ring
            140369, // Scrawled Recipe
            140370, // Amber Shard
            140371, // Letter from Exarch Maladaar
            140373, // Ornamented Boot Strap
            140374, // Jagged Worgen Fang
            140377, // Broken Medallion of Karabor
            140379, // Broken Warden Glaive Blade
            140380, // Swiftflight's Tail Feather
            140382, // Tiny War Drum
            140383, // Glowing Cave Mushroom
            140385, // Legion Pamphlet
            140387, // Bracer Gemstone
            140389, // Petrified Flame
            140391, // Argussian Diamond
            140392, // Safety Valve
            140393, // Repentia's Whip
            140459, // Moon Lily
            140460, // Thisalee's Fighting Claws
            140532, // Inscribed Vrykul Runestone
            140462, // Draketaming Spurs
            140463, // Broken Eredar Blade
            140466, // Corroded Eternium Rose
            140467, // Fel-Infused Shell
            140468, // Eagle Eggshell Fragment
            140469, // Felslate Arrowhead
            140470, // Ancient Gilnean Locket
            140471, // Lord Shalzaru's Relic
            140473, // Night-forged Halberd
            140474, // Nar'thalas Pottery Fragment
            140475, // Morning Glory Vine
            140476, // Astranaar Globe
            140477, // Inert Ashes
            140478, // Painted Bark
            140479, // Broken Legion Communicator
            140480, // Drained Construct Core
            140481, // Shimmering Hourglass
            140482, // Storm Drake Fang
            140484, // Well-Used Drinking Horn
            140485, // Duskpelt Fang
            140486, // Storm Drake Scale
            140487, // War-Damaged Vrykul Helmet
            140488, // Huge Blacksmith's Hammer
            140489, // Ettin Toe Ring
            140490, // Wooden Snow Shoes
            140491, // Stolen Pearl Ring
            140492, // Gleaming Glacial Pebble
            140494, // Eredar Tail-Cuff
            140497, // Bundle of Tiny Spears
            140498, // Legion Admirer's Note
            140503, // Blank To-Do List
            140504, // Kvaldir Anchor Line
            140505, // Sweaty Bandanna
            140507, // Unlabeled Potion
            140508, // Nightborne Artificer's Ring
            140509, // Demon-Scrawled Drawing
            140510, // Iron Black Rook Hold Key
            140511, // Soul Shackle
            140512, // Oversized Drinking Mug
            140513, // Dreadlord's Commendation
            140516, // Elemental Bracers
            140518, // Bottled Lightning
            140519, // Whispering Totem
            140520, // Amethyst Geode
            140521, // Fire Turtle Shell Fragment
            140522, // Petrified Spiderweb
            140523, // Crimson Cavern Mushroom
            140524, // Sharp Twilight Tooth
            140525, // Obsidian Mirror
            140528, // Dalaran Wine Glass
            140529, // Felstalker's Ring
            140530, // Opalescent Shell
            140531, // Ravencrest Family Seal
            140461  // Battered Trophy
        };

        private HashSet<uint> _itemUseOnOne = new HashSet<uint>() {
            3352, // Ooze-covered Bag
            6351, // Dented Crate
            6352, // Waterlogged Crate
            6353, // Small Chest
            6356, // Battered Chest
            6357, // Sealed Crate
            5523, // Small Barnacled Clam
            5524, // Thick-shelled Clam
            7973, // Big-mouth Clam
            13874, // Heavy Crate
            20708, // Tightly Sealed Trunk
            20766, // Slimy Bag
            20767, // Scum Covered Bag
            20768, // Oozing Bag
            21113, // Watertight Trunk
            21150, // Iron Bound Trunk
            21228, // Mithril Bound Trunk
            21746, // Lucky Red Envelope (Lunar Festival item)
            24476, // Jaggal Clam
            24881, // Satchel of Helpful Goods (5-15 1st)
            24889, // Satchel of Helpful Goods (5-15 others)
            24882, // Satchel of Helpful Goods (15-25 1st)
            24890, // Satchel of Helpful Goods (15-25 others)
            27481, // Heavy Supply Crate
            27511, // Inscribed Scrollcase
            27513, // Curious Crate
            32724, // Sludge Covered Object
            35792, // Mage Hunter Personal Effects
            35945, // Brilliant Glass (Daily Cooldown for Jewelcrafting - The Burning Crusade Edition)
            36781, // Darkwater Clam
            37586, // Handful of Treats (Hallow's End Event)
            44475, // Reinforced Crate
            44663, // Abandoned Adventurer's Satchel
            44700, // Brooding Darkwater Clam
            45072, // Brightly Colored Egg (Noble Garden Event)
            45909, // Giant Darkwater Clam
            51999, // Satchel of Helpful Goods (iLevel 25)
            52000, // Satchel of Helpful Goods (31)
            52001, // Satchel of Helpful Goods (41)
            52002, // Satchel of Helpful Goods (50)
            52003, // Satchel of Helpful Goods (57)
            52004, // Satchel of Helpful Goods (62)
            52005, // Satchel of Helpful Goods (66)
            52340, // Abyssal Clam
            54516, // Loot-Filled Pumpkin (Hallow's End Event)
            57542, // Coldridge Mountaineer's Pouch 
            61387, // Hidden Stash
            62242, // Icy Prism (Daily Cooldown for Jewelcrafting - Wrath Edition)
            64657, // Canopic Jar (Archaeology Tol'vir relic)
            67248, // Satchel of Helpful Goods (39)
            67250, // Satchel of Helpful Goods (85)
            67495, // Strange Bloated Stomach (Cataclysm Skinning)
            67539, // Tiny Treasure Chest
            67597, // Sealed Crate (level 85 version)
            69903, // Satchel of Exotic Mysteries (LFD - Extra Reward)
            72201, // Plump Intestines (MoP Skinning)
            73478, // Fire Prism (Daily Cooldown for Jewelcrafting - Cataclysm Edition)
            78890, // Crystalline Geode (Dragon Soul Raid - Normal 10/25 every bossloot)
            78891, // Elementium-Coated Geode (Dragon Soul Raid - Normal 10/25 Deathwing Kill)
            78892, // Perfect Geode (Dragon Soul Raid - Heroic Hardmode 10/25 Deathwing Kill)
            78897, // Pouch o' Tokens (5 Darkmoon Faire Game Coins)
            78898, // Sack o' Tokens (20 Darkmoon Faire Game Coins)
            78899, // Pouch o' Tokens (5 Darkmoon Faire Game Coins)
            78900, // Pouch o' Tokens (5 Darkmoon Faire Game Coins)
            78901, // Pouch o' Tokens (5 Darkmoon Faire Game Coins)
            78902, // Pouch o' Tokens (5 Darkmoon Faire Game Coins)
            78903, // Pouch o' Tokens (5 Darkmoon Faire Game Coins)
            78905, // Sack o' Tokens (20 Darkmoon Faire Game Coins)
            78906, // Sack o' Tokens (20 Darkmoon Faire Game Coins)
            78907, // Sack o' Tokens (20 Darkmoon Faire Game Coins)
            78908, // Sack o' Tokens (20 Darkmoon Faire Game Coins)
            78909, // Sack o' Tokens (20 Darkmoon Faire Game Coins)
            78930, // Sealed Crate (around the Darkmoon Faire Island)
            79896, // Pandaren Tea Set (Archaeology)
            79897, // Pandaren Game Board (Archaeology)
            79898, // Twin Stein Set (Archaeology)
            79899, // Walking Cane (Archaeology)
            79900, // Empty Keg (Archaeology)
            79901, // Carved Bronze Mirror (Archaeology)
            79902, // Gold-Inlaid Figurine (Archaeology)
            79903, // Apothecary Tins (Archaeology)
            79904, // Pearl of Yu'lon (Archaeology)
            79905, // Standard of Niuzao (Archaeology)
            79908, // Manacles of Rebellion (Archaeology)
            79909, // Cracked Mogu Runestone (Archaeology)
            79910, // Terracotta Arm (Archaeology)
            79911, // Petrified Bone Whip (Archaeology)
            79912, // Thunder King Insignia (Archaeology)
            79913, // Edicts of the Thunder King (Archaeology)
            79914, // Iron Amulet (Archaeology)
            79915, // Warlord's Branding Iron (Archaeology)
            79916, // Mogu Coin (Archaeology)
            79917, // Worn Monument Ledger (Archaeology)
            85224, // Basic Seed Pack
            85225, // Basic Seed Pack
            85226, // Basic Seed Pack
            87391, // Plundered Treasure (Luck of the Lotus Buff)
			88496, // Sealed Crate (MoP version)
			89610, // Pandaria Herbs (Trade for Spirit of Harmony)
			89613, // Cache of Treasures (Scenario Reward)
			89810, // Bounty of a Sundered Land (LFR Bonus Roll Gold Reward)
			90625, // Treasures of the Vale (Daily Quest Reward)
			90716, // Good Fortune (When using a Lucky Charm on a boss for loot)
			90839, // Cache of Sha-Touched Gold (World Boss gold drop)
			90840, // Marauder's Gleaming Sack of Gold (World Boss gold drop)
			92813, // Greater Cache of Treasures (Scenario Reward)
			92960, // Silkworm Cocoon (Tailoring Imperial Silk)
			93724, // Darkmoon Game Prize
			94219, // Arcane Trove (Daily Quest Reward IoTK Alliance)
			94220, // Sunreaver Bounty (Daily Quest Reward IoTK Horde)
			94296, // Cracked Primal Egg
			94566, // Fortuitous Coffer (Loot Item IoTK)
			95343, // Treasures of the Thunder King (LFR Loot)
			95469, // Serpent's Heart (Daily Cooldown for Jewelcrafting - MoP Edition)
			95601, // Shiny Pile of Refuse (World Boss drop)
			95602, // Stormtouched Cache (World Boss drop)
			95617, // Dividends of the Everlasting Spring (LFR Loot)
			95618, // Cache of Mogu Riches (LFR Loot)
			95619, // Amber Encased Treasure Pouch (LFR Loot)
			98096, // Large Sack of Coins (Brawler Fight Reward)
			98097, // Huge Sack of Coins (Brawler Fight Reward)
			98098, // Bulging Sack of Coins (Brawler Fight Reward)
			98099, // Giant Sack of Coins (Brawler Fight Reward)
			98100, // Humongous Sack of Coins (Brawler Fight Reward)
			98101, // Enormous Sack of Coins (Brawler Fight Reward)
			98102, // Overflowing Sack of Coins (Brawler Fight Reward)
			98103, // Gigantic Sack of Coins (Brawler Fight Reward)
			98133, // Greater Cache of Treasures (Scenario Reward)
			98134, // Heroic Cache of Treasures (Heroic Scenario Reward)
			98546, // Bulging Heroic Cache of Treasures (First Heroic Scenario Reward)
			98560, // Arcane Trove (Vendor Version Alliance)
			98562, // Sunreaver Bounty (Vendor Version Horde)
			103624, // Treasures of the Vale (Zone Loot)
			104034, // Purse of Timeless Coins (Timeless Isle)
			104035, // Giant Purse of Timeless Coins (Timeless Isle)
			104271, // Coalesced Turmoil (SoO LFR Loot)
			104272, // Celestial Treasure Box (Timeless Isle Loot)
			104273, // Flame-Scarred Cache of Offerings (Timeless Isle Loot)
			104275, // Twisted Treasures of the Vale (SoO LFR Loot)
			105713, // Twisted Treasures of the Vale (SoO Flex Loot)
			105714, // Coalesced Turmoil (SoO Flex Loot)
            114634, // Icy Satchel of Helpful Goods Item Level 70
            114641, // Icy Satchel of Helpful Goods Item Level 75
            114648, // Scorched Satchel of Helpful Goods Item Level 80
            114655, // Scorched Satchel of Helpful Goods Item Level 84
            114662, // Tranquil Satchel of Helpful Goods Item Level 85
            114669, // Tranquil Satchel of Helpful Goods Item Level 88
			139776, // Banner of the Mantid Empire (Archaeology)
			139779, // Ancient Sap Feeder (Archaeology)
			139780, // The Praying Mantid (Archaeology)
			139781, // Inert Sound Beacon (Archaeology)
			139782, // Remains of a Paragon (Archaeology)
			139783, // Mantid Lamp (Archaeology)
			139784, // Pollen Collector (Archaeology)
			139785  // Kypari sap Container (Archaeology)
        };

        private HashSet<uint> _itemUseOnThree = new HashSet<uint>() {
            10938, // Lesser Magic Essence
            10998, // Lesser Astral Essence
            11134, // Lesser Mystic Essence
            11174, // Lesser Nether Essence
            16202, // Lesser Eternal Essence
            22447, // Lesser Planar Essence
            34053, // Small Dream Shard
            34056, // Lesser Cosmic Essence
            52718, // Lesser Celestial Essence
            74252, // Small Ethereal Shard
            52720  // Small Heavenly Shard
        };

        private HashSet<uint> _itemUseOnFive = new HashSet<uint>() {
//			111671, // Enormous Abyssal Gulper Eel
//			111601, // Enormous Crescent Saberfish
//			111675, // Enormous Fat Sleeper
//			111674, // Enormous Blind Lake Sturgeon
//			111673, // Enormous Fire Ammonite
//			111672, // Enormous Sea Scorpion
//			111676, // Enormous Jawless Skulker
//			111670, // Enormous Blackwater Whiptail
//			118566,  // Enormous Savage Piranha
            33567 // Borean Leather Scraps
        };

        private HashSet<uint> _itemUseOnTen = new HashSet<uint>() {
            22572, // Mote of Air
            22573, // Mote of Earth
            22574, // Mote of Fire
            22575, // Mote of Life
            22576, // Mote of Mana
            22577, // Mote of Shadow
            22578, // Mote of Water
            37700, // Crystallized Air
            37701, // Crystallized Earth
            37702, // Crystallized Fire
            37703, // Crystallized Shadow
            37704, // Crystallized Life
            37705, // Crystallized Water
            49655, // Lovely Charm (Love is in the Air item)
            86547, // Skyshard
            89112, // Mote of Harmony
            90407, // Sparkling Shard (from Prospecting ores)
            97512, // Ghost Iron Nugget
            97546, // Kyparite Fragment
            97619, // Torn Green Tea Leaf
            97620, // Rain Poppy Petal
            97621, // Silkweed Stem
            97622, // Snow Lily Petal
            97623, // Fool's Cap Spores
			97624, // Desecrated Herb Pod
			108294, // Silver Ore Nugget
			108295, // Tin Ore Nugget
			108296, // Gold Ore Nugget
			108297, // Iron Ore Nugget
 			108298, // Thorium Ore Nugget
			108299, // Truesilver Ore Nugget
 			108300, // Mithril Ore Nugget
 			108301, // Fel Iron Ore Nugget
 			108302, // Adamantite Ore Nugget
 			108304, // Khorium Ore Nugget
 			108305, // Cobalt Ore Nugget
 			108306, // Saronite Ore Nugget
 			108307, // Obsidium Ore Nugget
 			108308, // Elementium Ore Nugget
			108318, // Mageroyal Petal
			108319, // Earthroot Stem
			108320, // Briarthorn Bramble
			108321, // Swiftthistle Leaf
			108322, // Bruiseweed Stem
			108323, // Wild Steelbloom Petal
			108324, // Kingsblood Petal
			108325, // Liferoot Stem
			108326, // Khadgar's Whisker Stem
			108327, // Grave Moss Leaf
			108328, // Fadeleaf Petal
			108329, // Dragon's Teeth Stem
			108330, // Stranglekelp Blade
			108331, // Goldthorn Bramble
			108332, // Firebloom Petal
			108333, // Purple Lotus Petal
			108334, // Arthas' Tears Petal
			108335, // Sungrass Stalk
			108336, // Blindweed Stem
			108337, // Ghost Mushroom Cap
			108338, // Gromsblood Leaf
			108339, // Dreamfoil Blade
			108340, // Golden Sansam Leaf
			108341, // Mountain Silversage Stalk
			108342, // Sorrowmoss Leaf
			108343, // Icecap Petal
			108344, // Felweed Stalk
			108345, // Dreaming Glory Petal
			108346, // Ragveil Cap
			108347, // Terocone Leaf
			108348, // Ancient Lichen Petal
			108349, // Netherbloom Leaf
			108350, // Nightmare Vine Stem
			108351, // Mana Thistle Leaf
			108352, // Goldclover Leaf
			108353, // Adder's Tongue Stem
			108354, // Tiger Lily Petal
			108355, // Lichbloom Stalk
 			108356, // Icethorn Bramble
			108357, // Talandra's Rose Petal
			108358, // Deadnettle Bramble
			108359, // Fire Leaf Bramble
			108360, // Cinderbloom Petal
			108361, // Stormvine Stalk
			108362, // Azshara's Veil Stem
			108363, // Heartblossom Petal
			108364, // Twilight Jasmine Petal
			108365, // Whiptail Stem
			108391, // Titanium Ore Nugget
			109624, // Broken Frostweed Stem
			109625, // Broken Fireweed Stem
			109626, // Gorgrond Flytrap Ichor
			109627, // Starflower Petal
			109628, // Nagrand Arrowbloom Petal
			109629, // Talador Orchid Petal
 			109991, // True Iron Nugget
			109992, // Blackrock Fragment
//			111664, // Abyssal Gulper Eel
//			111595, // Crescent Saberfish
//			111668, // Fat Sleeper
//			111667, // Blind Lake Sturgeon
//			111666, // Fire Ammonite
//			111665, // Sea Scorpion
//			111669, // Jawless Skulker
//			111663, // Blackwater Whiptail
//			118565, // Savage Piranha
 			112693, // Frostweed Seed
			112694  // Fireweed Seed
        };

        private HashSet<uint> _itemUseOnTwenty = new HashSet<uint>() {
            111659, // Small Abyssal Gulper Eel
            111589, // Small Crescent Saberfish
            111651, // Small Fat Sleeper
            111652, // Small Blind Lake Sturgeon
            111656, // Small Fire Ammonite
            111658, // Small Sea Scorpion
            111650, // Small Jawless Skulker
            111662, // Small Blackwater Whiptail
            118564  // Small Savage Piranha
        };

        private HashSet<uint> _itemRequiresSleep = new HashSet<uint>() {
            61387, // Hidden Stash
            67495, // Strange Bloated Stomach (Cataclysm Skinning)
            67539, // Tiny Treasure Chest
            72201, // Plump Intestines (MoP Skinning)
            87391, // Plundered Treasure (Luck of the Lotus Buff)
			88496, // Sealed Crate (MoP version)
			89610, // Pandaria Herbs (Trade for Spirit of Harmony)
			89613, // Cache of Treasures (Scenario Reward)
			89810, // Bounty of a Sundered Land (LFR Bonus Roll Gold Reward)
			90625, // Treasures of the Vale (Daily Quest Reward)
			90716, // Good Fortune (When using a Lucky Charm on a boss for loot)
			90839, // Cache of Sha-Touched Gold (World Boss gold drop)
			90840, // Marauder's Gleaming Sack of Gold (World Boss gold drop)
			92813, // Greater Cache of Treasures (Scenario Reward)
			92960, // Silkworm Cocoon (Tailoring Imperial Silk)
			94219, // Arcane Trove (Daily Quest Reward IoTK Alliance)
			94220, // Sunreaver Bounty (Daily Quest Reward IoTK Horde)
			94296, // Cracked Primal Egg
			94566, // Fortuitous Coffer (Loot Item IoTK)
			95343, // Treasures of the Thunder King (LFR Loot)
			95601, // Shiny Pile of Refuse (World Boss drop)
			95602, // Stormtouched Cache (World Boss drop)
			95617, // Dividends of the Everlasting Spring (LFR Loot)
			95618, // Cache of Mogu Riches (LFR Loot)
			95619, // Amber Encased Treasure Pouch (LFR Loot)
			98096, // Large Sack of Coins (Brawler Fight Reward)
			98097, // Huge Sack of Coins (Brawler Fight Reward)
			98098, // Bulging Sack of Coins (Brawler Fight Reward)
			98099, // Giant Sack of Coins (Brawler Fight Reward)
			98100, // Humongous Sack of Coins (Brawler Fight Reward)
			98101, // Enormous Sack of Coins (Brawler Fight Reward)
			98102, // Overflowing Sack of Coins (Brawler Fight Reward)
			98103, // Gigantic Sack of Coins (Brawler Fight Reward)
			98133, // Greater Cache of Treasures (Scenario Reward)
			98134, // Heroic Cache of Treasures (Heroic Scenario Reward)
			98546, // Bulging Heroic Cache of Treasures (First Heroic Scenario Reward)
			98560, // Arcane Trove (Vendor Version Alliance)
			98562  // Sunreaver Bounty (Vendor Version Horde)
        };

        private HashSet<uint> _destroyItems = new HashSet<uint>() {
            19221, // Darkmoon Special Reserve
			19222, // Cheap Beer
			19223, // Darkmoon Dog
			19224, // Red Hot Wings
			19225, // Deep Fried Candybar
			19299, // Fizzy Faire Drink
			19300, // Bottled Winterspring Water
			19304, // Spiced Beef Jerky
			19305, // Pickled Kodo Foot
			19306, // Crunchy Frog
			21151, // Rumsey Rum Black Label
			44940, // Corn-Breaded Sausage
			44941, // Fresh-Squeezed Limeade
			45188, // Whitered Kelp
			45189, // Torn Sail
			45190, // Driftwood
			45191, // Empty Clam
			45194, // Tangled Fishing Line
			45195, // Empty Rum Bottle
			45196, // Tattered Cloth
			45197, // Tree Branch
			45198, // Weeds
			45199, // Old Boot
			45200, // Sickly Fish
			45201, // Rock
			45202, // Water Snail
			73260, // Salty Sea Dog
			74822  // Sasparilla Sinker
		};

        public override void Pulse()
        {
            if (!_init)
            {
                base.OnEnable();
                Lua.DoString("SetCVar('AutoLootDefault','1')");
                Lua.Events.AttachEvent("LOOT_CLOSED", LootFinished);
                Lua.Events.AttachEvent("MAIL_CLOSED", MailboxFinished);
                Logging.Write(LogLevel.Normal, Colors.DarkRed, "TidyBags 3.6 ready for use...");
                _init = true;
            }

            if (_init)
                if (StyxWoW.Me.IsActuallyInCombat
                    || StyxWoW.Me.Mounted
                    || StyxWoW.Me.IsDead
                    || StyxWoW.Me.IsGhost
                    )
                {
                    return;
                }

            if (InventoryCheck)
            { // Loot Event has Finished
                foreach (WoWItem item in ObjectManager.GetObjectsOfType<WoWItem>())
                { // iterate over every item
                    if (item != null && item.BagSlot != -1 && StyxWoW.Me.FreeNormalBagSlots >= 2)
                    { // check if item exists and is in bag and we have space
                        if (_itemUseOnOne.Contains(item.Entry) || _artifactPowerItems.Contains(item.Entry))
                        { // stacks of 1
                            if (item.StackCount >= 1)
                            {
                                this.useItem(item);
                            }
                        }
                        else if (_itemUseOnThree.Contains(item.Entry))
                        { // stacks of 3
                            if (item.StackCount >= 3)
                            {
                                this.useItem(item);
                            }
                        }
                        else if (_itemUseOnFive.Contains(item.Entry))
                        { // stacks of 5
                            if (item.StackCount >= 5)
                            {
                                this.useItem(item);
                            }
                        }
                        else if (_itemUseOnTen.Contains(item.Entry))
                        { // stacks of 10
                            if (item.StackCount >= 10)
                            {
                                this.useItem(item);
                            }
                            //                        } else if (_itemUseOnTwenty.Contains(item.Entry)) { // stacks of 20
                            //                            if (item.StackCount >= 20) {
                            //                                this.useItem(item);
                            //                            }
                        }
                        else if (_destroyItems.Contains(item.Entry))
                        {
                            this.destroyItem(item);
                        }
                    }
                }
                InventoryCheck = false;
                StyxWoW.SleepForLagDuration();
            }
        }

        private void useItem(WoWItem item)
        {
            Logging.Write(LogLevel.Normal, Colors.DarkRed, "[{0} {1}]: Using {2} we have {3}", this.Name, this.Version, item.Name, item.StackCount);

            if (_itemRequiresSleep.Contains(item.Entry))
            {
                // some (soulbound) items require an additional sleep to prevent a loot bug
                StyxWoW.SleepForLagDuration();
            }
            Lua.DoString("UseItemByName(\"" + item.Name + "\")");
            StyxWoW.SleepForLagDuration();
        }

        private void destroyItem(WoWItem item)
        {
            Logging.Write(LogLevel.Normal, Colors.DarkRed, "[{0} {1}]: Destroying {2} we have {3}", this.Name, this.Version, item.Name, item.StackCount);
            item.PickUp();
            Lua.DoString("DeleteCursorItem()");
            StyxWoW.SleepForLagDuration();
        }
    }
}