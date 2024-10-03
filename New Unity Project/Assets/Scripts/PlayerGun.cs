using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGun : MonoBehaviour 
{
    [SerializeField]
    float cooldown = 3f;
    [SerializeField]
    float timer = 1;
    float range = 30f;

    [SerializeField]
    LayerMask enemyMask;

    [SerializeField]
    LayerMask windowMask;

    void FixedUpdate()
    {
        if(GameManager.Instance.gunFound)
        {
            GameManager.Instance.uiManager.UpdateGunCooldownSlider(cooldown / timer);

            if (timer < cooldown)
            {
                timer += .9f * Time.deltaTime;
            }
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

        Debug.Log("Shoot");
        Debug.DrawRay(GameManager.Instance.GunshotPoint.position,
            Camera.main.transform.forward * 10, 
            Color.blue, 15f);

        timer = 1;

        if (GameManager.Instance.PlayerArm.activeSelf)
        {
            GameManager.Instance.PlayerArm.GetComponentInChildren<Animator>().SetTrigger("shoot");
        }

        ShootEnemy();
        ShootWindow();
    }

    void ShootEnemy()
    {
        // Shoot Enemy
        bool hit = Physics.Raycast(GameManager.Instance.GunshotPoint.position,
            Camera.main.transform.forward,
            out RaycastHit result, range, enemyMask);

        if (hit)
        {
            Debug.Log("Shot Enemy");
            result.collider.gameObject.GetComponent<EnemyBehavior>().TakeDamage();
        }
    }

    void ShootWindow()
    {
        // Shoot Window
        bool hit = Physics.Raycast(GameManager.Instance.GunshotPoint.position,
            Camera.main.transform.forward,
            out _, range, windowMask);

        if (hit)
        {
            Debug.Log("Shot Window");
            GameManager.Instance.WindowShot = true;
        }
    }

}
