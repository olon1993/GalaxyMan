using UnityEngine;

namespace TheFrozenBanana
{
    public class ProjectileWeapon : MonoBehaviour, IWeapon
    {
        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        [SerializeField] private Transform _pointOfOrigin;
        [SerializeField] private Transform _pointOfTargetting;

        [SerializeField] GameObject DefaultProjectile;

        [SerializeField] private float _halfChargeTime = 0.5f;
        [SerializeField] GameObject HalfChargedProjectile;

        [SerializeField] private float _fullChargeTime = 1f;
        [SerializeField] GameObject FullChargedProjectile;


        [SerializeField] private bool _isLimitedAmmo;
        [SerializeField] private int _maxAmmo;
        [SerializeField] private int _currentAmmo;
        [SerializeField] private IWeapon.AmmoType _ammoType;
		[SerializeField] private int _animationLayer;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        public void Attack(float charge)
        {
            if (IsLimitedAmmo)
            {
                if (CurrentAmmo <= 0)
                {
                    Debug.Log(gameObject.name + " could not attack because they are out of ammo!");
                    return;
                }

                if (MaxAmmo > 0)
                {
                    CurrentAmmo -= 1;
                }
            }

            GameObject insProjectile = null;
            if (charge < _halfChargeTime)
            {
                insProjectile = Instantiate(DefaultProjectile, _pointOfOrigin.position, _pointOfTargetting.rotation);
            }
            else if(charge < _fullChargeTime)
            {
                insProjectile = Instantiate(HalfChargedProjectile, _pointOfOrigin.position, _pointOfTargetting.rotation);
            }
            else
            {
                insProjectile = Instantiate(FullChargedProjectile, _pointOfOrigin.position, _pointOfTargetting.rotation);
            }

            // Instantiate the weapon gameObject
            Projectile2d projectile2D = insProjectile.GetComponent<Projectile2d>();
            projectile2D.Direction = _pointOfTargetting.position - _pointOfOrigin.position;
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 direction = _pointOfTargetting.position - _pointOfOrigin.position;
            Gizmos.DrawRay(_pointOfOrigin.position, direction);
        }

        //**************************************************\\
        //******************* Properties *******************\\
        //**************************************************\\

        public bool IsLimitedAmmo
        {
            get { return _isLimitedAmmo; }
            set { _isLimitedAmmo = value; }
        }

        public int MaxAmmo
        {
            get { return _maxAmmo; }
            set { _maxAmmo = value; }
        }

        public int CurrentAmmo
        {
            get { return _currentAmmo; }
            set { _currentAmmo = value; }
        }

        public IWeapon.AmmoType AmmoTypeDefinition
        {
            get { return _ammoType; }
            set { _ammoType = value; }
        }

        public Transform PointOfOrigin
        {
            get { return _pointOfOrigin; }
            set { _pointOfOrigin = value; }
        }

        public Transform PointOfTargetting
        {
            get { return _pointOfTargetting; }
            set { _pointOfTargetting = value; }
        }

        public float AttackActionTime { get; }

		public int AnimationLayer {
			get { return _animationLayer; }
			set { _animationLayer = value; }
		}

        public float AttackCharge { get; set; }
    }
}
