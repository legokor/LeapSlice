using UnityEngine;

/// <summary>
/// Objektum megsemmisítése idővel.
/// </summary>
public class DespawnTime : MonoBehaviour {
    public float Timer = 3;

    /// <summary>
    /// Visszaszámlálás, és a végén megsemmisülés.
    /// </summary>
    void Update() {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
            Destroy(gameObject);
    }
}