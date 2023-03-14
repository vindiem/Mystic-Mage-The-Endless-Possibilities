using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnTransform;
    public Transform playerTransform;
    public float chaseRadius = 10f;
    public float shootRadius = 5f;
    public float moveSpeed = 3f;
    public float arrowSpeed = 20f;
    public float arrowRange = 100f;
    public float losePlayerTime = 5f;

    private Animator animator;
    private bool isAttacking = false;
    private bool isChasing = false;
    private Vector3 lastKnownPlayerPosition;
    private float losePlayerTimer = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Рассчитываем расстояние до игрока
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        if (distanceToPlayer <= chaseRadius)
        {
            // Игрок в зоне преследования лучника

            // Запоминаем позицию игрока
            lastKnownPlayerPosition = playerTransform.position;
            losePlayerTimer = losePlayerTime;

            if (distanceToPlayer <= shootRadius)
            {
                // Игрок в зоне выстрела лучника

                if (!isAttacking)
                {
                    // Начинаем анимацию атаки
                    animator.SetBool("isAttacking", true);
                    isAttacking = true;
                }
            }
            else
            {
                // Игрок в зоне преследования лучника, но вне зоны выстрела

                if (!isChasing)
                {
                    // Начинаем анимацию бега
                    animator.SetBool("isRunning", true);
                    isChasing = true;
                }

                // Перемещаем лучника в сторону игрока
                Vector3 directionToPlayer = lastKnownPlayerPosition - transform.position;
                transform.Translate(directionToPlayer.normalized * moveSpeed * Time.deltaTime, Space.World);
                transform.LookAt(lastKnownPlayerPosition);
            }
        }
        else
        {
            // Игрок вне зоны преследования лучника

            if (isChasing)
            {
                // Останавливаем анимацию бега
                animator.SetBool("isRunning", false);
                isChasing = false;
            }

            if (losePlayerTimer > 0f)
            {
                // Игрок был замечен недавно, и лучник еще не забыл о нем

                // Перемещаем лучника в последнюю известную позицию игрока
                Vector3 directionToLastKnownPlayerPosition = lastKnownPlayerPosition - transform.position;
                transform.Translate(directionToLastKnownPlayerPosition.normalized * moveSpeed * Time.deltaTime, Space.World);
                transform.LookAt(lastKnownPlayerPosition);

                // Уменьшаем таймер забывания игрока
                losePlayerTimer -= Time.deltaTime;
            }
            else
            {
                // Игрок был забыт лучником

                if (isAttacking)
                {
                    // Останавливаем анимацию атаки
                    animator.SetBool("isAttacking", false);
                    isAttacking = false;
                }
            }
        }
    }

    private void ShootArrow()
    {
        // Создаем стрелу
        GameObject arrowObject = Instantiate(arrowPrefab, arrowSpawnTransform.position, arrowSpawnTransform.rotation);
        Arrow arrow = arrowObject.GetComponent<Arrow>();

        // Настраиваем параметры стрелы
        arrow.speed = arrowSpeed;
        arrow.range = arrowRange;

        // Запускаем стрелу в сторону игрока
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        arrowObject.GetComponent<Rigidbody>().AddForce(directionToPlayer.normalized * arrowSpeed, ForceMode.Impulse);
    }
}