using Leap;
using UnityEngine;

/// <summary>
/// LeapMotion-kezelés még egyszerűbben.
/// </summary>
public class LeapMotion : Singleton<LeapMotion> {
    [Tooltip("A kép alja, ha 2D-ben van használva az eszköz")]
    public Vector2 LeapLowerBounds = new Vector2(-100, 0);
    [Tooltip("A kép teteje, ha 2D-ben van használva az eszköz")]
    public Vector2 LeapUpperBounds = new Vector2(100, 0);

    Controller Device;

    /// <summary>
    /// A komponens létrehozásakor csatlakozás a LeapMotion-höz teljesen automatikusan.
    /// </summary>
    void Awake() {
        Device = new Controller();
    }

    /// <summary>
    /// A komponens megyszűnésekor csatlakozzon le az eszközről.
    /// </summary>
    void OnDestroy() {
        if (Device.IsConnected)
            Device.ClearPolicy(Controller.PolicyFlag.POLICY_OPTIMIZE_HMD);
        Device.StopConnection();
    }

    /// <summary>
    /// Egy kéz LeapMotion által érzékelt pozíciójából számol képernyőpozíciót.
    /// </summary>
    /// <param name="FromLeap">LeapMotion által érzékelt pozíció</param>
    /// <returns>Képernyőpozíció</returns>
    Vector2 ScreenFromLeap(Vector2 FromLeap) {
        float LeftBounds = !SBS.Enabled ? LeapLowerBounds.x : (LeapLowerBounds.x - (LeapUpperBounds.x - LeapLowerBounds.x));
        return new Vector2(
            (Mathf.Clamp(FromLeap.x, LeftBounds, LeapUpperBounds.x) - LeftBounds) / (LeapUpperBounds.x - LeftBounds) * Screen.width,
            (Mathf.Clamp(FromLeap.y, LeapLowerBounds.y, LeapUpperBounds.y) - LeapLowerBounds.y) / (LeapUpperBounds.y - LeapLowerBounds.y) * Screen.height);
    }

    /// <summary>
    /// Kezek számának lekérdezése.
    /// </summary>
    /// <returns>Ennyi kezet lát a kontroller</returns>
    public int GetHandCount() {
        return Device.IsConnected ? Device.Frame().Hands.Count : 0;
    }

    /// <summary>
    /// Tenyér helyzetének lekérdezése az adott kézhez.
    /// </summary>
    /// <param name="HandID">Kéz azonosítója</param>
    /// <returns>Tenyér pozíciója</returns>
    public Vector2 PalmOnScreen(int HandID = 0) {
        if (Device.IsConnected && Device.Frame().Hands.Count > HandID) {
            Hand CheckedHand = Device.Frame().Hands[HandID];
            return ScreenFromLeap(new Vector2(CheckedHand.PalmPosition.x, CheckedHand.PalmPosition.y));
        } else {
            return new Vector2(-1, -1);
        }
    }

    /// <summary>
    /// Ujjal mutogatás: az adott kéz legtávolabbi ujjának pozíciója a képernyőn.
    /// </summary>
    /// <param name="HandID">Kéz azonosítója</param>
    /// <returns>Mutatott képernyőpozíció</returns>
    public Vector2 SinglePointOnScreen(int HandID = 0) {
        if (Device.IsConnected && Device.Frame().Hands.Count > HandID) {
            Hand CurrentHand = Device.Frame().Hands[HandID];
            Finger Furthest = CurrentHand.Fingers[0];
            foreach (Finger CheckedFinger in CurrentHand.Fingers)
                if (Furthest.StabilizedTipPosition.z > CheckedFinger.StabilizedTipPosition.z)
                    Furthest = CheckedFinger;
            return ScreenFromLeap(new Vector2(Furthest.StabilizedTipPosition.x, Furthest.StabilizedTipPosition.y));
        } else {
            return new Vector2(-1, -1);
        }
    }

    /// <summary>
    /// Kinyújtott ujjak száma az adott kézen.
    /// </summary>
    /// <param name="HandID">Kéz atonosítója</param>
    /// <returns>Kinyújtott ujjak száma</returns>
    public int ExtendedFingers(int HandID = 0) {
        int Counter = 0;
        if (Device.Frame().Hands.Count > HandID) {
            Hand CurrentHand = Device.Frame().Hands[HandID];
            foreach (Finger CheckedFinger in CurrentHand.Fingers)
                if (CheckedFinger.IsExtended)
                    Counter++;
        }
        return Counter;
    }
}