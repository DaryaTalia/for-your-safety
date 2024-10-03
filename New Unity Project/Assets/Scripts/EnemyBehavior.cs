using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    int health = 3;
    float speed = .4f;

    float distanceSensitivity = 5f;
    float attackSensitivity = 2f;
    float AttackCooldown = 5;
    bool canAttack;

    [SerializeField]
    GameObject KeycardPrefab;

    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        canAttack = true;

        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(health > 0)
        {
            float distFromPlayer = Vector3.Distance(transform.position, GameManager.Instance.Player.gameObject.transform.position);

            if (distFromPlayer >= distanceSensitivity)
            {
                transform.position = Vector3.Lerp(transform.position, GameManager.Instance.Player.gameObject.transform.position, speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, GameManager.Instance.Player.gameObject.transform.position, speed / 3 * Time.deltaTime);

                if (canAttack && distFromPlayer <= attackSensitivity)
                {
                    StartCoroutine(AttackPlayer());
                }
            }
        }        
    }

    void LateUpdate()
    {
        transform.rotation = mainCamera.transform.rotation;
    }

    IEnumerator AttackPlayer()
    {
        GameManager.Instance.TakeDamage();
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

            GetComponentInChildren<Animator>().enabled = false;

            GameObject key = Instantiate(KeycardPrefab, gameObject.transform.position, Quaternion.identity);
            key.transform.position = gameObject.transform.position;

            Destroy(gameObject, 3);
        }
    }
}
