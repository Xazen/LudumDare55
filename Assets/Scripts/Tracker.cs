using UnityEngine;

namespace DefaultNamespace
{
    public class Tracker : MonoBehaviour
    {
        [SerializeField]
        private GameObjectPool slashPool;

        public void PlaySlashAnimation()
        {
            var slash = slashPool.GetObject();
            slash.transform.position = gameObject.transform.position;
            slash.gameObject.SetActive(true);
        }
    }
}