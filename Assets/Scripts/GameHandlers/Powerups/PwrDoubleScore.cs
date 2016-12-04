using UnityEngine;

namespace LeapSlice {
    /// <summary>
    /// Pontduplázó power up.
    /// </summary>
    public class PwrDoubleScore : MonoBehaviour {
        [Tooltip("A power up működési ideje.")]
        [Range(3, 15)]
        public float Duration = 4.5f;

        /// <summary>
        /// A nem megduplázandó, power up előtt szerzett pontok
        /// </summary>
        int ScoreBefore;
        /// <summary>
        /// A duplázva megadott pontok
        /// </summary>
        int ScoreGiven = 0;

        /// <summary>
        /// Duplikálásvédelem: két pontduplázó egymást pörgetné.
        /// </summary>
        static PwrDoubleScore Instance = null;

        /// <summary>
        /// Indításkor adjon neki élettartamot, és törölje a hasonló power upokat, különben egymást pörgetik.
        /// </summary>
        void Start() {
            if (Instance != null)
                Destroy(Instance.gameObject);
            Instance = this;
            ScoreBefore = Game.Score;
            gameObject.AddComponent<DespawnTime>().Timer = Duration;
        }

        /// <summary>
        /// Képernyőeffekt a power uphoz.
        /// </summary>
        void OnGUI() {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Game.Instance.DoubleScoreBackground);
        }

        /// <summary>
        /// Az összes update után a képkocka alatt szerzett pontot megduplázza.
        /// </summary>
        void LateUpdate() {
            int ScoreEarned = Game.Score - ScoreBefore - ScoreGiven;
            ScoreGiven += ScoreEarned << 1;
            Game.Score = ScoreBefore + ScoreGiven;
        }

        /// <summary>
        /// Egy pontduplázó aktiválása.
        /// </summary>
        public static void Activate() {
            (new GameObject()).AddComponent<PwrDoubleScore>();
        }
    }
}