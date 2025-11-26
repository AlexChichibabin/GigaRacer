using Unity.VisualScripting;
using UnityEngine;

namespace Racing
{
    public class FallRespawner : MonoBehaviour, IDependency<CarRespawner>
    {
        [SerializeField] private respawnMode mode;

        private new Collider collider;
        [SerializeField]  private CarRespawner respawner;

        public void Construct(CarRespawner obj) => respawner = obj;

        public enum respawnMode
        {
            OutOfRoad,
            Turing
        }

        private void Start()
        {
            collider = GetComponent<Collider>();
        }
        public void SetRespawner(CarRespawner respawner)
        {
            this.respawner = respawner;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (mode == respawnMode.OutOfRoad)
            {
                if (other.transform.root.GetComponent<Car>() is Car)
                {
                    respawner.Respawn();
                }
            }
            if (mode == respawnMode.Turing)
            {
                if (other.GetComponent<ActivatedTrackPoint>() is ActivatedTrackPoint) return;
                respawner.Respawn();
            }
        }

    }
}