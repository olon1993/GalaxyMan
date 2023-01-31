using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{

	public class GameObjectSpawner : MonoBehaviour
	{
	    [SerializeField] private bool _showDebugLog = false;

		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\
		[SerializeField] private GameObject _blueprint;
		[SerializeField] private float _spawnTimeDelay = 1f;
		private float _timer = 0f;

	    //**************************************************\\
	    //******************** Methods *********************\\
	    //**************************************************\\

	    #region PublicMethods

	    #endregion


	    #region UnityMethods

	    void Awake()
	    {
        
	    }

	    void Start()
	    {
        
	    }

	    void Update()
	    {
			_timer += Time.deltaTime;

			if(_timer >= _spawnTimeDelay)
            {
				Instantiate(_blueprint, transform.position, transform.rotation);
				_timer = 0f;
            }
	    }

	    #endregion


	    #region PrivateMethods

	    #endregion


	    //**************************************************\\
	    //****************** Properties ********************\\
	    //**************************************************\\


	}
}
