using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

/// <summary>
/// Some kind of astar but without updating newer shorter found paths
/// </summary>
public class Navigation : MonoBehaviour
{
	Dictionary<Vector3Int, Node> nodes = new Dictionary<Vector3Int, Node>();

	public class Node
	{
		public Vector3Int pos;
		public Node nearestToStart;
		public bool visited;
		public float g = -1;
		public float h = -1;
	}

	public Transform fromTransform;
	public Transform toTransform;

	EdgeHeap<Node> open = new EdgeHeap<Node>();

	Node start;
	Node end;

	List<Node> tempNeighbours = new List<Node>();

	TileGrid tileGrid;

	[ContextMenu("Do nav")]
	void DoNavigation()
	{
		GetPath(fromTransform.position, toTransform.position);
	}

	public List<Vector3> GetPath(Vector3 from, Vector3 to)
	{
		tileGrid = FindObjectOfType<TileGrid>();
		open.Clear();
		nodes.Clear();

		var beginCell = tileGrid.walkableTileMap.WorldToCell(from);
		var endCell = tileGrid.walkableTileMap.WorldToCell(to);

		if(tileGrid.walkableTileMap.GetTile(beginCell) == null)
		{
			// Sometimes the from tile is null, so look for neighbours and do it hacky
			List<Node> results = new List<Node>();
			GetNeighbours(beginCell, ref results);
			bool found = false;
			foreach (var result in results)
			{
				if(result != null)
				{
					beginCell = result.pos;
					found = true;
				}
			}
			if(!found)
			{
				Debug.LogWarning("Begin is not on tile and couldn't find neighbor that is! " + from);
				return null;
			}
		}
		if(tileGrid.walkableTileMap.GetTile(endCell) == null)
		{
			Debug.LogWarning("End is not on tile: " + to);
			return null;
		}

		start = GetNode(beginCell);
		end = GetNode(endCell);

		if(start == null || end == null) return null;

		AstarSearch(start);

		var path = new List<Vector3>();
		path.Add(tileGrid.walkableTileMap.CellToWorld(end.pos));
		BuildShortestPath(path, end);
		path.RemoveAt(path.Count - 1); // remove first cell
		path.Reverse();
		path.Add(to);
		return path;
	}

	void BuildShortestPath(List<Vector3> list, Node node)
	{
		int maxLoops = 10000;
		while (--maxLoops > 0)
		{
			if (node.nearestToStart == null) return;

			list.Add(tileGrid.walkableTileMap.CellToWorld(node.nearestToStart.pos));
			node = node.nearestToStart;
		}
		if(maxLoops <= 0) Debug.LogError("Hit max in building the shortest path in A*");
	}

	void AstarSearch(Node start)
	{
		int maxCount = 1000;
		int nodeVisits = 0;

		start.g = 0;
		start.h = GetH(start);
		open.push(start);
		do
		{
			var node = open.pop();
			node.visited = true;

			nodeVisits++;

			GetNeighbours(node.pos, ref tempNeighbours);
			foreach (var neighbour in tempNeighbours)
			{
				if(neighbour == null) continue;

				var cost = node.g +  Vector3.Distance(node.pos, neighbour.pos);

				if ((!neighbour.visited && neighbour.g == -1))
				{
					neighbour.g = cost;
					neighbour.nearestToStart = node;

					if(neighbour.h == -1) neighbour.h = GetH(neighbour);
					open.push(neighbour);
				}
			}

			if (node == end)
			{
				return;
			}
		}
		while (open.Count != 0 && --maxCount > 0);

		if(maxCount <= 0) Debug.LogWarning("Hit max in A* search");
	}

	float GetH(Node node)
	{
		const float hCostHeuristic = 1.3f;
		return Vector3.Distance(node.pos, end.pos) * hCostHeuristic;
	}


	void GetNeighbours(Vector3Int pos, ref List<Node> result)
	{
		result.Clear();

		result.Add(GetNode(pos + Vector3Int.right));
		result.Add(GetNode(pos + Vector3Int.down));
		result.Add(GetNode(pos + Vector3Int.up));
		result.Add(GetNode(pos + Vector3Int.left));
	}

	Node GetNode(Vector3Int pos)
	{
		if(tileGrid.walkableTileMap.GetTile(pos) == null) return null;
		if(tileGrid.nonWalkableTileMap.GetTile(pos) != null) return null;

		Node node;
		if(!nodes.TryGetValue(pos, out node))
		{
			node = new Node()
			{
				pos = pos
			};
			nodes[pos] = node;
		}
		return node;
	}
}

/// <summary>
/// Adapted from https://stackoverflow.com/a/33888482/1405654
/// </summary>
public class EdgeHeap<T> where T : Navigation.Node
{
	T[] heap;
	public int Count { get; private set; }
	public EdgeHeap() : this(null) { }
	public EdgeHeap(IComparer<T> comparer) : this(16, comparer) { }
	public EdgeHeap(int capacity, IComparer<T> comparer)
	{
		this.heap = new T[capacity];
	}
	public void push(T v)
	{
		if (Count >= heap.Length) Array.Resize(ref heap, Count * 2);
		heap[Count] = v;
		SiftUp(Count++);
	}
	public T pop()
	{
		var v = top();
		heap[0] = heap[--Count];
		if (Count > 0) SiftDown(0);
		return v;
	}
	public T top()
	{
		if (Count > 0) return heap[0];
		return null;
	}
	public void Clear()
	{
		for (var i = 0; i < heap.Length; i++) heap[i] = null;
		Count = 0;
	}
	void SiftUp(int n)
	{
		var v = heap[n];
		for (var n2 = n / 2; n > 0 && CompareCost(v, heap[n2]) > 0; n = n2, n2 /= 2) heap[n] = heap[n2];
		heap[n] = v;
	}
	void SiftDown(int n)
	{
		var v = heap[n];
		for (var n2 = n * 2; n2 < Count; n = n2, n2 *= 2)
		{
			if (n2 + 1 < Count && CompareCost(heap[n2 + 1], heap[n2]) > 0) n2++;
			if (CompareCost(v, heap[n2]) >= 0) break;
			heap[n] = heap[n2];
		}
		heap[n] = v;
	}

	static int CompareCost(T a, T b)
	{
		var fCostA = a.g + a.h;
		var fCostB = b.g + b.h;
		if(fCostA < fCostB) return 1;
		if(fCostA > fCostB) return -1;
		return 0;
	}
}