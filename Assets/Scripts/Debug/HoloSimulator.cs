using UnityEngine;

namespace LeapSlice.Debug {
    /// <summary>
    /// HoloLens szimulálása, egérrel nézelődéssel, és QWEASD-mozgással.
    /// </summary>
    public class HoloSimulator : Singleton<HoloSimulator> {
#if UNITY_EDITOR // Játékba ne kerüljön, feleslegesen számol
        [Tooltip("Érzékenység.")]
        [Range(.01f, 1)]
        public float Sensitivity = .15f;

        /// <summary>
        /// Az egér előző képkockabeli pozíciója.
        /// </summary>
        Vector3 PrevMouse;

        /// <summary>
        /// Induláskor az egér helyzetének eltárolása, hogy ne legyen indulási ugrás.
        /// </summary>
        void Start() {
            PrevMouse = Input.mousePosition;
        }

        /// <summary>
        /// Mozgás alkalmazása minden képkockafrissítéskor.
        /// </summary>
        void Update() {
            transform.position += transform.rotation * new Vector3((Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0), // D: jobbra, A: balra
                                                                   (Input.GetKey(KeyCode.Q) ? 1 : 0) - (Input.GetKey(KeyCode.E) ? 1 : 0), // Q: fel, E: le
                                                                   (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0)) // W: előre, D: hátra
                                                                   * Time.deltaTime // Időzítés
                                                                   * (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift) ? 5 : 20); // Sebesség
            // Kamera forgatása
            Vector3 MouseDelta = Input.mousePosition - PrevMouse;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x - MouseDelta.y * Sensitivity, transform.rotation.eulerAngles.y + MouseDelta.x * Sensitivity, 0);
            PrevMouse = Input.mousePosition;
            if (Input.GetMouseButtonDown(0) && HoloLauncher.Instance)
                HoloLauncher.Instance.OnClick();
        }
#endif
    }
}