using UnityEngine;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class APIHelper : MonoBehaviour
{
    public List<string> lines = new List<string>();
    public string textResult;
    public float total;

    public float GetData(string key, int state, bool percentage)
    {
        // Extract the whole data
        string link;
        if (state < 10)
        {
            link = "https://api.census.gov/data/2016/acs/acs1/spp?get=S0201_0006E,POPGROUP,POPGROUP_TTL,NAME&for=state:0" + state.ToString();
        }
        else
        {
            link = "https://api.census.gov/data/2016/acs/acs1/spp?get=S0201_0006E,POPGROUP,POPGROUP_TTL,NAME&for=state:" + state.ToString();
        }
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        textResult = reader.ReadToEnd();

        // Check if key exists
        if (!textResult.Contains(key))
        {
            return -1;
        }

        if (percentage)
        {
            total = ExtractData("\"001\"");
        }

        // Extract each line
        bool readingLine = false;
        string line = "";
        for (int i = 0; i < textResult.Length; i++)
        {
            if (textResult.Substring(i, 1) == "[")
            {
                readingLine = true;
            }
            else if (textResult.Substring(i, 1) == "]")
            {
                line += textResult.Substring(i, 1);
                lines.Add(line);
                line = "";
                readingLine = false;
            }

            if (readingLine)
            {
                line += textResult.Substring(i, 1);
            }
        }

        // Find the line with the key
        int lineIndex = 0;
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].Contains(key))
            {
                lineIndex = i;
                break;
            }
        }

        // Build the value in the line
        string value = "";
        bool buildValue = false;
        for (int i = 0; i < lines[lineIndex].Length; i++)
        {
            if (buildValue && lines[lineIndex].Substring(i, 1) != "\"")
            {
                value += lines[lineIndex].Substring(i, 1);
            }

            if (lines[lineIndex].Substring(i, 1) == "\"")
            {
                if (!buildValue)
                {
                    buildValue = true;
                }
                else
                {
                    break;
                }
            }
        }

        if (value == "")
        {
            return -1;
        }
        lines.Clear();
        if (percentage)
        {
            return float.Parse(value) / total * 100;
        }
        return float.Parse(value);
    }

    public float ExtractData(string key)
    {
        // Extract each line
        bool readingLine = false;
        string line = "";
        for (int i = 0; i < textResult.Length; i++)
        {
            if (textResult.Substring(i, 1) == "[")
            {
                readingLine = true;
            }
            else if (textResult.Substring(i, 1) == "]")
            {
                line += textResult.Substring(i, 1);
                lines.Add(line);
                line = "";
                readingLine = false;
            }

            if (readingLine)
            {
                line += textResult.Substring(i, 1);
            }
        }

        // Find the line with the key
        int lineIndex = 0;
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].Contains(key))
            {
                lineIndex = i;
                break;
            }
        }

        // Build the value in the line
        string value = "";
        bool buildValue = false;
        for (int i = 0; i < lines[lineIndex].Length; i++)
        {
            if (buildValue && lines[lineIndex].Substring(i, 1) != "\"")
            {
                value += lines[lineIndex].Substring(i, 1);
            }

            if (lines[lineIndex].Substring(i, 1) == "\"")
            {
                if (!buildValue)
                {
                    buildValue = true;
                }
                else
                {
                    break;
                }
            }
        }

        if (value == "")
        {
            return -1;
        }
        lines.Clear();
        return float.Parse(value);
    }
}
