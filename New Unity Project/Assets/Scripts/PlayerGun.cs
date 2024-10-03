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
    float range = 30f;

    [SerializeField]
    LayerMask enemyMask;

    [SerializeField]
    LayerMask windowMask;

    void FixedUpdate()
    {
        if(GameManager.Instance.gunFound && timer > 0)
        {
            timer -= 1 * Time.deltaTime;

            GameManager.Instance.uiManager.UpdateGunCooldownSlider(timer / cooldown);
        }
    }

    public float Cooldown
    {
        get => cooldown;
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
        Debug.DrawRay(GameManager.Instance.GunshotPoint.position,
            Camera.main.transform.forward * 10, 
            Color.blue, 15f);

        RaycastHit result;

        // Shoot Enemy
        bool hit = Physics.Raycast(GameManager.Instance.GunshotPoint.position,
            Camera.main.transform.forward, 
            out result, range, enemyMask);

        if(GameManager.Instance.PlayerArm.activeSelf)
        {
            GameManager.Instance.PlayerArm.GetComponentInChildren<Animator>().SetTrigger("shoot");
        }        
        
        if (hit)
        {
            Debug.Log("Shot Enemy");
            result.collider.gameObject.GetComponent<EnemyBehavior>().TakeDamage();
        }

        // Shoot Window
        hit = Physics.Raycast(GameManager.Instance.GunshotPoint.position,
            Camera.main.transform.forward, 
            out result, range, windowMask);

        if(GameManager.Instance.PlayerArm.activeSelf)
        {
            GameManager.Instance.PlayerArm.GetComponentInChildren<Animator>().SetTrigger("shoot");
        }        
        
        if (hit)
        {
            Debug.Log("Shot Window");
            GameManager.Instance.EnterState(GameManager.GameStates.ENGINE_ROOM_WINDOW_SHOT);
        }
        
    }
}
