using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderChildsByY : MonoBehaviour
{
    // Start is called before the first frame update
    List<Transform> childs = new List<Transform>();

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        childs.Clear();
        foreach (Transform t in transform)
        {
           childs.Add(t);
        }
        var sortedChilds = childs.OrderByDescending(t => t.position.y).ToList();

        for (var i = 0; i < sortedChilds.Count; i++)
        {
	        var c = sortedChilds[i];
	        c.SetSiblingIndex(i);
        }
    }
}
