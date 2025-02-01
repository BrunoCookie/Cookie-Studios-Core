using UnityEngine;

namespace Audio
{
    public class AudioAddition : MonoBehaviour
    {
        public Sound[] sounds;

        private void Start()
        {
            foreach (Sound s in sounds)
            {
                AudioManager.instance.CreateAudioAddition(s);
            }
        }
    }
}