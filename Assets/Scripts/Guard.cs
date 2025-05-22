using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEditor;
using UnityEngine;
public class Guard : MonoBehaviour
{
	public static event System.Action OnGuardSpottedPlayer;

	
	public float speed = 5;
	public float waitTime = .3f;
	public float turnSpeed = 90;

	public Transform pathHolder;
	public Light spotlight;
	public float viewDistance;
	public LayerMask viewMask;
	private float viewAngle;
	private Transform player;
	private Color spotlightOriginalColor;

	private float playerVisibleTimer = 0f;
	private float timeToSpotPlayer = 0.5f;
	
	

	
	void Start() {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		viewAngle = spotlight.spotAngle;
		spotlightOriginalColor = spotlight.color;
		Vector3[] waypoints = new Vector3[pathHolder.childCount];
		for (int i = 0; i < waypoints.Length; i++) {
			waypoints [i] = pathHolder.GetChild (i).position;
			waypoints [i] = new Vector3 (waypoints [i].x, transform.position.y, waypoints [i].z);
		}
		StartCoroutine (FollowPath (waypoints));
		
	}

	void Update() {
		if (CanSeePlayer()) {
			playerVisibleTimer += Time.deltaTime;
		}
		else
		{
			playerVisibleTimer -= Time.deltaTime;
		}
		playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
		spotlight.color = Color.Lerp(spotlightOriginalColor, Color.red, playerVisibleTimer/timeToSpotPlayer);

		if (playerVisibleTimer >= timeToSpotPlayer)
		{
			if (OnGuardSpottedPlayer != null)
			{
				OnGuardSpottedPlayer();
			}	
		}
	}
	
	bool CanSeePlayer()
	{
		if(player !=null)
			if (Vector3.Distance(transform.position, player.position) < viewDistance)
			{
				Vector3 directionToPlayer = (player.position - transform.position).normalized;
				
				float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);
				if (angleBetweenGuardAndPlayer < viewAngle/2)
				{
					if (!Physics.Linecast(transform.position, player.position, viewMask))
					{
						return true;
					}
				}
			}
		return false;
	}

	IEnumerator FollowPath(Vector3[] waypoints) {
		transform.position = waypoints [0];

		int targetWaypointIndex = 1;
		Vector3 targetWaypoint = waypoints [targetWaypointIndex];
		transform.LookAt (targetWaypoint);

		while (true) {
			transform.position = Vector3.MoveTowards (transform.position, targetWaypoint, speed * Time.deltaTime);
			if (transform.position == targetWaypoint) {
				targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
				targetWaypoint = waypoints [targetWaypointIndex];
				yield return new WaitForSeconds (waitTime);
				yield return StartCoroutine (TurnToFace (targetWaypoint));
			}
			yield return null;
		}
	}

	IEnumerator TurnToFace(Vector3 lookTarget) {
		Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
		float targetAngle = Mathf.Atan2 (dirToLookTarget.x, dirToLookTarget.z ) * Mathf.Rad2Deg;

		while (Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle) > 0.05f || Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle) < -0.05f) {
			float angle = Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
			transform.eulerAngles = Vector3.up * angle;
			yield return null;
		}
	}
  

    
    private void OnDrawGizmos()
    {
        Vector3 startPos = pathHolder.GetChild(0).position;
        Vector3 previousPos = startPos;
        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, 0.3f);
            Gizmos.DrawLine(previousPos, waypoint.position);
            previousPos = waypoint.position;
        }
        Gizmos.DrawLine(previousPos, startPos);
        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }
}
