using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    [SerializeField] private float _damage = 25.0f;
    [SerializeField] private float _playerHealth = 100;
    public GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Player health: " + _playerHealth);
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");  
       
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            _playerHealth -= _damage;
            Debug.Log("Player Health : " + _playerHealth);
            if (_playerHealth < 25f)
            {
                Debug.Log("Player Dead");

            }
        }
    }
}
