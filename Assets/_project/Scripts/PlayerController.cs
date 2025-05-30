using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Rigidbody2D _rb;
    private Vector2 _dir;
    

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        _dir = new Vector2(h, v);

        if (_dir.sqrMagnitude > 1)
        {
            _dir.Normalize();
        }
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _dir * (_speed * Time.deltaTime));
    }
}
