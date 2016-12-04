using UnityEngine;

namespace LeapSlice {
    /// <summary>
    /// Képernyőről kiesett objektumok eltüntetése.
    /// </summary>
    public class Despawner : MonoBehaviour {
        /// <summary>
        /// Kiesés figyelése és kezelése.
        /// </summary>
        void Update() {
            if (transform.position.y < Game.BottomLeft.y - 2) { // Ha a játékmezőből kiesett
                Target ThisTarget = gameObject.GetComponent<Target>();
                if (ThisTarget)
                    ThisTarget.Fell(); // Hívja meg a nem szétesett tárgyakra a leesés kezelését
                Destroy(gameObject); // Tüntessen el mindent
            }
        }
    }
}