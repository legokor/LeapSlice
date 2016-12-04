using UnityEngine;

namespace LeapSlice {
    /// <summary>
    /// Esésjelző
    /// </summary>
    public class FallMarker : MonoBehaviour {
        [Tooltip("Jelzésből hátralévő idő.")]
        public float Duration = 1.5f;

        /// <summary>
        /// Létrejöttekor eséshang lejátszása.
        /// </summary>
        void Awake() {
            SingleShotAudio.Attach(Game.Instance.FallSound, gameObject);
        }

        /// <summary>
        /// Esés kijelzése. A pozíciót a komponens világbeli pozíciója határozza meg.
        /// </summary>
        void OnGUI() {
            int XSize = Screen.height / 10;
            GUI.skin.label.fontSize = XSize;
            GUI.color = Color.red;
            Vector3 ScreenPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, Game.BottomLeft.y, transform.position.z));
            GUI.Label(new Rect(ScreenPos.x - XSize / 2, Screen.height - ScreenPos.y - XSize, XSize, XSize), "X");
            GUI.color = Color.white;
        }

        /// <summary>
        /// Élettartam kezelése.
        /// </summary>
        void Update() {
            Duration -= Time.deltaTime;
            if (Duration <= 0)
                Destroy(gameObject);
        }
    }
}