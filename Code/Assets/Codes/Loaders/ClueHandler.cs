using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ClueHandler : MonoBehaviour
{
    private List<GameObject> clueButtons = new List<GameObject>();

    public Clue selectedClue1;

    public Clue selectedClue2;

    public UnityEngine.UI.Button selectedButton1;

    public UnityEngine.UI.Button selectedButton2;

    public void Start()
    {
        Clear();

        GenerateClueButtons();
    }

    private void Clear()
    {
        clueButtons.Clear();

        selectedButton1 = null;

        selectedButton2 = null;

        selectedClue1 = null;

        selectedClue2 = null;

       
    }

    private void GenerateClueButtons()
    {
        GameObject prefabButton = GameObject.Find("CluePrefabButton");

        UnityEngine.UI.Button plusButton = GameObject.Find("PlusButton").GetComponent<UnityEngine.UI.Button>();
        plusButton.onClick.RemoveAllListeners();
        plusButton.onClick.AddListener(HandlePlusButtonClick);

        for (int i = 0; i < Data.Clues.Count; i++)
        {
            Clue clue = Data.Clues[i];

            GameObject newButton = GenerateNewClueButton(prefabButton);

            if (newButton == null)
            {
                clueButtons.Clear();
                GenerateClueButtons();
            }

            // Ezzel itt gond volt mert valamiért megnövelte a méretet --> newButton.transform.SetParent(GameObject.FindGameObjectWithTag("cluecanv").transform, false);
            clueButtons.Add(newButton);
            UnityEngine.UI.Button button = newButton.GetComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(() => HandleClueButtonClick(button));

            Text buttonText = (Text)newButton.GetComponentInChildren(typeof(Text));
            buttonText.text = clue.Title;
        }

        foreach (var button in clueButtons)
        {
            button.transform.SetParent(GameObject.FindGameObjectWithTag("cluecanv").transform, false);
        }
    }

    public static bool FedesbenVan(GameObject gameObjectA, GameObject gameObjectB)
    {
        // Ha a megtalált GameObject nem ClueButton tag-el van ellátva akkor false-t ad vissza
        if (gameObjectA.tag != "ClueButton")
        {
            return false;
        }

        RectTransform rectTransformA = gameObjectA.transform as RectTransform;
        RectTransform rectTransformB = gameObjectB.transform as RectTransform;

        Rect rectA = CalculateRect(rectTransformA);
        Rect rectB = CalculateRect(rectTransformB);

        // Megvizsgáljuk, hogy a két gameObject generálás után fedésben van e
        return rectA.Overlaps(rectB, true);
    }

    public static Rect CalculateRect(RectTransform rectTransform)
    {
        //a recttransform 4 sarkát lekérdezem worldspace-ben
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        corners = AddPaddingToRect(corners);

        //kiszámítom a szélességét és magasságát
        Vector2 size = new Vector2(corners[3].x - corners[0].x, corners[1].y - corners[0].y); //Corners[0] a bal alsó, Corners[1] a bal felső, Corners[2] a jobb felső, Corners[3] a jobb alsó sarok

        //létrehozok egy Rect-et. 
        return new Rect(corners[1], size);
    }

    public static Vector3[] AddPaddingToRect(Vector3[] corners)
    {
        // Kibővítem a clueButton-ok szélességét és magasságát, hogy a generálásnál, ne kerüljenek közvetlen egymás mellé

        corners[0].x -= 2;
        corners[0].y -= 2;
        corners[1].x -= 2;
        corners[1].y += 2;
        corners[2].x += 2;
        corners[2].y += 2;
        corners[3].x += 2;
        corners[3].y -= 2;

        return corners;
    }

    public GameObject GenerateNewClueButton(GameObject prefabClueButton)
    {
        Vector3 spawnPosition = new Vector3();

        GameObject newButton = Instantiate(prefabClueButton, spawnPosition + (spawnPosition), Quaternion.identity) as GameObject;
        newButton.transform.SetParent(null);


        RectTransform prefabRectTransform = (prefabClueButton.transform as RectTransform);
        //Rect prefabRect = prefabRectTransform.rect;

        //Transform clueCanvasTransform = GameObject.FindGameObjectWithTag("cluecanv").transform;
        bool vanAtfedes = true;

        int minX = -210;
        int maxX = 210;
        int minY = -130;
        int maxY = 130;

        // addig generálunk egy újabb pozíciót, amíg az jó helyre nem kerül
        int tryCount = 0;
        while (vanAtfedes && tryCount < 100000)
        {
            float xPos = UnityEngine.Random.Range(minX, maxX);
            float yPos = UnityEngine.Random.Range(minY, maxY);
            newButton.transform.position = new Vector3(xPos, yPos, 0f);

            int i = 0;
            //while (i < clueCanvasTransform.childCount && !FedesbenVan(clueCanvasTransform.GetChild(i).gameObject, newButton))
            while (i < clueButtons.Count && !FedesbenVan(clueButtons[i], newButton))
            {
                i++;
            }

            vanAtfedes = i < clueButtons.Count && clueButtons.Count != 0;

            ++tryCount;
        }

        if (tryCount == 100000)
        {
            return null;
        }

        return newButton;
    }

    public void HandleClueButtonClick(UnityEngine.UI.Button button)
    {

        // Megkeresem a képernyő alján levő panelek text komponenseit
        Text panel1TitleText = (Text)GameObject.Find("SelectedClueTitlePanel1").GetComponentInChildren(typeof(Text));
        Text panel2TitleText = (Text)GameObject.Find("SelectedClueTitlePanel2").GetComponentInChildren(typeof(Text));
        Text panel1DescText = (Text)GameObject.Find("SelectedClueDescPanel1").GetComponentInChildren(typeof(Text));
        Text panel2DescText = (Text)GameObject.Find("SelectedClueDescPanel2").GetComponentInChildren(typeof(Text));

        // ClueButton Text komponense
        Text buttonText = (Text)button.GetComponentInChildren(typeof(Text));
        Clue clue = Data.Clues.Find(c => c.Title == buttonText.text);

        // Mikor kerül az első panelre és mikor a másodikra a Clue leírása
        if (selectedClue1 == null)
        {
            selectedClue1 = clue;
            panel1DescText.text = clue.Desription;
            panel1TitleText.text = clue.Title;
            selectedButton1 = button;
        }
        else if (selectedClue2 == null && selectedClue1 != clue)
        {
            selectedClue2 = clue;
            panel2DescText.text = clue.Desription;
            panel2TitleText.text = clue.Title;
            selectedButton2 = button;
        }
        else if (selectedClue1 == clue && selectedClue2 == null)
        {
            selectedClue1 = null;
            panel1DescText.text = "";
            panel1TitleText.text = "";
            selectedButton1 = null;
        }
        else if (selectedClue2 == clue && selectedClue1 != null)
        {
            selectedClue2 = null;
            panel2DescText.text = "";
            panel2TitleText.text = "";
            selectedButton2 = null;
        }
        //else if(selectedClue1 != null && selectedClue2 != null)
        //{
        //    selectedClue1 = clue;
        //    panel1DescText.text = clue.Desription;
        //    panel1TitleText.text = clue.Title;
        //    selectedButton1 = button;

        //    selectedClue2 = null;
        //    panel2DescText.text = "";
        //    panel2TitleText.text = "";
        //    selectedButton2 = null;
        //}
        else
        {
            selectedClue1 = clue;
            panel1DescText.text = clue.Desription;
            panel1TitleText.text = clue.Title;
            selectedButton1 = button;

            selectedClue2 = null;
            panel2DescText.text = "";
            panel2TitleText.text = "";
            selectedButton2 = null;
        }
    }

    public void HandlePlusButtonClick()
    {
        // Megkeresem a képernyő alján levő panelek text komponenseit
        Text panel1TitleText = (Text)GameObject.Find("SelectedClueTitlePanel1").GetComponentInChildren(typeof(Text));
        Text panel2TitleText = (Text)GameObject.Find("SelectedClueTitlePanel2").GetComponentInChildren(typeof(Text));
        Text panel1DescText = (Text)GameObject.Find("SelectedClueDescPanel1").GetComponentInChildren(typeof(Text));
        Text panel2DescText = (Text)GameObject.Find("SelectedClueDescPanel2").GetComponentInChildren(typeof(Text));

        if (selectedClue1 == null && selectedClue2 == null)
        {
            //Toast Unity pack a pop up üzenet megjelenítésére (akkor jelenik meg, ha nem választott ki semmit a felhasználó)
            Toast.Instance.Show("A párosításhoz nyomokat kell választani!", 1f, Toast.ToastColor.Dark);
            return;
        }
        else if (selectedClue1 == null || selectedClue2 == null)
        {
            //Toast Unity pack a pop up üzenet megjelenítésére (akkor jelenik meg, ha csak egy nyomot választott ki a felhasználó)
            Toast.Instance.Show("A párosításhoz két nyomot kell választani!", 1f, Toast.ToastColor.Dark);
            return;
        }

        // Megnézzük, hogy a két clue párban van-e
        Relation clueRelation = Data.ClueRelations.Find(crelation => crelation.Input1 == selectedClue1 || crelation.Input2 == selectedClue1);
        // ha párban van
        if (clueRelation != null && (selectedClue2 == clueRelation.Input1 || selectedClue2 == clueRelation.Input2))
        {
            // ha a clue párnak csak 1 kimenete van
            if (clueRelation.Output2 == null)
            {
                // Megnézzük, hogy az így kapott conclusion-nek van-e conclusion-párja, amivel motivációt alkot
                Relation conclusionRelation = Data.ConclusionRelations.Find(conRel =>
                       conRel.Input1 == clueRelation.SelectedOutput
                    || conRel.Input2 == clueRelation.SelectedOutput);
                if (conclusionRelation != null)
                {
                    // Ha van, akkor megézzük, hogy a conclusion-pár másik tagja ki lett-e már választva
                    Relation previousClueRelation = Data.ChoosenClueRelations.Find(clueRel =>
                           clueRel.SelectedOutput == conclusionRelation.Input1
                        || clueRel.SelectedOutput == conclusionRelation.Input2);
                    // Ha már a másik conclusion is ki lett választva
                    if (previousClueRelation != null)
                    {
                        // Felkerül egy motiváció
                        Data.ChoosenConclusionRelations.Add(conclusionRelation);

                        // Megnézem, hogy van-e olyan konklúzió, ami ezzel a motivációval (conclusionPair) finalDeduction-ba megy
                        if (conclusionRelation.SelectedOutput != null)
                        {
                            Relation conclusionAndMotivationToFinalDeductionRelation = Data.ConclusionAndMotivationToFinalDeductionRelations.Find(conclusionAndMotivationRelation =>
                            conclusionAndMotivationRelation.Input2 == conclusionRelation.SelectedOutput);
                            if (conclusionAndMotivationToFinalDeductionRelation != null)
                            {
                                // Felkerül egy finalDeduction
                                Data.ChoosenConclusionAndMotivationToFinalDeductionRelations.Add(conclusionAndMotivationToFinalDeductionRelation);
                            }
                        }
                    }
                }

                // Megnézzük, hogy van-e motiváció, amivel finalDeductionba mutat ez a konklúzió
                Relation conclusionAndMotivationToFinalDeductionRelations = Data.ConclusionAndMotivationToFinalDeductionRelations.Find(conAndMotToFinal => 
                       conAndMotToFinal.Input1 == clueRelation.SelectedOutput 
                    || conAndMotToFinal.Input2 == clueRelation.SelectedOutput);
                // Ha van finalDeduction hozzá
                if (conclusionAndMotivationToFinalDeductionRelations != null)
                {
                    // Megnézzük, hogy van e kiválasztott motiváció hozzá
                    Relation previousConclusionRelation = Data.ChoosenConclusionRelations.Find(conRel => 
                           conRel.SelectedOutput == conclusionAndMotivationToFinalDeductionRelations.Input2);
                    if (previousConclusionRelation != null)
                    {
                        // Felkerül egy finalDeduction
                        Data.ChoosenConclusionAndMotivationToFinalDeductionRelations.Add(conclusionAndMotivationToFinalDeductionRelations);
                    }
                }

                // Megnézzük, hogy van-e korábban kiválasztott conclusion, amivel finalDeductionba mutat ez a konklúzió
                Relation conclusionToFinalDeductionRelation = Data.ConclusionToFinalDeductionRelations.Find(concToFinalDed =>
                       concToFinalDed.Input1 == clueRelation.SelectedOutput
                    || concToFinalDed.Input2 == clueRelation.SelectedOutput);
                // Ha van finalDeduction hozzá
                if (conclusionAndMotivationToFinalDeductionRelations != null)
                {
                    // Megnézzük, hogy van e kiválasztott motiváció hozzá
                    Relation previousClueRelation = Data.ChoosenClueRelations.Find(clueRel =>
                           clueRel.SelectedOutput == conclusionAndMotivationToFinalDeductionRelations.Input1
                        || clueRel.SelectedOutput == conclusionAndMotivationToFinalDeductionRelations.Input2);
                    if (previousClueRelation != null)
                    {
                        // Felkerül egy finalDeduction
                        Data.ChoosenConclusionsToFinalDeductionRelations.Add(conclusionAndMotivationToFinalDeductionRelations);
                    }
                }
            }

            // Felkerül egy Conclusion
            Data.ChoosenClueRelations.Add(clueRelation);

            //A sikeresen párba állított nyomok eltűnnek a képernyőről
            selectedButton1.gameObject.SetActive(false);
            selectedButton2.gameObject.SetActive(false);

            //Toast Unity pack a pop up üzenet megjelenítésére (akkor jelenik meg, ha sikeres a párosítás)
            Toast.Instance.Show("Sikeres párosítás!\nA konklúzió felkerült a gráfra!", 2f, Toast.ToastColor.Green);

            //GraphDrawer graphDrawer = (GraphDrawer)FindObjectOfType<GraphDrawer>();
            //graphDrawer.Awake();
        }
        else
        {
            //Letöltött Toast Unity pack a pop up üzenet megjelenítésére (akkor jelenik meg, ha nincs párban a két kiválasztott nyom)
            Toast.Instance.Show("A kiválasztott nyomok nem alkotnak párt!", 2f, Toast.ToastColor.Red);
        }

        //Ha sikerül a párosítás ha nem, az elmentett gombokat, panelTexteket és nyomokat alaphelyzetbe állítom
        selectedClue1 = null;
        panel1DescText.text = "";
        panel1TitleText.text = "";
        selectedButton1 = null;
        selectedClue2 = null;
        panel2DescText.text = "";
        panel2TitleText.text = "";
        selectedButton2 = null;
    }
}