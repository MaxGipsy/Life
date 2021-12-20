using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    // Цвета фигур (повдение)
    public enum colors
    {
        colorRed, // Красный - охотится
        colorBlue, // Синий - стоит на месте
        colorGreen, // Зеленый - двигается, меняет направление при касании края
        colorYellow // Желтый - двигается, меняет направление по времени
    }

    public colors color; // Цвет фигуры
    public int countOfSides; // Количество сторон (возможных направлений)
    public float direction; // Направление движения фигуры
    public int id;

    private const float speed = 5f; // Скорость движения фигур
    private float time = 0f; // Время без смены направления
    public GameObject target = null; // Цель для "красного" жителя


    // Установка направления движения существа
    public void SetDirection()
    {
        int rnd = Random.Range(0, countOfSides);
        direction = 360f / countOfSides * rnd;
        //transform.rotation = Quaternion.Euler(0, 0, direction);
    }

    
    // Движение существ
    public void Move()
    {
        switch (color)
        {
            case (colors.colorBlue):
                {
                    break;
                }
            case (colors.colorGreen):
                {
                    MoveGreen();
                    break;
                }
            case (colors.colorYellow):
                {
                    MoveYellow();
                    break;
                }
            case (colors.colorRed):
                {
                    MoveRed();
                    break;
                }
            default:
                {
                    break;
                }
        }
    }


    // Движение "зеленых" существ
    private void MoveGreen()
    {
        // Находится ли существо в пределах экрана
        if (((transform.position.x >= GameManager.Instance.xMin) && (transform.position.x <= GameManager.Instance.xMax))
            && ((transform.position.y >= GameManager.Instance.yMin) && (transform.position.y <= GameManager.Instance.yMax)))
        {
            Vector3 angle = new Vector3(Mathf.Sin(direction), Mathf.Cos(direction), 0);
            //transform.position += transform.up * speed * Time.deltaTime;
            transform.position += angle * speed * Time.deltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, angle,
            //    speed * Time.deltaTime);
        }
        else // Если существо вышло за пределы экрана - вернуть его в экран и сменить направление
        {
            if (transform.position.x < GameManager.Instance.xMin)
            {
                transform.position = new Vector3(GameManager.Instance.xMin, transform.position.y, 0);
            }
            else if (transform.position.x > GameManager.Instance.xMax)
            {
                transform.position = new Vector3(GameManager.Instance.xMax, transform.position.y, 0);
            }
            if (transform.position.y < GameManager.Instance.yMin)
            {
                transform.position = new Vector3(transform.position.x, GameManager.Instance.yMin, 0);
            }
            else if (transform.position.y > GameManager.Instance.yMax)
            {
                transform.position = new Vector3(transform.position.x, GameManager.Instance.yMax, 0);
            }
            SetDirection();
        }
    }


    // Движение "желтых" существ
    private void MoveYellow()
    {
        time += Time.deltaTime; // Увеличение времени без смены направления
        if(time < 1) // Если время не превышено, идти в выбранном направлении
        {
            MoveGreen();
        }
        else // Иначе сбросить таймер и сменить направление
        {
            time = 0;
            SetDirection();
        }
    }


    // Движение "красных" существ
    private void MoveRed()
    {
        // Если цель живая, преследовать
        if (target != null)
        {
            Vector3 dir = target.transform.position - this.transform.position;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg, Vector3.forward);
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position,
                speed * Time.deltaTime);
        }
        else if (GameManager.Instance.creatures.Count > 1)// Иначе выбрать новую цель
        {
            // Выбор цели
            target = GameManager.Instance.creatures[Random.Range(0, GameManager.Instance.creatures.Count)];
            // Если выбрал самого себя - выбрать новую цель
            while (target.GetComponent<Creature>().id == this.id)
            {
                target = GameManager.Instance.creatures[Random.Range(0, GameManager.Instance.creatures.Count)];
            }
        }
        else
        {
            return;
        }
    }


    // Действие при столкновении
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.CollisionPair pair;
        pair.creature1 = this.gameObject;
        pair.creature2 = collision.gameObject;
        GameManager.Instance.collisionPairs.Add(pair);
    }

}
