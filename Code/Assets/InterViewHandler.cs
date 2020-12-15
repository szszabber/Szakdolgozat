﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InterViewHandler : MonoBehaviour
{
    public TextAsset xmlRawFile;

    private List<GameObject> interViewButtons = new List<GameObject>();

    Text panel1Text;
    Text panel2Text;

    //string[] images = Directory.GetFiles(@"C:\Gitrepos\Szakdolgozat\Code\Assets\Background\Interviews", "*.jpg");


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

        foreach (XElement interViewXelement in interView)
        {
            string imageName = interViewXelement.Element("Image").Value;
            string buttonText = interViewXelement.Element("NameText").Value;
            string interViewText = interViewXelement.Element("InterViewText").Value;
            InterView interViewData = new InterView(imageName, buttonText, interViewText);
            Data.InterViews.Add(interViewData);
        }
    }

    private void GenerateInterViewButtons()
    {
        GameObject prefabButton = GameObject.Find("InterViewPrefabButton");

        float ySize = 50f;

        for (int i = 0; i < Data.InterViews.Count; i++)
        {
            InterView interView = Data.InterViews[i];

            //string[] images = Directory.GetFiles(@"C:\Gitrepos\Szakdolgozat\Code\Assets\Background\Interviews", "*.jpg");

            GameObject newButton = Instantiate(prefabButton, new Vector3(-270f, (ySize - 150f) + ySize * i, 0f), Quaternion.identity) as GameObject;
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
            Image buttonImage = (Image)newButton.GetComponentInChildren(typeof(Image));

            buttonText.text = interView.Name;
            //buttonImage.sprite = 
        }
        foreach (var button in interViewButtons)
        {
            button.transform.SetParent(GameObject.FindGameObjectWithTag("interViewCanv").transform, false);
        }
    }

    private void HandleInterViewButtonClick(Button button)
    {
        panel1Text = (Text)GameObject.Find("InterViewPanelTitleText").GetComponentInChildren(typeof(Text));
        panel2Text = (Text)GameObject.Find("InterViewPanelText").GetComponentInChildren(typeof(Text));


        Text buttonText = (Text)button.GetComponentInChildren(typeof(Text));
        InterView interView = Data.InterViews.Find(i => i.Name == buttonText.text);

        panel1Text.text = interView.Name;
        panel2Text.text = interView.Text;
    }
}