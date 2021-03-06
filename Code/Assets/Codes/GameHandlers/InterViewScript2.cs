﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InterViewScript2 : MonoBehaviour
{
    public TextAsset xmlRawFile;

    Text interVieweeNamePanel;

    Text interVieweeTextPanel;

    private List<GameObject> interViewButtons = new List<GameObject>();

    public ScrollRect scrollView;

    public GameObject scrollContent;

    public GameObject scrollItemPrefab;

    public bool isGenerated = false;

    public void Awake()
    {
        if (isGenerated == false)
        {
            GenerateInterViewButtons();
            isGenerated = true;
        }
    }

    private void GenerateInterViewButtons()
    {
        GameObject prefabButton = GameObject.Find("InterViewPrefabButton");

        for (int i = 0; i < Data.InterViews.Count; i++)
        {
            InterView interView = Data.InterViews[i];

            GameObject newButton = Instantiate(prefabButton);

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

            buttonText.text = interView.Name;
        }
        foreach (var button in interViewButtons)
        {
            button.transform.SetParent(scrollContent.transform, false);
        }
    }

    private void HandleInterViewButtonClick(Button button)
    {
        GameObject mainInterviewcanvas = GameObject.Find("MainInterViewCanvas");
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
