﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InterViewScript : MonoBehaviour
{
    public TextAsset xmlRawFile;

    private List<GameObject> interViewButtons = new List<GameObject>();

    public Text sourceText;

    public void Awake()
    {
        string data = xmlRawFile.text;

        Clear();

        ReadInterViews(data);

        GenerateInterViewButtons();
    }

    private void Clear()
    {
        interViewButtons.Clear();
        Data.InterViews.Clear();
    }

    private void ReadInterViews(string xmlFileAsText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlFileAsText);

        IEnumerable<XElement> interView = xmlDoc.Root.Element("MainSceneData").Elements("InterView").ToList();
        IEnumerable<XElement> interViewSource = xmlDoc.Root.Element("InteViewSceneData").Elements("Data").ToList();

        foreach (XElement interViewXelement in interView)
        {
            string imageName = interViewXelement.Element("Image").Value;
            string buttonText = interViewXelement.Element("NameText").Value;
            string interViewText = interViewXelement.Element("InterViewText").Value;
            InterView interViewData = new InterView(imageName, buttonText, interViewText);
            Data.InterViews.Add(interViewData);
        }

        foreach (XElement interViewSourceXelement in interViewSource)
        {
            string source = interViewSourceXelement.Element("Source").Value;

            sourceText.text = source;
        }
    }

    private void GenerateInterViewButtons()
    {
        GameObject prefabButton = GameObject.Find("InterVieweePrefabButton");
        float xSize = 330;

        for (int i = 0; i < Data.InterViews.Count; i++)
        {
            InterView interView = Data.InterViews[i];

            //string[] images = Directory.GetFiles(@"C:\Gitrepos\Szakdolgozat\Code\Assets\Background\Interviews", "*.jpg");

            GameObject newButton = Instantiate(prefabButton, new Vector3((xSize - 1000) + xSize * i, 0f, 0f), Quaternion.identity) as GameObject;
            newButton.transform.SetParent(null);

            if (newButton == null)
            {
                interViewButtons.Clear();
                GenerateInterViewButtons();
            }

            interViewButtons.Add(newButton);
            UnityEngine.UI.Button button = newButton.GetComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(() => HandleInterViewButtonClick(button));

            Text buttonText = (Text)newButton.GetComponentInChildren(typeof(Text));
            //Image buttonImage = (Image)newButton.GetComponentInChildren(typeof(Image));

            buttonText.text = interView.Name;
            //buttonImage.sprite
        }
        foreach (var button in interViewButtons)
        {
            button.transform.SetParent(GameObject.FindGameObjectWithTag("interViewCanv").transform, false);
        }

    }

    Text interVieweeNamePanel;
    Text interVieweeTextPanel;

    private void HandleInterViewButtonClick(Button button)
    {
        GameObject mainInterviewcanvas = GameObject.Find("MainInterviewcanvas");
        GameObject interVieweeCanvas = mainInterviewcanvas.transform.Find("InterVieweeCanvas").gameObject;
        GameObject interViewCanvas = mainInterviewcanvas.transform.Find("InterViewCanvas").gameObject;
        interVieweeCanvas.SetActive(true);
        interViewCanvas.SetActive(false);

        interVieweeNamePanel = (Text)GameObject.Find("InterVieweeNamePanel").GetComponentInChildren(typeof(Text));
        interVieweeTextPanel = (Text)GameObject.Find("InterVieweeTextPanel").GetComponentInChildren(typeof(Text));

        Text buttonText = (Text)button.GetComponentInChildren(typeof(Text));
        InterView interView = Data.InterViews.Find(i => i.Name == buttonText.text);

        interVieweeNamePanel.text = interView.Name;
        interVieweeTextPanel.text = interView.Text;
    }
}