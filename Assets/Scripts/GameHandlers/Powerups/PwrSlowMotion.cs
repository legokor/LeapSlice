using UnityEngine;

namespace LeapSlice {
    /// <summary>
    /// Időlassító power up.
    /// </summary>
    public class PwrSlowMotion : PowerUp {
        [Tooltip("Cél időtelési sebesség.")]
        [Range(.25f, .74f)]
        public float TimeScale = .5f;

        /// <summary>
        /// Lassítás előtti időskála.
        /// </summary>
        float OldScale;

        /// <summary>
        /// Konstruktor, időtartam beállítása.
        /// </summary>
        public PwrSlowMotion() {
            Duration = 2.5f;
        }

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
            if (!Game.Playing) // Ha közben véget ért a játék, pusztuljon el, miután alaphelyzetbe állította az időskálát
                Destroy(gameObject);
        }

        /// <summary>
        /// Megszűnéskor a lassítás beszűntetése.
        /// </summary>
        void OnDestroy() {
            Time.timeScale = Game.Playing ? Mathf.Max(OldScale, Game.Instance.ArcadeSpeed) : 1;
            SingleShotAudio.Play(Game.Instance.SlowMotionOutSound, transform.position);
        }
    }
}