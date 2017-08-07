using Cavern;
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
    /// Hang lejátszása egy objektumon, Cavern motorral.
    /// </summary>
    /// <param name="Clip">Hang</param>
    /// <param name="Obj">Objektum</param>
    static void AttachWithCavern(AudioClip Clip, GameObject Obj) {
        AudioSource3D Source = Obj.AddComponent<AudioSource3D>();
        Source.Clip = Clip;
        Source.VolumeRolloff = Rolloffs.Disabled;
    }

    /// <summary>
    /// Hang lejátszása egy objektumon, Unity motorral.
    /// </summary>
    /// <param name="Clip">Hang</param>
    /// <param name="Obj">Objektum</param>
    static void AttachWithUnity(AudioClip Clip, GameObject Obj) {
        Obj.AddComponent<AudioSource>().PlayOneShot(Clip);
    }

    /// <summary>
    /// Hang lejátszása egy objektumon.
    /// </summary>
    /// <param name="Clip">Hang</param>
    /// <param name="Obj">Objektum</param>
    public static void Attach(AudioClip Clip, GameObject Obj) {
        if (UseCavern)
            AttachWithCavern(Clip, Obj);
        else
            AttachWithUnity(Clip, Obj);
    }

    /// <summary>
    /// Hang lejátszása Cavern használatával.
    /// </summary>
    /// <param name="Clip">Hang</param>
    /// <param name="At">Pozíicó</param>
    static void PlayWithCavern(AudioClip Clip, Vector3 At) {
        AudioSource3D Source = CreateContainer(Clip, At).AddComponent<AudioSource3D>();
        Source.Clip = Clip;
        Source.VolumeRolloff = Rolloffs.Disabled;
    }

    /// <summary>
    /// Hang lejátszása a Unity hangmotorjával.
    /// </summary>
    /// <param name="Clip">Hang</param>
    /// <param name="At">Pozíció</param>
    static void PlayWithUnity(AudioClip Clip, Vector3 At) {
        CreateContainer(Clip, At).AddComponent<AudioSource>().PlayOneShot(Clip);
    }

    /// <summary>
    /// Hang lejátszása a tér egy pontján.
    /// </summary>
    /// <param name="Clip">Hang</param>
    /// <param name="At">Pozíció</param>
    public static void Play(AudioClip Clip, Vector3 At) {
        if (UseCavern)
            PlayWithCavern(Clip, At);
        else
            PlayWithUnity(Clip, At);
    }
}