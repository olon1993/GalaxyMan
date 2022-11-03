using System;
using System.Collections;
using System.Collections.Generic;
using TheFrozenBanana;
using UnityEngine;

public class DependencyManager : MonoBehaviour
{
    private Dictionary<Type, object> _dependencies;

    // Unity Dependencies
    [SerializeField] private bool _registerAnimator = false;
    [SerializeField] private bool _registerCharacterController = false;
    [SerializeField] private bool _registerRigidbody3d = false;
    [SerializeField] private bool _registerRigidbody2d = false;

    // Custom Dependencies
    [SerializeField] private bool _registerInputManager = false;
    [SerializeField] private bool _registerInputManager3d = false;
    [SerializeField] private bool _registerAnimationManager = false;
    [SerializeField] private bool _registerLocomotion = false; 
    [SerializeField] private bool _registerLocomotion2dSideScroller = false;
    [SerializeField] private bool _registerCombatant = false;
    [SerializeField] private bool _registerHealth = false;

    private void Awake()
    {
        _dependencies = new Dictionary<Type, object>();

        if (_registerAnimator)
        {
            Register(typeof(Animator));
        }

        if (_registerCharacterController)
        {
            Register(typeof(CharacterController));
        }

        if (_registerRigidbody3d)
        {
            Register(typeof(Rigidbody));
        }

        if (_registerRigidbody2d)
        {
            Register(typeof(Rigidbody2D));
        }

        if (_registerInputManager)
        {
            Register(typeof(IInputManager));
        }

        if (_registerInputManager3d)
        {
            Register(typeof(IInputManager3d));
        }

        if (_registerAnimationManager)
        {
            Register(typeof(IAnimationManager));
        }

        if (_registerLocomotion)
        {
            Register(typeof(ILocomotion));
        }

        if (_registerLocomotion2dSideScroller)
        {
            Register(typeof(ILocomotion2dSideScroller));
        }

        if (_registerCombatant)
        {
            Register(typeof(ICombatant));
        }

        if (_registerHealth)
        {
            Register(typeof(IHealth));
        }
    }

    private void Register(Type type)
    {
        var dependency = GetComponentInChildren(type);

        if(dependency == null)
        {
            dependency = GetComponent(type);
        }

        if (dependency != null)
        {
            _dependencies.Add(type, dependency);
        }
        else
        {
            Debug.LogError("No " + type.Name + " found on " + gameObject.name + ". Either the registry request is invalid or an error occurred.");
        }
    }

    public Dictionary<Type, object> Registry { get { return _dependencies; } }
}
