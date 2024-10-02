using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    int health = 3;
    [SerializeField]
    float speed = .05f;

    Vector3 target;
    float distanceSensitivity = 4f;
    float attackSensitivity = 1f;
    float AttackCooldown = 3;
    bool canAttack;

    [SerializeField]
    GameObject KeycardPrefab;

    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        target = GameManager.Instance.Player.gameObject.transform.position;
        canAttack = true;

        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        float distFromPlayer = Vector3.Distance(transform.position, target);

        if (distFromPlayer >= distanceSensitivity)
        {
            transform.position = Vector3.Lerp(transform.position, target,speed * Time.deltaTime);
        }

        if (distFromPlayer <= attackSensitivity && canAttack)
        {
            AttackPlayer();
        }
    }

    void LateUpdate()
    {
        transform.rotation = mainCamera.transform.rotation;
    }

    IEnumerator AttackPlayer()
    {
        --GameManager.Instance.PlayerHealth;
        canAttack = false;

        yield return new WaitForSeconds(AttackCooldown);
        canAttack = true;
    }

    public void TakeDamage()
    {
        --health;
        CheckDeath();
    }

    void CheckDeath()
    {
        if(health <= 0)
        {
            Debug.Log("Enemy is Dead");

            if(gameObject.CompareTag("Enemy"))
            {
                Instantiate(KeycardPrefab, gameObject.transform);
            }

            Destroy(gameObject, 3);
        }
    }
}
