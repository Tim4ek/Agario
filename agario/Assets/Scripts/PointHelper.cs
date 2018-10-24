using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PointHelper : NetworkBehaviour
{
    [SyncVar]
    public Color Color;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().color = Color;
    }
}
