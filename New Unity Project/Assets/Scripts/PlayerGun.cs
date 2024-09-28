using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGun : MonoBehaviour 
{
    [SerializeField]
    float cooldown = 3f;
    [SerializeField]
    float timer = 0;
    float range = 20f;

    [SerializeField]
    LayerMask enemyMask;

    void FixedUpdate()
    {
        if(GameManager.Instance.gunFound && timer > 0)
        {
            timer -= 1 * Time.deltaTime;
        }
    }

    // Left Mouse Click to call 
    public void OnShoot()
    {
        if(!GameManager.Instance.gunFound)
        {
            Debug.Log("Gun not acquired.");
            return;
        }

        if(timer > 0)
        {
            Debug.Log("Timer = " + timer);
            return;
        }

        Debug.Log("Shoot");
        timer = cooldown;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, Color.blue, 15f);

        RaycastHit result;

        bool hit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out result, range, enemyMask);
        
        if (hit)
        {
            Debug.Log("Shot Enemy");
            result.collider.gameObject.GetComponent<EnemyBehavior>().TakeDamage();
        }
        
    }
}
