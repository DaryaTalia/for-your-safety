using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    int health = 3;
    float speed = .45f;

    float distanceSensitivity = 5f;
    float attackSensitivity = 2f;
    float AttackCooldown = 5;
    bool canAttack;

    [SerializeField]
    GameObject KeycardPrefab;

    Animator anim;

    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        canAttack = true;
        anim = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(health > 0 && health < 300)
        {
            float distFromPlayer = Vector3.Distance(transform.position, GameManager.Instance.Player.gameObject.transform.position);

            if (distFromPlayer >= distanceSensitivity)
            {
                transform.position = Vector3.Lerp(transform.position, GameManager.Instance.Player.gameObject.transform.position, speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, GameManager.Instance.Player.gameObject.transform.position, speed / 2 * Time.deltaTime);

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
        anim.SetTrigger("hurt");
        --health;
        CheckDeath();
    }

    void CheckDeath()
    {
        if(health <= 0 && health < 300)
        {
            health = 301;

            Debug.Log("Enemy is Dead");

            GetComponentInChildren<Animator>().enabled = false;

            GameObject key = Instantiate(KeycardPrefab, gameObject.transform.position, Quaternion.identity);
            key.transform.position = gameObject.transform.position;

            Destroy(gameObject, 3);
        }
    }
}
