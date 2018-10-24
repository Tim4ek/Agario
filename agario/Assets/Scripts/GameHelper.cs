using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameHelper : NetworkBehaviour
{
    public Vector2 MapSize = new Vector2(20, 20);

    private PlayerHelper _playerHelper;

    public PlayerHelper CurrentPlayer
    {
        get { return _playerHelper; }
        set { _playerHelper = value; }
    }


    [Server]
    // Use this for initialization
    IEnumerator Start()
    {
        Debug.Log("GameHelper Start ()");
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 10; i++)
        {
            CreatePoint(Color.green);
        }
    }

    [Server]
    public void CreatePoint(Color color)
    {
        GameObject point = Instantiate<GameObject>(Resources.Load<GameObject>("Point"));
        Vector2 rand = Random.insideUnitCircle;
        point.transform.position = rand * Random.Range(5, MapSize.x);
        NetworkServer.Spawn(point);
        point.GetComponent<PointHelper>().Color = color;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
