using UnityEngine;

namespace LeapSlice {
    /// <summary>
    /// Gyors tárgybedobáló power up.
    /// </summary>
    public class PwrItemstorm : PowerUp {
        [Tooltip("Másodpercenként bedobált tárgyak száma.")]
        [Range(5, 25)]
        public int ObjectsPerSec = 10;

        /// <summary>
        /// Következő tárgybedobásig hátralévő idő.
        /// </summary>
        float Delay = 0;

        /// <summary>
        /// Konstruktor, időtartam beállítása.
        /// </summary>
        public PwrItemstorm() {
            Duration = 1.5f;
        }

        /// <summary>
        /// Képernyőeffekt a power uphoz.
        /// </summary>
        void OnGUI() {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Game.Instance.ItemstormBackground);
        }

        /// <summary>
        /// Küldje be a kért mennyiségű tárgyat.
        /// </summary>
        void LateUpdate() {
            Delay -= Time.deltaTime;
            if (Delay <= 0) {
                Dispenser.ForceDispense();
                Delay = 1 / (float)ObjectsPerSec;
            }
        }
    }
}