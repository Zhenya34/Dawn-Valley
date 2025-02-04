using UnityEngine;

namespace Player.Placement
{
    public class SoundFeedback : MonoBehaviour
    {
        [SerializeField] private AudioClip clickSound, placeSound, removeSound, wrongPlacementSound;
        [SerializeField] private AudioSource audioSource;

        public void PlaySound(SoundType soundType)
        {
            if (soundType == SoundType.Click)
                audioSource.PlayOneShot(clickSound);
            else if (soundType == SoundType.Place)
                audioSource.PlayOneShot(placeSound);
            else if (soundType == SoundType.Remove)
                audioSource.PlayOneShot(removeSound);
            else if (soundType == SoundType.WrongPlacement) 
                audioSource.PlayOneShot(wrongPlacementSound);
        }
    }

    public enum SoundType
    {
        Click,
        Place,
        Remove,
        WrongPlacement
    }
}