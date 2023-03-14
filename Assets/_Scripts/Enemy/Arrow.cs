using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;  // Скорость полета стрелы
    public float range = 50f;  // Дальность полета стрелы

    private Vector3 initialPosition;  // Начальная позиция стрелы

    private void Start()
    {
        initialPosition = transform.position;  // Сохраняем начальную позицию стрелы
    }

    private void Update()
    {
        // Перемещаем стрелу вперед на заданную скорость
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Проверяем, не превысила ли стрела заданную дальность полета
        if (Vector3.Distance(initialPosition, transform.position) > range)
        {
            Destroy(gameObject);  // Удаляем стрелу
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);  // Удаляем стрелу при столкновении с другим объектом
    }
}
