using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class Healthbar : MonoBehaviour
    {
        public Life life;
        public bool showOnStart;
        public bool shouldFadeOut;
        public float fadeOutTime;
    
        [Space(5)]
        public Gradient gradient;

        private Vector3 posOffset;
        private CanvasGroup canvasGroup;
        private float fadeOutTimer;

        private void Awake(){
            this.canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start(){
            this.posOffset = transform.position  - this.life.transform.position;
            this.life.onHealthChange += UpdateHealthbar;
        
            UpdateHealthbar();
            this.canvasGroup.alpha = this.showOnStart ? 1 : 0;
        }
    
        private void LateUpdate(){
            transform.rotation = Quaternion.identity;
            transform.position = this.life.transform.position + this.posOffset;

            if(Time.time > this.fadeOutTimer && this.shouldFadeOut){
                this.canvasGroup.alpha = 0;
            }
        }

        private void UpdateHealthbar(){
            float quotient = (float)this.life.health / this.life.maxHealth;
            GetComponent<Slider>().value = quotient;
            transform.GetChild(0).GetComponent<Image>().color = this.gradient.Evaluate(quotient);
            this.canvasGroup.alpha = 1;
            this.fadeOutTimer = Time.time + this.fadeOutTime;
        }
    }
}
