using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LeapSlice {
    /// <summary>
    /// Tárgyakat dobál a pályára.
    /// </summary>
    public static class Dispenser {
        /// <summary>
        /// A következő dobásig hátralévő idő.
        /// </summary>
        static float CurrentDrop;
        /// <summary>
        /// Intervallum két bedobott tárgy közt.
        /// </summary>
        static float DropDelay;
        /// <summary>
        /// Következő adag érkezéséig hátralévő idő.
        /// </summary>
        static float NextPack;
        /// <summary>
        /// Ennyi objektum semmisült meg bármiféle módon.
        /// </summary>
        static int Despawned;
        /// <summary>
        /// Ennyi objektum van a jelenlegi adagból fellőve.
        /// </summary>
        static int Shot;
        /// <summary>
        /// Ennyi objektum van még a jelenlegi adagból fellövetlen.
        /// </summary>
        static int ThisPack;

        /// <summary>
        /// Egy objektum eltűnésekor hívódik meg.
        /// </summary>
        public static void ObjectDespawned() {
            ++Despawned;
        }

        /// <summary>
        /// Alaphelyzetbe állítja a dobálót.
        /// </summary>
        public static void DispenserReset() {
            ThisPack = Despawned = 0;
            NextPack = 2; // 2 másodperc levegővétel a játékosnak pont elég az első adag előtt
        }

        /// <summary>
        /// Objektum feldobása erőltetve.
        /// </summary>
        public static void ForceDispense() {
            ++ThisPack;
            CurrentDrop = 0;
        }

        /// <summary>
        /// Dobáló frissítése: amikor szükség van rá (játék alatt), ezt képkockánként egyszer meg kell hívni.
        /// </summary>
        public static void DispenserUpdate() {
            // Új körök indítása
            if (ThisPack <= Despawned) {
                NextPack -= Time.deltaTime;
                if (NextPack <= 0) {
                    Time.timeScale += .025f;
                    ThisPack = Random.Range(1, 10);
                    Shot = Despawned = 0;
                    NextPack = 2;
                    DropDelay = CurrentDrop = Random.value * Convert.ToInt32(!Input.GetKey(KeyCode.C)); // Igen, C-t nyomva bedobja egyben, és igen, ez cheat
                }
                // Dolgok bedobálása
            } else if (ThisPack != Shot) {
                CurrentDrop -= Time.deltaTime;
                if (CurrentDrop <= 0) {
                    CurrentDrop = DropDelay;
                    Shot++;
                    GameObject Spawnie = DropDelay > .33f && Game.GameMode != GameModes.TimeAttack && Random.value < .2f
                        ? Game.Instance.ScaledGrenade() // Ha 1/3 másodpercnél ritkábban repülnek fel tárgyak, és nem időtámadást játszunk, akkor 20%, hogy bomba
                        : Game.Instance.RandomObject(); // Különben véletlenszerű tárgy
                    // A képernyő középső 3/4-ébe rakjon dolgokat
                    Spawnie.transform.position = Vector3.Lerp(Game.CloserBL, Game.CloserBR, Random.value * .75f + .125f) - new Vector3(0, Game.Instance.Scale * .02f, 0);
                    Spawnie.GetComponent<Rigidbody>().AddForce(Vector3.Lerp(Game.ForceLeft, Game.ForceRight, Random.value) +
                        new Vector3(0, Random.Range(Game.Instance.Scale * 16, Game.Instance.Scale * 18), 0)); // Hadd repüljön
                }
            }
        }
    }
}