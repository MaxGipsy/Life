using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Slider[] sliders = new Slider[4]; // 0 - blue, 1 - green, 2 - yellow, 3 - red
    public Text[] chances = new Text[4]; // 0 - blue, 1 - green, 2 - yellow, 3 - red
    public InputField inputCount;

    private float valueBlue = 0.25f;
    private float valueGreen = 0.25f;
    private float valueYellow = 0.25f;
    private float valueRed = 0.25f;
    private int maxCount = 0;

    void Start()
    {
        sliders[0].SetValueWithoutNotify(valueBlue);
        sliders[1].SetValueWithoutNotify(valueGreen);
        sliders[2].SetValueWithoutNotify(valueYellow);
        sliders[3].SetValueWithoutNotify(valueRed);

        sliders[0].onValueChanged.AddListener(delegate { ChangeSliders(sliders[0], valueBlue); });
        sliders[1].onValueChanged.AddListener(delegate { ChangeSliders(sliders[1], valueGreen); });
        sliders[2].onValueChanged.AddListener(delegate { ChangeSliders(sliders[2], valueYellow); });
        sliders[3].onValueChanged.AddListener(delegate { ChangeSliders(sliders[3], valueRed); });

        TextChanceUpdate();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Esc");
            Application.Quit();
        }
        TextChanceUpdate();
    }


    public void OnClick()
    {
        if (int.TryParse(inputCount.text, out maxCount))
        {
            if ((maxCount > 0) && (maxCount <= 1000))
            {
                DataManager.Instance.countOfBlue = sliders[0].value;
                DataManager.Instance.countOfGreen = sliders[1].value;
                DataManager.Instance.countOfYellow = sliders[2].value;
                DataManager.Instance.countOfRed = sliders[3].value;
                DataManager.Instance.countOfCreatures = maxCount;
                SceneManager.LoadScene("MainScene");
            }
        }
    }


    public void ChangeSliders(Slider slider, float value)
    {
        float difference = value - slider.value;
        for (int i = 0; i < 4; i++)
        {
            if (sliders[i] != slider)
            {
                sliders[i].SetValueWithoutNotify(sliders[i].value + (difference / 3));
            }
        }

        valueBlue = sliders[0].value;
        valueGreen = sliders[1].value;
        valueYellow = sliders[2].value;
        valueRed = sliders[3].value;
    }


    private void TextChanceUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            chances[i].text = sliders[i].value.ToString("0.00");
        }
    }
}
