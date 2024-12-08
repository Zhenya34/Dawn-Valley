using UnityEngine;

namespace Animals.Pets.PetsActivator
{
    public class AllPetsActivator : MonoBehaviour
    {
        [SerializeField] private Transform player;

        public void ActivatePet(string petName)
        {
            GameObject petObject = FindPetByName(petName);

            if (petObject)
            {
                petObject.SetActive(true);
                TeleportPetToPlayer(petObject);
            }
        }

        public void DeactivatePet(string petName)
        {
            GameObject petObject = FindPetByName(petName);

            if (petObject)
            {
                petObject.SetActive(false);
            }
        }

        private GameObject FindPetByName(string petName)
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.name == petName)
                {
                    return child.gameObject;
                }
            }
            return null;
        }

        private void TeleportPetToPlayer(GameObject petObject)
        {
            if (player)
            {
                petObject.transform.position = player.position;
            }
        }
    }
}