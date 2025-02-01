using UnityEngine;

namespace _Design_Patterns.StateMachine
{
    public abstract class StateManager : MonoBehaviour
    {
        public State currentState;

        protected virtual void Start()
        {
            //Declare all States here
            //Initialize currentState

            currentState.onStateEnter();
        }

        protected virtual void Update()
        {
            currentState.onStateUpdate();
        }

        public virtual void SwitchState(State state)
        {
            currentState.onStateExit();
            currentState = state;
            currentState.onStateEnter();
        }

        protected virtual void OnCollisionEnter2D(Collision2D other)
        {
            currentState.onCollisionEnter(other);
        }

        protected virtual void OnCollisionStay2D(Collision2D other)
        {
            currentState.onCollisionStay(other);
        }

        protected virtual void OnCollisionExit2D(Collision2D other)
        {
            currentState.onCollisionExit(other);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            currentState.onTriggerEnter(other);
        }

        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            currentState.onTriggerStay(other);
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            currentState.onTriggerExit(other);
        }
    }
}