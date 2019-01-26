using System.Collections.Generic;
using UnityEngine;

public class NavigationAgent : MonoBehaviour
{
	public Transform to;

	public float speed = 2;

	List<Vector3> path;
	int pathIndex;

	[ContextMenu("Move")]
	public void Move()
	{
		path = FindObjectOfType<Navigation>().GetPath(this.transform.position, to.position);
	}

	void Update()
	{
		if(path != null && path.Count > 0)
		{
			var pathNodeDestination = path[pathIndex];

			var dist = Vector3.Distance(transform.position, pathNodeDestination);
			if(dist > .1f)
			{
				transform.position = Vector3.MoveTowards(transform.position, path[pathIndex], Time.deltaTime * 2);
			}
			else
			{
				pathIndex++;
				if(pathIndex >= path.Count)
				{
					// Reached destination
					path = null;
					pathIndex = 0;
				}
			}
		}
	}
}