using UnityEngine;

namespace Mert.AudioSystem
{
    public class Clicker : MonoBehaviour
    {
        [SerializeField] SoundData click;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Click");
                SoundManager.Instance.CreateSound().WithSoundData(click).WithPosition(transform.position).WithRandomPitch().Play();
            }
        }
    }
}
