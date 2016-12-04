using UnityEngine;

namespace LeapSlice {
    /// <summary>
    /// Gyors tárgybedobáló power up.
    /// </summary>
    public class PwrItemstorm : MonoBehaviour {
        [Tooltip("A power up működési ideje.")]
        [Range(.5f, 5)]
        public float Duration = 1.5f;
        [Tooltip("Másodpercenként bedobált tárgyak száma.")]
        [Range(5, 25)]
        public int ObjectsPerSec = 10;

        /// <summary>
        /// Következő tárgybedobásig hátralévő idő.
        /// </summary>
        float Delay = 0;

        /// <summary>
        /// Indításkor adjon neki élettartamot.
        /// </summary>
        void Start() {
            gameObject.AddComponent<DespawnTime>().Timer = Duration;
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

        /// <summary>
        /// Egy tárgyvihar aktiválása.
        /// </summary>
        public static void Activate() {
            (new GameObject()).AddComponent<PwrItemstorm>();
        }
    }
}