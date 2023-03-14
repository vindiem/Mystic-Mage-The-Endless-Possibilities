using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;  // �������� ������ ������
    public float range = 50f;  // ��������� ������ ������

    private Vector3 initialPosition;  // ��������� ������� ������

    private void Start()
    {
        initialPosition = transform.position;  // ��������� ��������� ������� ������
    }

    private void Update()
    {
        // ���������� ������ ������ �� �������� ��������
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // ���������, �� ��������� �� ������ �������� ��������� ������
        if (Vector3.Distance(initialPosition, transform.position) > range)
        {
            Destroy(gameObject);  // ������� ������
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);  // ������� ������ ��� ������������ � ������ ��������
    }
}
