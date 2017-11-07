using UnityEngine;

namespace LeapSlice {
    /// <summary>
    /// Választható power-upok.
    /// </summary>
    public enum PowerUps { None = -1, DoubleScore, Itemstorm, SlowMotion }

    public abstract class PowerUp : MonoBehaviour {
        /// <summary>
        /// Power-up időtartama.
        /// </summary>
        protected float Duration;

        /// <summary>
        /// Tűnjön el, ha letelt.
        /// </summary>
        void Awake() {
            gameObject.AddComponent<DespawnTime>().Timer = Duration;
        }

        /// <summary>
        /// Adott típusú power-up aktiválása.
        /// </summary>
        public static void Activate<T>() where T : PowerUp {
            (new GameObject()).AddComponent<T>().gameObject.transform.position = Camera.main.transform.position;
        }
    }
}