using UnityEngine;

namespace LeapSlice {
    /// <summary>
    /// Kombójelző.
    /// </summary>
    public class ComboMarker : MonoBehaviour {
        [Tooltip("Jelzett kombó értéke.")]
        public int Combo;

        [Tooltip("Jelzésből hátralévő idő.")]
        public float Duration = 1.5f;

        /// <summary>
        /// Létrejöttekor kombóhang lejátszása.
        /// </summary>
        void Awake() {
            SingleShotAudio.Attach(Game.Instance.ComboSound, gameObject);
        }

        /// <summary>
        /// Kombó kijelzése. A pozíciót a komponens világbeli pozíciója határozza meg.
        /// </summary>
        void OnGUI() {
            float Size = Screen.height / 10f * Mathf.Log10(10 + Combo);
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.skin.label.fontSize = (int)Size;
            Vector3 ScreenPos = Camera.main.WorldToViewportPoint(transform.position);
            Utils.ShadedLabel(new Rect(Screen.width * ScreenPos.x - Size * 3, Screen.height * (1f - ScreenPos.y) - Size, Size * 6, 2 * Size), Combo.ToString(),
                Color.Lerp(Color.yellow, Color.red, (Combo - 3) / 7f));
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