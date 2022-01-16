using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public string[] topics;
    public string topic;
    public GameObject[] states;
    public float[] dataList;
    public APIHelper apiHelper;
    public float scale;
    public string publicKey;
    public bool percentage;
    private string oldKey;
    public Text buttonText;

    private void Start()
    {
        // Choose a topic
        topic = topics[Random.Range(0, topics.Length)];
    }

    public void GetData()
    {
        int offset = 1;
        for (int i = 0; i < 50; i++)
        {
            if (i + offset == 3 || i + offset == 7 || i + offset == 11 || i + offset == 14 || i + offset == 43 || i + offset == 52)
            {
                offset++;
            }
            oldKey = publicKey;
            dataList[i] = apiHelper.GetData("\"" + publicKey + "\"", i + offset, percentage);
        }
    }

    public void DisplayData()
    {
        if (oldKey != publicKey)
        {
            buttonText.text = "Loading ...";
            GetData();
            buttonText.text = "Generate Map";
        }

        // Find highest value
        float highestVal = 0;
        foreach (float value in dataList)
        {
            if (value > highestVal)
            {
                highestVal = value;
            }
        }
        float lowestVal = highestVal;
        foreach (float value in dataList)
        {
            if (value < highestVal)
            {
                lowestVal = value;
            }
        }
        for (int i = 0; i < 50; i++)
        {
            if (dataList[i] == -1)
            {
                // No result
                states[i].GetComponent<Image>().color = Color.grey;
            }
            else if (dataList[i] >= ((highestVal - lowestVal) * 5 / 6 * scale))
            {
                states[i].GetComponent<Image>().color = new Color(0.5f, 0, 0);
            }
            else if (dataList[i] < ((highestVal - lowestVal) * 5 / 6 * scale) && dataList[i] >= ((highestVal - lowestVal) * 4 / 6 * scale))
            {
                states[i].GetComponent<Image>().color = Color.red;
            }
            else if (dataList[i] < ((highestVal - lowestVal) * 4 / 6 * scale) && dataList[i] >= ((highestVal - lowestVal) * 3 / 6 * scale))
            {
                states[i].GetComponent<Image>().color = new Color(1, 0.5f, 0);
            }
            else if (dataList[i] < ((highestVal - lowestVal) * 3 / 6 * scale) && dataList[i] >= ((highestVal - lowestVal) * 2 / 6 * scale))
            {
                states[i].GetComponent<Image>().color = Color.yellow;
            }
            else if (dataList[i] < ((highestVal - lowestVal) * 2 / 6 * scale) && dataList[i] >= ((highestVal - lowestVal) * 1 / 6 * scale))
            {
                states[i].GetComponent<Image>().color = new Color(0.5f, 1, 0);
            }
            else if (dataList[i] < ((highestVal - lowestVal) * 1 / 6 * scale) && dataList[i] >= lowestVal)
            {
                states[i].GetComponent<Image>().color = Color.green;
            }
            else if (dataList[i] < 0.1f && dataList[i] >= 0)
            {
                states[i].GetComponent<Image>().color = Color.white;
            }
        }

        /*for (int i = 0; i < 50; i++)
        {
            dataList[i] = Random.Range(0, 100);
        }*/
    }
}
