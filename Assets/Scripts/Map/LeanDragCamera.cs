using CW.Common;
using Lean.Common;
using UnityEngine;

namespace Lean.Touch
{
	/// <summary>This component allows you to move the current GameObject (e.g. Camera) based on finger drags and the specified ScreenDepth.</summary>
	[HelpURL(LeanTouch.HelpUrlPrefix + "LeanDragCamera")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Drag Camera")]
	public class LeanDragCamera : MonoBehaviour
	{
		/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
		public LeanFingerFilter Use = new LeanFingerFilter(true);

		/// <summary>The method used to find world coordinates from a finger. See LeanScreenDepth documentation for more information.</summary>
		public LeanScreenDepth ScreenDepth = new LeanScreenDepth(LeanScreenDepth.ConversionType.DepthIntercept);

		/// <summary>The movement speed will be multiplied by this.
		/// -1 = Inverted Controls.</summary>
		[Tooltip("The movement speed will be multiplied by this.\n\n-1 = Inverted Controls.")]
		public float Sensitivity = 1.0f;

		/// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
		/// -1 = Instantly change.
		/// 1 = Slowly change.
		/// 10 = Quickly change.</summary>
		[Tooltip("If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.")]
		public float Dampening = -1.0f;

		/// <summary>This allows you to control how much momenum is retained when the dragging fingers are all released.
		/// NOTE: This requires <b>Dampening</b> to be above 0.</summary>
		[Tooltip("This allows you to control how much momenum is retained when the dragging fingers are all released.\n\nNOTE: This requires <b>Dampening</b> to be above 0.")]
		[Range(0.0f, 1.0f)]
		public float Inertia;

		[HideInInspector]
		[SerializeField]
		private Vector3 remainingDelta;

		private bool _isEnabled = true;
		public bool isEnabled
		{
			get => _isEnabled;
			set
			{
				if (_isEnabled != value)
				{
					_isEnabled = value;
					if (!_isEnabled)
					{
						// Reset remainingDelta when the component is disabled
						remainingDelta = Vector3.zero;
					}
				}
			}
		}


		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually add a finger.</summary>
		public void AddFinger(LeanFinger finger)
		{
			Use.AddFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove a finger.</summary>
		public void RemoveFinger(LeanFinger finger)
		{
			Use.RemoveFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove all fingers.</summary>
		public void RemoveAllFingers()
		{
			Use.RemoveAllFingers();
		}
#if UNITY_EDITOR
		protected virtual void Reset()
		{
			Use.UpdateRequiredSelectable(gameObject);
		}
#endif
		protected virtual void Awake()
		{
			Use.UpdateRequiredSelectable(gameObject);
		}

		protected virtual void LateUpdate()
		{
			if (!_isEnabled) return;
			// Get the fingers we want to use
			var fingers = Use.UpdateAndGetFingers();

			// Get the last and current screen point of all fingers
			var lastScreenPoint = LeanGesture.GetLastScreenCenter(fingers);
			var screenPoint     = LeanGesture.GetScreenCenter(fingers);

			// Get the world delta of them after conversion
			var worldDelta = ScreenDepth.ConvertDelta(lastScreenPoint, screenPoint, gameObject);

			// Store the current position
			var oldPosition = transform.localPosition;

			// Pan the camera based on the world delta
			transform.position -= worldDelta * Sensitivity;

			// Add to remainingDelta
			remainingDelta += transform.localPosition - oldPosition;

			// Get t value
			var factor = CwHelper.DampenFactor(Dampening, Time.deltaTime);

			// Dampen remainingDelta
			var newRemainingDelta = Vector3.Lerp(remainingDelta, Vector3.zero, factor);

			// Shift this position by the change in delta
			transform.localPosition = oldPosition + remainingDelta - newRemainingDelta;

			if (fingers.Count == 0 && Inertia > 0.0f && Dampening > 0.0f)
			{
				newRemainingDelta = Vector3.Lerp(newRemainingDelta, remainingDelta, Inertia);
			}

			// Update remainingDelta with the dampened value
			remainingDelta = newRemainingDelta;
			ConstrainCamera();

		}
		
		[Tooltip("The camera whose orthographic size will be used.")]
		public Camera Camera;

		[Tooltip("The plane this transform will be constrained to")]
		public LeanPlane Plane;

		public void ConstrainCamera()
		{
			if (Camera != null)
			{
				if (Plane != null)
				{
					var ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
					var hit = default(Vector3);

					if (Plane.TryRaycast(ray, ref hit, 0.0f, false) == true)
					{
						var delta   = transform.position - hit;
						var local   = Plane.transform.InverseTransformPoint(hit);
						var snapped = local;
						var size    = new Vector2(Camera.orthographicSize * Camera.aspect, Camera.orthographicSize);

						if (Plane.ClampX == true)
						{
							var min = Plane.MinX + size.x;
							var max = Plane.MaxX - size.x;

							if (min > max)
							{
								snapped.x = (min + max) * 0.5f;
							}
							else
							{
								snapped.x = Mathf.Clamp(local.x, min, max);
							}
						}

						if (Plane.ClampY == true)
						{
							var min = Plane.MinY + size.y;
							var max = Plane.MaxY - size.y;

							if (min > max)
							{
								snapped.y = (min + max) * 0.5f;
							}
							else
							{
								snapped.y = Mathf.Clamp(local.y, min, max);
							}
						}

						if (local != snapped)
						{
							transform.position = Plane.transform.TransformPoint(snapped) + delta;
						}
					}
				}
			}
			else
			{
				Debug.LogError("Failed to find camera. Either tag your cameras MainCamera, or set one in this component.", this);
			}
		}
	}
}