using UnityEngine;

namespace _Design_Patterns.StateMachine
{
    public abstract class State
    {
        public abstract void onStateEnter();

        public abstract void onStateUpdate();

        public abstract void onStateExit();

    public abstract void onCollisionEnter(Collision2D col);
    public abstract void onCollisionStay(Collision2D col);
    public abstract void onCollisionExit(Collision2D col);

    public abstract void onTriggerEnter(Collider2D col);
    public abstract void onTriggerStay(Collider2D col);
    public abstract void onTriggerExit(Collider2D col);
    }
}
