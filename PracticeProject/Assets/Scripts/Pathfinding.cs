using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
	public Transform seeker;
	public Transform target;


	Grid grid;

	void Awake ()
	{
		grid = GetComponent<Grid> ();
	}

	void Update(){
		FindPath (seeker.position, target.position);
	}

	void FindPath (Vector3 startPos, Vector3 targetPos)
	{

		Node startNode = grid.GetNodeFromWorldPoint (startPos);
		Node targetNode = grid.GetNodeFromWorldPoint (targetPos);


		List<Node> openSet = new List<Node> ();
		HashSet<Node> closedSet = new HashSet<Node> ();

		openSet.Add (startNode);
		while (openSet.Count > 0) {
			Node currentNode = openSet [0];
			for (int i = 1; i < openSet.Count; i++) {
				if (openSet [i].getfCost () < currentNode.getfCost () || openSet [i].getfCost () == currentNode.getfCost () && openSet [i].hCost < currentNode.hCost) {
					currentNode = openSet [i];
				}
			}
			openSet.Remove (currentNode);
			closedSet.Add (currentNode);

			if (currentNode == targetNode) {
				RetracePath(startNode, targetNode);
				return;
			}

			foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
				if ((!(neighbour.walkable)) || closedSet.Contains (neighbour)) {
					continue;
				}

				int newMovementCostToNeighbour = currentNode.gCost + GetDistanceNode(currentNode,neighbour);

				if(newMovementCostToNeighbour < neighbour.gCost || !(openSet.Contains(neighbour))){

					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDistanceNode (neighbour,targetNode);
					neighbour.parent = currentNode;

					if(!(openSet.Contains (neighbour))){
						openSet.Add(neighbour);
					}
				}
			}

				  

		}
	}

	void RetracePath(Node startNode, Node endNode){
		List<Node> path = new List<Node> ();

		Node currentNode = endNode;
		while (currentNode != startNode) {
			path.Add (currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse ();
		grid.path = path;

	}

	int GetDistanceNode (Node a, Node b){
		int dstX = Mathf.Abs (a.gridX - b.gridX);
		int dstY = Mathf.Abs (a.gridY - b.gridY);

		if (dstX > dstY) {
			return 14*dstY + 10* (dstX-dstY);
		}
			return 14*dstX + 10* (dstY-dstX);
	}
}

