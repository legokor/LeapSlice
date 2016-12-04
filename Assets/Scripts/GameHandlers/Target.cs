using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LeapSlice {
    /// <summary>
    /// Elszelhető tárgy.
    /// </summary>
    [RequireComponent(typeof(Despawner)), RequireComponent(typeof(Rigidbody))]
    public class Target : MonoBehaviour {
        /// <summary>
        /// Töredékek, amik a halálakor létrejönnek.
        /// </summary>
        public GameObject[] Shrapnels;

        /// <summary>
        /// Lehetséges tárgytípusok.
        /// </summary>
        public enum Types { Neutral, Grenade }
        /// <summary>
        /// Tárgytípus.
        /// </summary>
        public Types Type = Types.Neutral;

        /// <summary>
        /// Power up-e.
        /// </summary>
        public PowerUps PowerUp = PowerUps.None;

        /// <summary>
        /// El lett-e vágva.
        /// </summary>
        bool GotHit = false;
        /// <summary>
        /// Tömegtest-kezelő.
        /// </summary>
        Rigidbody Body;
        /// <summary>
        /// Forgatónyomaték.
        /// </summary>
        Vector3 Torque;

        /// <summary>
        /// Véletlen irányba forogjon a tárgy.
        /// </summary>
        void Start() {
            transform.rotation = Quaternion.Euler(Random.value * 360, Random.value * 360, Random.value * 360);
            Body = GetComponent<Rigidbody>();
            Body.angularVelocity = Torque = new Vector3(Random.value + .5f, Random.value + .5f, Random.value + .5f);
        }

        /// <summary>
        /// Forgás fenntartása, amikor frissül a fizikai motor.
        /// </summary>
        void FixedUpdate() {
            Body.angularVelocity = Torque;
        }

        /// <summary>
        /// Találatkor hívódik meg.
        /// </summary>
        /// <param name="Score">Elvágó játékos pontszáma</param>
        public void Hit(ref int Score) {
            if (GotHit || !Body)
                return;
            GotHit = true; // Ha ez nincs, rengetegszer meghívódna a függvény
            if (name.StartsWith("Menu"))
                Game.SelectedMenuItem = Convert.ToInt32(name.Substring(4)); // A kiválasztott menüelem száma a névben van
            if (Game.Playing) { // Csak játék közben módosítson statisztikákat, mert a Game Over menüvel nem tűnnek el -> nem bug, feature
                Dispenser.ObjectDespawned();
                Game.SinceLastCombo = 0;
                Game.ThisCombo++;
                Game.LastHit = transform.position;
                if (Type == Types.Grenade) {
                    ((GameObject)Instantiate(Game.Instance.Explosion, transform.position, transform.rotation)).transform.localScale *= Game.Instance.Scale * .1f; // Nagyobb robbanás effekt
                    foreach (Rigidbody EachBody in FindObjectsOfType(typeof(Rigidbody))) { // Lökjön el mindent magától
                        if (EachBody != Body) {
                            Destroy(EachBody.gameObject.GetComponent<Target>());
                            EachBody.gameObject.AddComponent<DespawnTime>();
                            Dispenser.ObjectDespawned();
                        }
                        EachBody.AddExplosionForce(Game.Instance.Scale * 50, transform.position, Game.Instance.Scale + Game.Instance.Scale);
                    }
                    Game.LoseLife = true;
                    if (Game.GameMode == GameModes.Multiplayer) // Többjátékos módban pontlevonás is jár a gránátért
                        Score -= 25;
                    Game.ThisCombo = 0;
                } else
                    Score++;
                // Ha power up vágódott el, aktiválódjon
                if (PowerUp != PowerUps.None) switch (PowerUp) {
                        case PowerUps.DoubleScore: PwrDoubleScore.Activate(); break;
                        case PowerUps.Itemstorm: PwrItemstorm.Activate(); break;
                        case PowerUps.SlowMotion: PwrSlowMotion.Activate(transform.position); break;
                    }
            }
            Vector3 ParentVelocity = Body.velocity;
            foreach (GameObject Shrapnel in Shrapnels) { // Essen szét darabokra
                Shrapnel.transform.parent = null;
                Shrapnel.AddComponent<Despawner>();
                Rigidbody ChildBody = Shrapnel.AddComponent<Rigidbody>();
                ChildBody.AddForce(ParentVelocity);
                ChildBody.AddTorque(new Vector3(Random.value * 100, Random.value * 100, Random.value * 100));
                ChildBody.AddExplosionForce(Game.Instance.Scale * 6, transform.position, 10);
            }
            ((GameObject)Instantiate(Game.Instance.Explosion, transform.position, transform.rotation)).transform.localScale *= Game.Instance.Scale * .02f; // Robbanás effekt
            SingleShotAudio.Play(Game.Instance.SliceSounds[Random.Range(0, Game.Instance.SliceSounds.Length)], transform.position); // Vágás hangja
            Destroy(gameObject);
        }

        /// <summary>
        /// Képernyőről kieséskor hívódik meg.
        /// </summary>
        public void Fell() {
            Dispenser.ObjectDespawned(); // Tudassuk a dobálóval, hogy valami eltűnt
            if (Type == Types.Grenade) { // A gránátnál az a jó, ha leesik
                Destroy(gameObject);
                return;
            }
            GameObject Marker = new GameObject(); // Jelző a képernyőre
            Marker.transform.position = transform.position;
            Marker.AddComponent<FallMarker>().Duration *= Time.timeScale;
            Game.LoseLife = true; // Életvesztés
        }
    }
}