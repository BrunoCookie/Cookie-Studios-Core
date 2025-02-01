using Audio;
using UnityEngine;

namespace Singletons.Scripts.Audio
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