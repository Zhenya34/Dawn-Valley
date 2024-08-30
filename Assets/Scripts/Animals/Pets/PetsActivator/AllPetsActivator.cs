using UnityEngine;

public class AllPetsActivator : MonoBehaviour
{
    [SerializeField] private Transform _player;

    public void ActivatePet(string petName)
    {
        GameObject petObject = FindPetByName(petName);

        if (petObject != null)
        {
            petObject.SetActive(true);
            TeleportPetToPlayer(petObject);
        }
    }

    public void DeactivatePet(string petName)
    {
        GameObject petObject = FindPetByName(petName);

        if (petObject != null)
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
        if (_player != null)
        {
            petObject.transform.position = _player.position;
        }
    }
}