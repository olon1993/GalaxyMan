using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{

    public abstract class AnimationState : MonoBehaviour, IAnimationState
    {

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        protected DependencyManager _dependencyManager;
        protected Animator _animator;

        [SerializeField] protected AnimationClip _animation;
        [SerializeField] protected int _priority;
        [SerializeField] protected int _animationLayer = 0;
        [SerializeField] protected bool _canBeInterrupted = true;
        [SerializeField] protected bool _isAnimationOverride;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        protected virtual void Awake()
        {
            _dependencyManager = GetComponentInParent<DependencyManager>();
            if (_dependencyManager == null)
            {
                Debug.LogError("DependencyManager not found on " + name);
            }

            _animator = (Animator)_dependencyManager.Registry[typeof(Animator)];
            if (_animator == null)
            {
                Debug.LogError("Animator not found on " + name);
            }
        }

        public virtual int CompareTo(IAnimationState other)
        {
            if (other.Priority > Priority)
            {
                return 1;
            }

            return -1;
        }

        public virtual void OnStateEnter() { }

        public virtual void OnStateExecute() 
        {
            _animator.Play(_animation.name, _animationLayer);
        }

        public virtual void OnStateExit() { }

        public abstract bool ShouldPlay();

        //**************************************************\\
        //******************* Properties *******************\\
        //**************************************************\\

        public virtual bool CanBeInterrupted
        {
            get { return _canBeInterrupted; }
            protected set { _canBeInterrupted = value; }
        }

        public virtual int Priority { get { return _priority; } }

    }
}
