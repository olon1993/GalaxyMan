using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBounding2D : MonoBehaviour
{
	//**************************************************\\
	//********************* Fields *********************\\
	//**************************************************\\
	[SerializeField] protected bool _showDebugLog = false;

	[SerializeField] protected LayerMask collisionMask;
	protected Collider2D _collider;
	protected float _edgeCheckWidth = 0.1f;

	protected float _horizontalRaySpacing;
	protected float _verticalRaySpacing;
	protected const float _distanceBetweenRays = 0.15f;
	protected int _horizontalRayCount;
	protected int _verticalRayCount;
	protected ColliderCorners _colliderCorners;
	protected const float _skinWidth = 0.015f;


	//**************************************************\\
	//******************** Methods *********************\\
	//**************************************************\\

	protected virtual void Awake() {
		_collider = GetComponent<Collider2D>();
		if (_collider == null) {
			Debug.LogError("Collider2D not found on " + name);
		}
		CalculateRaySpacing();
	}

	protected void UpdateBounds() {

		Bounds bounds = _collider.bounds;
		_colliderCorners.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		_colliderCorners.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
		_colliderCorners.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
		_colliderCorners.TopRight = new Vector2(bounds.max.x, bounds.max.y);
	}

	protected void CalculateRaySpacing() {
		Bounds bounds = _collider.bounds;
		bounds.Expand(_skinWidth * -2);

		float boundsWidth = bounds.size.x;
		float boundsHeight = bounds.size.y;

		_horizontalRayCount = Mathf.RoundToInt(boundsHeight / _distanceBetweenRays);
		_verticalRayCount = Mathf.RoundToInt(boundsWidth / _distanceBetweenRays);

		_horizontalRaySpacing = bounds.size.y / (_horizontalRayCount - 1);
		_verticalRaySpacing = bounds.size.x / (_verticalRayCount - 1);
	}
	//**************************************************\\
	//******************* Properties *******************\\
	//**************************************************\\

	protected struct ColliderCorners
	{ 
		public Vector2 TopRight, TopLeft;
		public Vector2 BottomRight, BottomLeft;
	}
}
