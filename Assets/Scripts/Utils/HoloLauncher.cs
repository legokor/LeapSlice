using UnityEngine;

#if UNITY_WSA_10_0
using UnityEngine.VR.WSA.Input;
#endif

/// <summary>
/// HoloLens módban indítja a játékot.
/// Ami még kell, hogy működjön: a HoloToolkit-Unity-ből egy kurzor és egy spatial mapping.
/// </summary>
public class HoloLauncher : Singleton<HoloLauncher> {
    [Tooltip("A játéktér elhelyezését segítő kurzor.")]
    public GameObject Cursor;
    [Tooltip("Játékmező, amit a játékos elhelyez, amikor felületre kattint.")]
    public GameObject GameField;

    /// <summary>
    /// Ha nem falra kattintott a játékos, ennyi ideig tudassa vele a képernyőn.
    /// </summary>
    float FailTimer = 0;

#if UNITY_WSA_10_0
    /// <summary>
    /// Rendelje hozzá a koppintáshoz a játéktér elhelyezését.
    /// </summary>
    void Start() {
        InteractionManager.SourcePressed += AirTap;
    }
#endif

    /// <summary>
    /// Instrukciók/hibaüzenet kijelzése.
    /// </summary>
    void OnGUI() {
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.skin.label.fontSize = Screen.height / 10;
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), FailTimer > 0 ? "No wall detected.\nPlease move closer and try again." : "Tap on a wall to place the game.");
    }

    /// <summary>
    /// Falra kattintás kezelése.
    /// </summary>
#if UNITY_WSA_10_0
    public void AirTap(InteractionSourceState state) {
#else
    public void AirTap(object nullobj) {
#endif
        RaycastHit Hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(.5f, .5f)), out Hit)) {
            GameField.transform.position = Hit.point + Hit.normal * .1f;
            GameField.transform.LookAt(Hit.point);
            GameField.SetActive(true);
#if UNITY_WSA_10_0
            InteractionManager.SourcePressed -= AirTap;
#endif
            Destroy(this);
            if (Cursor)
                Destroy(Cursor);
        } else
            FailTimer = 5;
    }

    public void Update() {
        FailTimer -= Time.deltaTime * .5f;
    }
}
