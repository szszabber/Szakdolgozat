using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneDataReader : MonoBehaviour
{
    public TextAsset xmlRawFile;

    public Text sourceText;

    private void Start()
    {
        string data = xmlRawFile.text;

        ReadMainSceneData(data);
    }

    private void ReadMainSceneData(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        IEnumerable<XElement> mainScene = xmlDoc.Root.Element("MainSceneData").Elements("MainScene").ToList();

        foreach (XElement mainMenuXelement in mainScene)
        {
            string source = mainMenuXelement.Element("Source").Value;

            sourceText.text = source;
        }
    }
}
