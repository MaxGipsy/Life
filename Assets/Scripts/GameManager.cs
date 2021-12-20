using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Структура для хранения списка столкновений
    public struct CollisionPair
    {
        public GameObject creature1;
        public GameObject creature2;
    }
    // Массив префабов существ
    public GameObject[] prefabs = new GameObject[4];

    // GameManager для доступа из других скриптов
    public static GameManager Instance { get; private set; }


    public List<GameObject> creatures = new List<GameObject>(); // Список существ
    public int countOfCreatures = 10; // Количество существ на старте

    // Границы поля
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;

    public List<CollisionPair> collisionPairs = new List<CollisionPair>(); // Список пар столкнувшихся объектов


    private void Start()
    {
        Instance = this;
        // Определение границ экрана
        xMin = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)).x + 0.4f;
        xMax = -xMin;
        yMin = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)).y + 0.4f;
        yMax = -yMin;

        Initialization();
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        // Для каждого существа вызывается метод движения
        for (int i = 0; i < creatures.Count; i++)
        {
            creatures[i].GetComponent<Creature>().Move();
        }
        CheckCollision();
    }


    private void Initialization()
    {
        // Очистка списка столкновений
        collisionPairs.Clear();
        // Заполнение списка существ
        GameObject creatureBuffer;
        creatures.Clear();
        countOfCreatures = DataManager.Instance.countOfCreatures;
        for (int i = 0; i < countOfCreatures; i++)
        {
            // Создание GameObject с выбором "цвета" существа
            creatureBuffer = Instantiate(prefabs[selectColor()],
                new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0), Quaternion.identity);
            creatureBuffer.GetComponent<Creature>().id = i; // Присвоение id
            creatureBuffer.GetComponent<Creature>().countOfSides = Random.Range(2, 7); // Выбор случайного количества сторон (направлений)
            creatureBuffer.GetComponent<Creature>().SetDirection(); // Выбор направления движения
            creatureBuffer.GetComponentInChildren<TextMesh>().text = creatureBuffer.GetComponent<Creature>().countOfSides.ToString();
            creatures.Add(creatureBuffer); // Добавление существа в список
        }


    }


    private void CheckCollision()
    {
        Debug.Log(collisionPairs.Count);

        for (int i = 1; i < collisionPairs.Count; i++)
        {
            if ((collisionPairs[i].creature1 == collisionPairs[0].creature2) &&
                    (collisionPairs[i].creature2 == collisionPairs[0].creature1))
            {
                int sides = collisionPairs[i].creature1.GetComponent<Creature>().countOfSides +
                    collisionPairs[i].creature2.GetComponent<Creature>().countOfSides;
                Vector3 pos = collisionPairs[i].creature1.transform.position;

                creatures.Remove(collisionPairs[i].creature1);
                creatures.Remove(collisionPairs[i].creature2);
                Destroy(collisionPairs[i].creature1);
                Destroy(collisionPairs[i].creature2);
                collisionPairs.RemoveAt(i);
                collisionPairs.RemoveAt(0);

                GameObject creatureBuffer;
                countOfCreatures++;
                creatureBuffer = Instantiate(prefabs[Random.Range(0, 4)], pos, Quaternion.identity);
                creatureBuffer.GetComponent<Creature>().id = countOfCreatures;
                creatureBuffer.GetComponent<Creature>().countOfSides = sides;
                creatureBuffer.GetComponentInChildren<TextMesh>().text = sides.ToString();

                creatureBuffer.GetComponent<Creature>().SetDirection();
                creatures.Add(creatureBuffer);

                break;
            }
        }
        collisionPairs.Clear();
    }


    private int selectColor()
    {
        float rnd = Random.value;
        if (rnd <= DataManager.Instance.countOfBlue)
        {
            return 0;
        }
        else if (rnd <= DataManager.Instance.countOfBlue + DataManager.Instance.countOfGreen)
        {
            return 1;
        }
        else if (rnd <= DataManager.Instance.countOfBlue + DataManager.Instance.countOfGreen +
            DataManager.Instance.countOfYellow)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }
}
