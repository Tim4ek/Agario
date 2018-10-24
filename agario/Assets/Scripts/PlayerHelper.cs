using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class PlayerHelper : NetworkBehaviour
{
    [SyncVar]
    public float Speed = 5;

    [SyncVar]
    public float Size = 0.2f;

    [SyncVar]
    public Color Color = Color.blue;

    public bool IsPart { get; set; }

    GameHelper _gameHelper;
    // Use this for initialization
    void Start()
    {
        _gameHelper = GameObject.FindObjectOfType<GameHelper>();


        if (!isLocalPlayer)
            return;

        _gameHelper.CurrentPlayer = this;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(Size, Size, Size);
        GetComponent<SpriteRenderer>().color = Color;
        if (!isLocalPlayer)
            return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = Vector3.MoveTowards(transform.position, mousePos,
            Time.deltaTime * Speed);

        CheckBounds();
    }

    private void CheckBounds()
    {
        if (transform.position.x >= _gameHelper.MapSize.x)
            transform.position = new Vector3(_gameHelper.MapSize.x - 0.01f,
                transform.position.y, 0);

        if (transform.position.y >= _gameHelper.MapSize.y)
            transform.position = new Vector3(transform.position.x,
               _gameHelper.MapSize.y - 0.01f, 0);
        
        /// Низ
        if (transform.position.x <= -_gameHelper.MapSize.x)
            transform.position = new Vector3(-_gameHelper.MapSize.x + 0.01f,
                transform.position.y, 0);

        if (transform.position.y <= -_gameHelper.MapSize.y)
            transform.position = new Vector3(transform.position.x,
               -_gameHelper.MapSize.y + 0.01f, 0);
    }

    [Server]
    public void ChangeSize(float size)
    {
        Size = size;
        Speed = 1 / Size;
        transform.localScale = new Vector3(Size, Size, Size);
    }

    [ServerCallback]
    void OnTriggerStay2D(Collider2D other)
    {
        Bounds enemy = other.bounds;
        Bounds current = GetComponent<Collider2D>().bounds;

        Vector2 centerEnemy = enemy.center;
        Vector2 centerCurrent = current.center;

        if (current.size.x > enemy.size.x &&
           Vector3.Distance(centerCurrent, centerEnemy) < current.size.x)
        {
            if (other.GetComponent<PointHelper>())
            {
                ChangeSize(Size + 0.05f);
                _gameHelper.CreatePoint(Color.red);
            }
            else
                ChangeSize(Size + other.transform.localScale.x);

            NetworkServer.Destroy(other.gameObject);
        }
    }
}
