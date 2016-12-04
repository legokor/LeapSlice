using UnityEngine;

/// <summary>
/// Segédfüggvények, amiknek itt a legjobb helye.
/// </summary>
public static class Utils {
    /// <summary>
    /// Négyzetre emelés.
    /// </summary>
    /// <param name="x">Szám</param>
    /// <returns>A szám négyzete</returns>
    public static float Square(float x) {
        return x * x;
    }

    /// <summary>
    /// Képernyőpozícióból világi pozíciót számol.
    /// </summary>
    /// <param name="Pos">Képernyőpozíció</param>
    /// <returns>Adott pixelen megjelenített pont a világban</returns>
    public static Vector3 ScreenPosInWorld(Vector2 Pos) {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Pos);
        if (Physics.Raycast(ray, out hit, 200, 1 << LayerMask.NameToLayer("UI"))) // Csak a UI rétegen (= a láthatatlan falon) jelenjen meg
            return hit.point;
        return Vector3.zero;
    }

    /// <summary>
    /// Árnyékoltan megjelenített szöveg. Csak OnGUI-ból hívható.
    /// </summary>
    /// <param name="Place">Helyzet a képernyőn</param>
    /// <param name="Text">Szöveg</param>
    /// <param name="Col">Szöveg színe</param>
    public static void ShadedLabel(Rect Place, string Text, Color Col) {
        GUI.color = Color.black;
        GUI.Label(new Rect(Place.x + 2, Place.y + 2, Place.width, Place.height), Text); // Először feketén, eltolva az árnyék
        GUI.color = Col;
        GUI.Label(Place, Text); // Fölérajzolva a valódi helyével és színével
    }
}
