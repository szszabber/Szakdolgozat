using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime;
using System.Xml.Linq;
using System.Linq;
using System;
//using System;

public class XMLQuotesReader : MonoBehaviour
{
    public TextAsset xmlRawFile;

    //public Text textOne;
    public GameObject Text;
    public string Quote;

    public void RandomQuote()
    {
        string[] quotes = new string[] {
            "Maga a hallgatás mestere, Watson, ezért olyan izgalmas magával beszélgetni.",
            "Mily szörnyű a bölcsesség, ha nem lehet hasznára a bölcsnek!",
            "Szarvashiba elméletet alkotni adatok nélkül.",
            "A részletek fontosságát nem lehet eltúlozni!",
            "A vaknál is vakabb az, aki nem lát a szemétől.",
            "A szerelmünknek nem tudunk parancsolni, de a cselekedeteinknek igen.",
            "Teljesen fölösleges megtiltani valamit egy nőnek, mert úgyis az lesz, amit ő akar.",
            "Ha a lehetetlent kizártuk, ami marad, az az igazság, akármilyen valószínűtlen legyen is."};
        Quote = quotes[UnityEngine.Random.Range(0, quotes.Length)];
        Text.GetComponent<Text>().text = Quote.ToString();
    }
}