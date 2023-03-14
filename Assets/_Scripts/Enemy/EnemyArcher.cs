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
        // ������������ ���������� �� ������
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        if (distanceToPlayer <= chaseRadius)
        {
            // ����� � ���� ������������� �������

            // ���������� ������� ������
            lastKnownPlayerPosition = playerTransform.position;
            losePlayerTimer = losePlayerTime;

            if (distanceToPlayer <= shootRadius)
            {
                // ����� � ���� �������� �������

                if (!isAttacking)
                {
                    // �������� �������� �����
                    animator.SetBool("isAttacking", true);
                    isAttacking = true;
                }
            }
            else
            {
                // ����� � ���� ������������� �������, �� ��� ���� ��������

                if (!isChasing)
                {
                    // �������� �������� ����
                    animator.SetBool("isRunning", true);
                    isChasing = true;
                }

                // ���������� ������� � ������� ������
                Vector3 directionToPlayer = lastKnownPlayerPosition - transform.position;
                transform.Translate(directionToPlayer.normalized * moveSpeed * Time.deltaTime, Space.World);
                transform.LookAt(lastKnownPlayerPosition);
            }
        }
        else
        {
            // ����� ��� ���� ������������� �������

            if (isChasing)
            {
                // ������������� �������� ����
                animator.SetBool("isRunning", false);
                isChasing = false;
            }

            if (losePlayerTimer > 0f)
            {
                // ����� ��� ������� �������, � ������ ��� �� ����� � ���

                // ���������� ������� � ��������� ��������� ������� ������
                Vector3 directionToLastKnownPlayerPosition = lastKnownPlayerPosition - transform.position;
                transform.Translate(directionToLastKnownPlayerPosition.normalized * moveSpeed * Time.deltaTime, Space.World);
                transform.LookAt(lastKnownPlayerPosition);

                // ��������� ������ ��������� ������
                losePlayerTimer -= Time.deltaTime;
            }
            else
            {
                // ����� ��� ����� ��������

                if (isAttacking)
                {
                    // ������������� �������� �����
                    animator.SetBool("isAttacking", false);
                    isAttacking = false;
                }
            }
        }
    }

    private void ShootArrow()
    {
        // ������� ������
        GameObject arrowObject = Instantiate(arrowPrefab, arrowSpawnTransform.position, arrowSpawnTransform.rotation);
        Arrow arrow = arrowObject.GetComponent<Arrow>();

        // ����������� ��������� ������
        arrow.speed = arrowSpeed;
        arrow.range = arrowRange;

        // ��������� ������ � ������� ������
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        arrowObject.GetComponent<Rigidbody>().AddForce(directionToPlayer.normalized * arrowSpeed, ForceMode.Impulse);
    }
}