using UnityEngine;

namespace LeapSlice {
    /// <summary>
    /// Időlassító power up.
    /// </summary>
    public class PwrSlowMotion : MonoBehaviour {
        [Tooltip("A power up működési ideje.")]
        [Range(1, 15)]
        public float Duration = 5f;
        [Tooltip("Cél időtelési sebesség.")]
        [Range(.25f, .74f)]
        public float TimeScale = .5f;

        /// <summary>
        /// Lassítás előtti időskála.
        /// </summary>
        float OldScale;

        /// <summary>
        /// Induláskor az idő belassítása, a működési idő korrigálása az új időskálára, és hang lejátszása.
        /// </summary>
        void Start() {
            Duration *= TimeScale;
            OldScale = Time.timeScale;
            SingleShotAudio.Play(Game.Instance.SlowMotionInSound, transform.position);
        }

        /// <summary>
        /// Képernyőeffekt a power uphoz.
        /// </summary>
        void OnGUI() {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Game.Instance.SlowMotionBackground);
        }

        /// <summary>
        /// Az összes update után felülírja az idő telésének sebességét a sajátjára. Ha letelt az ideje, állítsa vissza régi időskálázást, és pusztuljon el.
        /// </summary>
        void LateUpdate() {
            Time.timeScale = TimeScale;
            Duration -= Time.deltaTime;
            if (Duration <= 0)
                DestroyAndResetTime(Mathf.Max(OldScale, Game.Instance.ArcadeSpeed)); // Ha egy másik lassító is aktív, ne annak a sebességét hagyja maga után.
            if (!Game.Playing) // Ha közben véget ért a játék, pusztuljon el, miután alaphelyzetbe állította az időskálát.
                DestroyAndResetTime(1);
        }

        /// <summary>
        /// A lassítás beszűntetése.
        /// </summary>
        /// <param name="Scale">Régi időskála</param>
        void DestroyAndResetTime(float Scale) {
            Time.timeScale = Scale;
            SingleShotAudio.Play(Game.Instance.SlowMotionOutSound, transform.position);
            Destroy(gameObject);
        }

        /// <summary>
        /// Egy lassítás aktiválása.
        /// </summary>
        public static void Activate(Vector3 At) {
            (new GameObject()).AddComponent<PwrSlowMotion>().gameObject.transform.position = At;
        }
    }
}