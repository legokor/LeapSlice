#define CAVERN

#if CAVERN
using Cavern;
#endif

using UnityEngine;

/// <summary>
/// Egyetlen hang lejátszása.
/// </summary>
public static class SingleShotAudio {
    /// <summary>
    /// Cavern immerzív hangmotor használata.
    /// </summary>
    public static bool UseCavern = true;

    /// <summary>
    /// Hangtároló objektum létrehozása.
    /// </summary>
    /// <param name="Clip">Hang</param>
    /// <param name="At">Pozíció</param>
    /// <returns>Egy objektum, ami a hang hosszáig él</returns>
    static GameObject CreateContainer(AudioClip Clip, Vector3 At) {
        GameObject Obj = new GameObject("Single Shot Audio");
        Obj.transform.position = At;
        Obj.AddComponent<DespawnTime>().Timer = Clip.length + 1;
        return Obj;
    }

    /// <summary>
    /// Hang lejátszása egy objektumon.
    /// </summary>
    /// <param name="Clip">Hang</param>
    /// <param name="Obj">Objektum</param>
    public static void Attach(AudioClip Clip, GameObject Obj) {
#if CAVERN
        AudioSource3D Source = Obj.AddComponent<AudioSource3D>();
        Source.Clip = Clip;
        Source.VolumeRolloff = Rolloffs.Disabled;
#else
        Obj.AddComponent<AudioSource>().PlayOneShot(Clip);
#endif
    }

    /// <summary>
    /// Hang lejátszása a tér egy pontján.
    /// </summary>
    /// <param name="Clip">Hang</param>
    /// <param name="At">Pozíció</param>
    public static void Play(AudioClip Clip, Vector3 At) {
#if CAVERN
        AudioSource3D Source = CreateContainer(Clip, At).AddComponent<AudioSource3D>();
        Source.Clip = Clip;
        Source.VolumeRolloff = Rolloffs.Disabled;
#else
        CreateContainer(Clip, At).AddComponent<AudioSource>().PlayOneShot(Clip);
#endif
    }
}