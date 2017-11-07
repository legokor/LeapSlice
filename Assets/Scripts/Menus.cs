using UnityEngine;

namespace LeapSlice {
    public static class Menus {
        /// <summary>
        /// Menüelem.
        /// </summary>
        public struct Item {
            /// <summary>
            /// A menüelemet reprezentáló objektum, aminek átvágásával az adott elemet ki lehet választani.
            /// </summary>
            public GameObject Obj;
            /// <summary>
            /// Aránylagos hely a háttérkép négy sarka közt.
            /// </summary>
            public Vector2 ScreenPos;
            /// <summary>
            /// Hely a képernyőn.
            /// </summary>
            public Vector2 ActualScrPos;
            /// <summary>
            /// Címke.
            /// </summary>
            public string Action;
        }

        /// <summary>
        /// Menü eltüntetése.
        /// </summary>
        public static void DespawnMenu(ref Item[] MenuItems) {
            foreach (Item Item in MenuItems)
                Object.Destroy(Item.Obj);
            MenuItems = new Item[0];
        }

        /// <summary>
        /// Menüelemek helyének frissítése az aránylagos helyük alapján.
        /// </summary>
        public static void PlaceMenu(Item[] MenuItems) {
            for (int i = 0; i < MenuItems.Length; i++) {
                MenuItems[i].Obj.transform.position = new Vector3(
                    Mathf.Lerp(Game.CloserBL.x, Game.CloserBR.x, MenuItems[i].ScreenPos.x),
                    Mathf.Lerp(Game.CloserBL.y, Game.CloserTL.y, MenuItems[i].ScreenPos.y),
                    Mathf.Lerp(Game.CloserBL.z, Game.CloserBR.z, MenuItems[i].ScreenPos.x));
                MenuItems[i].ActualScrPos = Camera.main.WorldToScreenPoint(MenuItems[i].Obj.transform.position);
            }
        }

        /// <summary>
        /// Főmenü kitöltése.
        /// </summary>
        public static void SetupMainMenu(Item[] MenuItems) {
            MenuItems[0].Action = "New Game";
            MenuItems[0].ScreenPos = new Vector2(.25f, .5f);
            MenuItems[1].Action = "Customize";
            if (!Game.Instance.KioskMode) {
                MenuItems[1].ScreenPos = new Vector2(.5f, .5f);
                MenuItems[2].Action = "Quit";
                MenuItems[2].ScreenPos = new Vector2(.75f, .5f);
            } else
                MenuItems[1].ScreenPos = new Vector2(.75f, .5f);
        }

        /// <summary>
        /// Személyreszabási képernyő kitöltése.
        /// </summary>
        public static void SetupCustomizationMenu(Item[] MenuItems, int BestScore, int BestCombo) {
            int BladeCount = Game.Instance.Blades.Length, WallpaperCount = Game.Instance.Wallpapers.Length, Back = BladeCount + WallpaperCount;
            MenuItems[0].Action = "Rust";
            MenuItems[0].ScreenPos = new Vector2(.1f, .66f);
            MenuItems[1].Action = BestScore >= 100 ? "Poo" : "Reach 100";
            MenuItems[1].ScreenPos = new Vector2(.1f, .33f);
            MenuItems[2].Action = BestScore >= 150 ? "Flame" : "Reach 150";
            MenuItems[2].ScreenPos = new Vector2(.3f, .66f);
            MenuItems[3].Action = BestScore >= 200 ? "Gold" : "Reach 200";
            MenuItems[3].ScreenPos = new Vector2(.3f, .33f);
            float WallPart = 1f / (WallpaperCount + 1);
            for (int i = BladeCount; i < Back; i++)
                MenuItems[i].ScreenPos = new Vector2(.6f, (Back - i) * WallPart);
            MenuItems[BladeCount++].Action = "Bus Seat";
            MenuItems[BladeCount++].Action = BestCombo >= 5 ? "Cliff" : "Combo 5";
            MenuItems[BladeCount++].Action = BestCombo >= 7 ? "Stoned" : "Combo 7";
            MenuItems[Back].Action = "Back";
            MenuItems[Back].ScreenPos = new Vector2(.9f, .5f);
        }

        /// <summary>
        /// Módválasztó képernyő kitöltése.
        /// </summary>
        public static void SetupModeSelect(Item[] MenuItems) {
            MenuItems[0].Action = "Back";
            MenuItems[0].ScreenPos = new Vector2(.75f, .25f);
            MenuItems[(int)GameModes.ScoreAttack].Action = "Score Attack";
            MenuItems[(int)GameModes.ScoreAttack].ScreenPos = new Vector2(.25f, .75f);
            MenuItems[(int)GameModes.TimeAttack].Action = "Time Attack";
            MenuItems[(int)GameModes.TimeAttack].ScreenPos = new Vector2(.5f, .75f);
            MenuItems[(int)GameModes.Arcade].Action = "Arcade";
            MenuItems[(int)GameModes.Arcade].ScreenPos = new Vector2(.75f, .75f);
            if (Game.Instance.HoloLensMode) return;
            MenuItems[(int)GameModes.Multiplayer].Action = "Multiplayer";
            MenuItems[(int)GameModes.Multiplayer].ScreenPos = new Vector2(.25f, .25f);
        }

        /// <summary>
        /// Játék vége képernyő kitöltése.
        /// </summary>
        public static void SetupGameOver(Item[] MenuItems) {
            MenuItems[0].Action = "Retry";
            MenuItems[0].ScreenPos = new Vector2(.25f, .25f);
            MenuItems[1].Action = "Main Menu";
            MenuItems[1].ScreenPos = new Vector2(.75f, .25f);
        }
    }
}