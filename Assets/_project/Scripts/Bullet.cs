using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private Rigidbody2D _rb;
    public Vector2 Dir {  get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.velocity = Dir * _speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
