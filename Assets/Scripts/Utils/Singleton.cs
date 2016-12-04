using UnityEngine;

/// <summary>
/// Amennyiben csak egy van egy adott komponensből, könnyebbé teszi a kezelését azzal, hogy egy statikus változón keresztül elérhető a példány.
/// A Singleton lét nem garantálja a komponens egyediségét!
/// </summary>
/// <typeparam name="T">Komponens</typeparam>
public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
    /// <summary>
    /// A komponens legrégebbi példánya.
    /// </summary>
    static T _Instance;

    /// <summary>
    /// A komponens legrégebbi példányának megkeresése és lekérdezése.
    /// </summary>
    public static T Instance {
        get {
            if (_Instance == null)
                _Instance = FindObjectOfType<T>();
            return _Instance;
        }
    }
}