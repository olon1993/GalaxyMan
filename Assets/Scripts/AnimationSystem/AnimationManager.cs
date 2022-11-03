using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TheFrozenBanana
{
    public class AnimationManager : MonoBehaviour, IAnimationManager
    {
        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        // Dependencies
        [SerializeField] private List<IAnimationState> _animationStates;

        private IAnimationState _currentAnimationState;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        private void Awake()
        {
            _animationStates = GetComponents<IAnimationState>().ToList();
            
            if(_animationStates.Count == 0)
            {
                Debug.LogError("No Animation States found on " + name);
            }
            else
            {
                _animationStates.Sort();
            }

        }

        private void Start()
        {
            _currentAnimationState = _animationStates[_animationStates.Count - 1];
            _currentAnimationState.OnStateEnter();
        }

        private void LateUpdate()
        {
            EvaluateState();
        }

        private void EvaluateState()
        {
            foreach (IAnimationState state in _animationStates)
            {
				if (state.ShouldPlay()) 
				{
                    CurrentAnimationState = state;
					break;
				}
			}
        }

        //**************************************************\\
        //******************* Properties *******************\\
        //**************************************************\\

        public IAnimationState CurrentAnimationState 
        {
            get { return _currentAnimationState; }
            private set
            {
                if (_currentAnimationState != value)
                {
                    if (_currentAnimationState.CanBeInterrupted == false)
                    {
                        return;
                    }

                    _currentAnimationState.OnStateExit();
                    _currentAnimationState = value;
                    _currentAnimationState.OnStateEnter();
                    _currentAnimationState.OnStateExecute();
                }
            }
        }
    }
}
