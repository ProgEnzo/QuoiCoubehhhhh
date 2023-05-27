
using UnityEngine;

namespace Objects.Target
{
    public class Target : MonoBehaviour
    {
        [SerializeField] private GameObject lockUI;
        
        private void Start()
        {
            //PlayerController.instance.targets.Add(this);
        }

        public void ManagerLockUI(bool active)
        {
            lockUI.SetActive(active);
        }

        /*private void OnTriggerEnter(Collider other)
        {
            if (PlayerController.instance.haveSnowBallInHand) return;

            if (other.CompareTag("SnowBall"))
            {
                other.GetComponent<Snowball>().Explode();
                HaveBeenShooted();
            }
        }*/
        
        // Appel de la fonction quand on touche la cible avce une boule de neige
        protected virtual void HaveBeenShooted()
        {
            // Fonction à override pour chaque type de cible pour effectuer des action différentes
        }
    }
}

