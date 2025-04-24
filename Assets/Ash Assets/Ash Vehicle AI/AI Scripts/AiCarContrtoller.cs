
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;

namespace AshVP
{

	public class AiCarContrtoller : MonoBehaviour
	{
		/// <summary>
		/// Reference to the current lane.
		/// </summary>
		public HR_Lane currentLane;

		/// <summary>
		/// Reference to the lane manager.
		/// </summary>
		private HR_TrafficManager trafficManager;
		public HR_TrafficManager TrafficManager
		{

			get
			{

				if (trafficManager == null)
					trafficManager = HR_TrafficManager.Instance;

				return trafficManager;

			}

		}


		/// <summary>
		/// Property to get the Rigidbody component.
		/// </summary>


		private Rigidbody rigid;

		[Header("Trigger Collider Settings")]
		[Tooltip("Multiplier for extending the trigger volume along the Z-axis.")]
		[SerializeField] private float triggerSizeZMultiplier = 2f;

		[Tooltip("Offset to push the trigger forward relative to the vehicle's bounds.")]
		[SerializeField] private float triggerForwardOffset = 4f;

		[HideInInspector]
		public BoxCollider triggerCollider;


		[Header("Suspension")]
		[Range(0, 5)]
		public float SuspensionDistance = 0.2f;
		public float suspensionForce = 30000f;
		public float suspensionDamper = 200f;
		public Transform groundCheck;
		public Transform fricAt;
		public Transform CenterOfMass;

		[Header("Car Stats")]
		public float accelerationForce = 200f;
		public float turnTorque = 100f;
		public float brakeForce = 150f;
		public float frictionForce = 70f;
		public float dragAmount = 4f;
		public float TurnAngle = 30f;

		public float maxRayLength = 0.8f, slerpTime = 0.2f;
		private float VehicleGravity = -30;
		private Vector3 centerOfMass_ground;

		[HideInInspector]
		public bool grounded;

		public Transform TargetTransform;
		[Header("Visuals")]
		public Transform[] TireMeshes;
		public Transform[] TurnTires;

		[Header("Curves")]
		public AnimationCurve frictionCurve;
		public AnimationCurve accelerationCurve;
		public bool separateReverseCurve = false;
		public AnimationCurve ReverseCurve;
		public AnimationCurve turnCurve;
		public AnimationCurve driftCurve;
		public AnimationCurve engineCurve;

		private float speedValue, fricValue, turnValue, curveVelocity;
		[HideInInspector]
		public Vector3 carVelocity;
		[HideInInspector]
		public RaycastHit hit;

		[Header("Other Settings")]
		public AudioSource[] engineSounds;
		public bool airDrag;
		public float SkidEnable = 20f;
		public float skidWidth = 0.12f;

		//Ai stuff
		[HideInInspector]
		public float TurnAI = 1f;
		[HideInInspector]
		public float SpeedAI = 1f;

		public float brakeAngle = 30f;
		private float desiredTurning;

		public float spawnHeight = 0f;

		public float pointToPointThreshold = 4;

		private List<Transform> nearbyCars = new List<Transform>();
		private AiCarContrtoller closestCar;

		/// <summary>
		/// Array of headLights
		/// </summary>
		public Light[] headLights;

		/// <summary>
		/// Array of brake lights.
		/// </summary>
		public Light[] brakeLights;

		/// <summary>
		/// Array of signal lights.
		/// </summary>
		public Light[] signalLights;

		/// <summary>
		/// Enum representing the direction of changing lanes.
		/// </summary>
		public enum ChangingLines { Straight, Right, Left }

		/// <summary>
		/// Current direction of lane change
		/// </summary>
		[HideInInspector]
		public ChangingLines changingLines = ChangingLines.Straight;

		/// <summary>
		/// Enum representing the state of the signals.
		/// </summary>
		private enum SignalsOn { Off, Right, Left, All }

		/// <summary>
		/// Current state of the signals.
		/// </summary>
		private SignalsOn signalsOn = SignalsOn.Off;

		private bool headlightsOn = false;
		private bool brakingOn = false;
		private float signalTimer = 0f;

		public bool isChangingLane = false;
		public bool oppositeDirection;
		private bool crashed = false;

		private float adjustedSpeedValue;
		private float adjustedBrakeForce;

		private float closestCarDistance = 0f;
		private float[] tempDistances;
		private int[] tempIndices;
		private enum LaneChangeState
		{
			Idle,
			Signaling,
			Changing,
			Completed
		}

		private LaneChangeState laneChangeState = LaneChangeState.Idle;
		private float laneChangeTimer = 0f;
		private float signalDuration = 0.5f; // sinyal süresi
		private float changeDuration = 0.2f; // şerit değişim süresi

		private HR_Lane pendingLane;

        private bool isBoosting = false;
        private float boostDuration = 4f;
        private float boostMultiplier = 1f;
        private float boostStartTime = 0f;
        private float boostTarget = 1.3f;
        private float boostInitial = 1f;

        private void Awake()
		{
			accelerationForce = Random.Range(1100, 1200);

			Light[] allLights = GetComponentsInChildren<Light>();

			foreach (Light light in allLights)
				light.renderMode = LightRenderMode.ForceVertex;

			// Adjust center of mass and gravity
			InitializeVehiclePhysics();
		}
		private void Start()
		{
			if (!oppositeDirection)
			{
                InvokeRepeating(nameof(AutoChangeLine), Random.Range(15, 45), Random.Range(15, 45));
            }

            // Max kaç noktaya kadar desteklemek istiyorsan ona göre ayarla
            int maxPoints = 5;

			tempDistances = new float[maxPoints];
			tempIndices = new int[maxPoints];

        }

		public void RealignCar()
		{
			rigid.linearVelocity = new Vector3(0f, 0f, Random.Range(16f, 20f));
			crashed = false;

			
			signalsOn = SignalsOn.Off;
			changingLines = ChangingLines.Straight;

			// Enabling headlights if it's night.
			headlightsOn = TrafficManager.GameplayManager != null && (TrafficManager.GameplayManager.dayOrNight == HR_GamePlayManager.DayOrNight.Night);

            if (oppositeDirection)
            {
                // Bu üçü kritik!
                laneChangeState = LaneChangeState.Idle;
                pendingLane = null;
            }
        }

        void FixedUpdate()
		{
			// Acceleration boost süresi dolduysa, 1'e kadar düşür
			carVelocity = transform.InverseTransformDirection(rigid.linearVelocity); //local velocity of car

			curveVelocity = Mathf.Abs(carVelocity.magnitude) / 100;

			//inputs
			float turnInput = crashed == true ? 0 : turnTorque * TurnAI * Time.fixedDeltaTime * 1000;
			float speedInput = crashed == true ? 0 : accelerationForce * SpeedAI * Time.fixedDeltaTime * 1000;
		
			//helping veriables
			speedValue = speedInput * accelerationCurve.Evaluate(Mathf.Abs(carVelocity.z) / 100);
			fricValue = frictionForce * frictionCurve.Evaluate(carVelocity.magnitude / 100);

			// the new method of calculating turn value
			Vector3 aimedPoint = TargetTransform.position;
			aimedPoint.y = transform.position.y;
			Vector3 aimedDir = (aimedPoint - transform.position).normalized;
			Vector3 myDir = transform.forward;
			myDir.y = 0;
			myDir.Normalize();
			desiredTurning = Mathf.Abs(Vector3.Angle(myDir, aimedDir));
            float rawTurn = turnInput * turnCurve.Evaluate(desiredTurning / TurnAngle);
            turnValue = Mathf.Clamp(rawTurn, -3000f, 2900f); // Sınır koy: denge sağlar


            //grounded check
            if (Physics.Raycast(groundCheck.position, -transform.up, out hit, maxRayLength))
			{
				accelarationLogic();
				turningLogic();
				frictionLogic();
				brakeLogic();

                if (isBoosting)
                {
                    float boostProgress = (Time.time - boostStartTime) / boostDuration;
                    boostProgress = Mathf.Clamp01(boostProgress);

                    // yavaş yavaş boost değerine yaklaş (örneğin 1.0 → 1.5)
                    boostMultiplier = Mathf.Lerp(boostInitial, boostTarget, boostProgress);

                    if (boostProgress >= 1f)
                    {
                        isBoosting = false;
                        boostMultiplier = 1f;
                    }
                }



                //for drift behaviour
                rigid.angularDamping = dragAmount * driftCurve.Evaluate(Mathf.Abs(carVelocity.x) / 70);

				//draws green ground checking ray ....ingnore
				Debug.DrawLine(groundCheck.position, hit.point, Color.green);
				grounded = true;

				//rb.linearDamping = 0.1f;

				rigid.centerOfMass = centerOfMass_ground;

				switch (laneChangeState)
				{
					case LaneChangeState.Signaling:
						laneChangeTimer += Time.deltaTime;
						if (laneChangeTimer >= signalDuration)
						{
							laneChangeTimer = 0f;
							laneChangeState = LaneChangeState.Changing;
						}
						break;

					case LaneChangeState.Changing:
						laneChangeTimer += Time.deltaTime;
						if (laneChangeTimer >= changeDuration)
						{
							currentLane = pendingLane;
							laneChangeTimer = 0f;
							laneChangeState = LaneChangeState.Completed;
						}
						break;

					case LaneChangeState.Completed:
						laneChangeTimer += Time.deltaTime;
						if (laneChangeTimer >= 1f)
						{
							isChangingLane = false;
							changingLines = ChangingLines.Straight;
							laneChangeTimer = 0f;
							laneChangeState = LaneChangeState.Idle;
						}
						break;
				}

				Lights();
			}
			else
			{
				grounded = false;
				rigid.linearDamping = 0.1f;
				rigid.centerOfMass = CenterOfMass.localPosition;
				if (!airDrag)
				{
					//rb.angularDamping = 0.1f;
				}

			}


		}

		void Update()
		{
            float rotationSmoothness = 3f;

            int baseIndex = GetClosestWaypointToCar(1);
            Vector3 lookaheadTarget = GetConstrainedLookaheadTarget(baseIndex, rotationSmoothness);

            Vector3 dirToTarget = (lookaheadTarget - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToTarget);
            if (angle > 60f)
            {
                lookaheadTarget = currentLane.points[baseIndex].position;
            }

            TargetTransform.position = lookaheadTarget;

            tireVisuals();
			//audioControl();

			Vector3 dirToMovePosition = (TargetTransform.position - transform.position).normalized;
			
            float angleToDir = Vector3.SignedAngle(transform.forward, dirToMovePosition, Vector3.up);

            if (angleToDir > 0)
            {
                TurnAI = 1f * turnCurve.Evaluate(desiredTurning / TurnAngle);
            }
            else
            {
                TurnAI = -1f * turnCurve.Evaluate(desiredTurning / TurnAngle);
            }

            closestCarDistance = closestCar != null ? Vector3.Distance(transform.position, closestCar.transform.position) : 0f;

			brakingOn = crashed || (closestCarDistance != 0 && closestCarDistance < 15f);
		}

		private void Lights()
		{
			signalsOn = SignalsOn.Off;

			if (changingLines == ChangingLines.Right)
				signalsOn = SignalsOn.Right;

			if (changingLines == ChangingLines.Left)
				signalsOn = SignalsOn.Left;

			signalTimer += Time.deltaTime;

			for (int i = 0; i < signalLights.Length; i++)
			{

				signalLights[i].intensity = signalsOn switch
				{
					SignalsOn.Off => 0f,
					SignalsOn.Left when signalLights[i].transform.localPosition.x < 0f => signalTimer >= .5f ? 0f : 1f,
					SignalsOn.Right when signalLights[i].transform.localPosition.x > 0f => signalTimer >= .5f ? 0f : 1f,
					SignalsOn.All => signalTimer >= .5f ? 0f : 1f,
					_ => signalLights[i].intensity
				};

				if (signalTimer >= 1f)
					signalTimer = 0f;

			}

			for (int i = 0; i < headLights.Length; i++)
				headLights[i].intensity = headlightsOn ? 1f : 0f;

			for (int i = 0; i < brakeLights.Length; i++)
				brakeLights[i].intensity = brakingOn ? 1f : (headlightsOn ? .6f : 0f);

		}

		private void OnCollisionEnter(Collision col)
		{
			if (col.transform.CompareTag(Settings.PLAYER_TAG))
			{
				crashed = true;
				signalsOn = SignalsOn.All;

			}
			if (col.transform.CompareTag(Settings.TRAFFIC_CAR_TAG))
				signalsOn = SignalsOn.All;

		}

		private void OnTriggerEnter(Collider col)
		{
			if (col.CompareTag(Settings.TRAFFIC_CAR_TAG))
			{
				if (!nearbyCars.Contains(col.transform))
				{
					nearbyCars.Add(col.transform);
					UpdateClosestCar();
				}

			}
		}

		private void OnTriggerExit(Collider col)
		{
			if (col.CompareTag(Settings.TRAFFIC_CAR_TAG))
			{
				if (nearbyCars.Contains(col.transform))
				{
					nearbyCars.Remove(col.transform);
					UpdateClosestCar();

					if (nearbyCars.Count == 0)
					{
						closestCar = null;
					}
				}
			}
		}

		private void UpdateClosestCar()
		{
			if (nearbyCars.Count == 0)
			{
				closestCar = null;
				return;
			}

			closestCar = nearbyCars[0].GetComponent<AiCarContrtoller>();
			float minDistance = Vector3.Distance(transform.position, closestCar.transform.position);
			foreach (Transform car in nearbyCars)
			{
				float distance = Vector3.Distance(transform.position, car.position);
				if (distance < minDistance)
				{
					minDistance = distance;
					closestCar = car.GetComponent<AiCarContrtoller>(); 
				}
			}

		}

        private Vector3 GetConstrainedLookaheadTarget(int baseIndex, float lookaheadDistance = 10f)
        {
            float totalDistance = 0f;
            int targetIndex = baseIndex;

            for (int i = baseIndex; i < currentLane.points.Count - 1; i++)
            {
                float segmentDistance = Vector3.Distance(currentLane.points[i].position, currentLane.points[i + 1].position);
                totalDistance += segmentDistance;

                if (totalDistance >= lookaheadDistance)
                {
                    targetIndex = i + 1;
                    break;
                }
            }

            Vector3 p1 = currentLane.points[baseIndex].position;
            Vector3 p2 = currentLane.points[targetIndex].position;

            float t = Mathf.Clamp01(lookaheadDistance / (totalDistance + 0.001f));
            return Vector3.Lerp(p1, p2, t);
        }



        private int GetClosestWaypointToCar(int position)
		{
			int count = 0;
			Vector3 vehicleForward = transform.forward;

			for (int i = 0; i <= position; i++)
			{
				tempDistances[i] = float.MaxValue;
				tempIndices[i] = -1;
			}

			for (int i = 0; i < currentLane.points.Count; i++)
			{
				Vector3 toPoint = currentLane.points[i].position - transform.position;
				Vector3 dir = toPoint.normalized;
				if (Vector3.Dot(vehicleForward, dir) <= 0f) continue;

				float sqrDist = toPoint.sqrMagnitude;

				for (int j = 0; j <= position; j++)
				{
					if (sqrDist < tempDistances[j])
					{
						for (int k = position; k > j; k--)
						{
							tempDistances[k] = tempDistances[k - 1];
							tempIndices[k] = tempIndices[k - 1];
						}

						tempDistances[j] = sqrDist;
						tempIndices[j] = i;
						break;
					}
				}
			}

			return tempIndices[position] != -1 ? tempIndices[position] : (tempIndices[0] != -1 ? tempIndices[0] : 0);
		}


		public void audioControl()
		{
			//audios
			if (grounded)
			{
				if (Mathf.Abs(carVelocity.x) > SkidEnable - 0.1f)
				{
					engineSounds[1].mute = false;
				}
				else { engineSounds[1].mute = true; }
			}
			else
			{
				engineSounds[1].mute = true;
			}

			engineSounds[1].pitch = 1f;

			engineSounds[0].pitch = 2 * engineCurve.Evaluate(curveVelocity);
			if (engineSounds.Length == 2)
			{
				return;
			}
			else { engineSounds[2].pitch = 2 * engineCurve.Evaluate(curveVelocity); }



		}

		public void tireVisuals()
		{
			//Tire mesh rotate
			foreach (Transform mesh in TireMeshes)
			{
				mesh.transform.RotateAround(mesh.transform.position, mesh.transform.right, carVelocity.z / 3);
			}

			//TireTurn
			foreach (Transform FM in TurnTires)
			{
				FM.localRotation = Quaternion.Slerp(FM.localRotation, Quaternion.Euler(FM.localRotation.eulerAngles.x,
					Mathf.Clamp(desiredTurning, desiredTurning, TurnAngle) * TurnAI, FM.localRotation.eulerAngles.z), slerpTime);
			}
		}

        public void accelarationLogic()
        {
            float accelerationMultiplier = 1f;

            if (closestCar != null)
            {
                accelerationMultiplier = Mathf.Lerp(accelerationMultiplier, Mathf.Clamp01(closestCarDistance / 29), Time.deltaTime * 10f);
            }

            adjustedSpeedValue = speedValue * accelerationMultiplier * boostMultiplier;
            rigid.AddForceAtPosition(transform.forward * adjustedSpeedValue, groundCheck.position);
        }


        public void turningLogic()
		{
			//turning
			if (carVelocity.z > 0.1f)
			{
				rigid.AddTorque(transform.up * turnValue);
			}

			if (carVelocity.z < -0.1f)
			{
				rigid.AddTorque(transform.up * -turnValue);
			}
		}


		public void frictionLogic()
		{
			Vector3 sideVelocity = carVelocity.x * transform.right;

			Vector3 contactDesiredAccel = -sideVelocity / Time.fixedDeltaTime;

			float clampedFrictionForce = rigid.mass * contactDesiredAccel.magnitude;

			Vector3 gravityForce = VehicleGravity * rigid.mass * Vector3.up;

			Vector3 gravityFriction = -Vector3.Project(gravityForce, transform.right);

            Vector3 maxfrictionForce = Vector3.ClampMagnitude(fricValue * 50 * (-sideVelocity.normalized), clampedFrictionForce);
 
            rigid.AddForceAtPosition(maxfrictionForce + gravityFriction, fricAt.position);
		}

        public void brakeLogic()
        {
            float brakeMultiplier = 1f;

            // Viraj sertliği bazlı yavaşlatma
            float turningFactor = Mathf.Clamp01(desiredTurning / 30f); // 30° üstü max fren
            float curveBrakeForce = brakeForce * turningFactor;

            // En yakın araç varsa ona göre de frenle
            if (closestCar != null)
            {
                float distanceRatio = Mathf.Clamp01(closestCarDistance / 35f);
                float targetBrakeMultiplier = 1f - distanceRatio;

                if (closestCarDistance <= 9f)
                {
                    brakeMultiplier = Mathf.Lerp(brakeMultiplier, targetBrakeMultiplier, Time.deltaTime * 25f);
                    if (isBoosting && closestCarDistance < 8f)
                        CancelBoost();
                }
                else
                {
                    brakeMultiplier = Mathf.Lerp(brakeMultiplier, targetBrakeMultiplier, Time.deltaTime * 10f);
                }
            }

            // Toplam fren gücü (viraj + trafik)
            adjustedBrakeForce = curveBrakeForce * brakeMultiplier;

            // Ek güvenlik payı
            if (closestCarDistance <= 9f)
                adjustedBrakeForce *= 1.5f;

            rigid.AddForceAtPosition(-transform.forward * adjustedBrakeForce, groundCheck.position);
        }



        public void ChangeLines(bool isPlayerTriggered = false)
        {
            if (oppositeDirection) return;

            if (laneChangeState != LaneChangeState.Idle && !isPlayerTriggered)
                return;

            if (!HR_LaneManager.Instance)
                return;

            LaneSelectionResult result = HR_LaneManager.Instance.GetAvailableLane(currentLane, transform);
            HR_Lane newLane = result.lane;
            int direction = result.direction;

            if (newLane == currentLane)
            {
                if (isPlayerTriggered && closestCar == null && !isBoosting)
                {
                    BoostSpeed(); // Sadece oyuncu tetiklediyse
                }

                return;
            }

            // Şerit değişimi başlatılıyor (boost zaten yukarıda kontrol edildi)
            pendingLane = newLane;

            changingLines = direction switch
            {
                1 => ChangingLines.Right,
                -1 => ChangingLines.Left,
                _ => ChangingLines.Straight
            };

            signalTimer = 0f;
            laneChangeTimer = 0f;
            isChangingLane = true;
            laneChangeState = LaneChangeState.Signaling;
        }


        private void AutoChangeLine() => ChangeLines(false);


        private void BoostSpeed()
        {
			if (closestCar != null) return; // En yakında araç varsa tetiklenmesin

            if (isBoosting) return; // boost hâlâ aktifse tekrar tetiklenmesin

            isBoosting = true;
            boostStartTime = Time.time;
            boostInitial = 1f;
            boostTarget = 1.2f;
            boostMultiplier = 1f;
        }

		private void CancelBoost()
		{
			isBoosting = false;
			boostMultiplier = 1f;
		}


        private void InitializeVehiclePhysics()
		{
			CreateTriggerVolume();
			rigid = GetComponent<Rigidbody>();
			grounded = false;
			//engineSounds[1].mute = true;
			rigid.centerOfMass = CenterOfMass.localPosition;
			Vector3 centerOfMass_ground_temp = Vector3.zero;
			for (int i = 0; i < TireMeshes.Length; i++)
			{
				centerOfMass_ground_temp += TireMeshes[i].parent.parent.localPosition;

            }
            centerOfMass_ground_temp.y = 0;
			centerOfMass_ground = centerOfMass_ground_temp / 4;


			if (GetComponent<GravityCustom>())
			{
				VehicleGravity = GetComponent<GravityCustom>().gravity;
			}
			else
			{
				VehicleGravity = Physics.gravity.y;
			}
		}

		private void CreateTriggerVolume()
		{

			Bounds bounds = HR_GetBounds.GetBounds(transform);

			// Creating trigger for detecting front vehicles.
			GameObject triggerColliderGO = new GameObject("HR_TriggerVolume");
			triggerColliderGO.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
			triggerColliderGO.transform.position = bounds.center;
			triggerColliderGO.transform.rotation = transform.rotation;
			triggerColliderGO.transform.SetParent(transform, true);
			triggerColliderGO.transform.localScale = transform.localScale;

			BoxCollider boxCollider = triggerColliderGO.AddComponent<BoxCollider>();
			boxCollider.isTrigger = true;
			boxCollider.size = bounds.size;
			boxCollider.center = bounds.center;

			triggerCollider = triggerColliderGO.GetComponent<BoxCollider>();
			triggerCollider.size = new Vector3(bounds.size.x * 0.5f, bounds.size.y, bounds.size.z * triggerSizeZMultiplier);
			triggerCollider.center = new Vector3(bounds.center.x, 0f, bounds.center.z + (triggerCollider.size.z / 2f) - (bounds.size.z) + triggerForwardOffset);

		}




#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Application.isPlaying) return;

            Vector3 carSize = GetComponent<BoxCollider>().size;
            Vector3 rayOrigin = transform.position;
            float rayLength = 3f;
            Vector3 boxHalfExtents = new Vector3(1.3f, 0.5f, carSize.z + 6f);
            Quaternion boxRotation = transform.rotation;

            // Sağ taraf
            Vector3 rightDirection = transform.right;
            Vector3 rightBoxCenter = rayOrigin + rightDirection.normalized * rayLength;
            Gizmos.color = Color.green;
            Gizmos.matrix = Matrix4x4.TRS(rightBoxCenter, boxRotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2);

            // Sol taraf
            Vector3 leftDirection = -transform.right;
            Vector3 leftBoxCenter = rayOrigin + leftDirection.normalized * rayLength;
            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.TRS(leftBoxCenter, boxRotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2);

            // Reset matrix
            Gizmos.matrix = Matrix4x4.identity;
        }

#endif
    }
}
