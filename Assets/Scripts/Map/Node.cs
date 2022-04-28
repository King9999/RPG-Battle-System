using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* A node is basically a room that consists of 4 paths. The player travels along these paths to reach other nodes.
    when a node is generated, the paths and any objects they contain are random. Exactly one node will always be a starting point,
    and one other node must be an exit to a new area. All other nodes can have various objects, or none at all.

    Nodes can have events which are not known until a player steps into the node with the event. Events are mostly positive,
    but they can be dangerous too.
*/
public class Node : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite eventSprite;
    public enum EventNode {None, HpMpBoost}
    public EventNode eventNode;

    public Path[] paths;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
