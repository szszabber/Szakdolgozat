using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Overlap : MonoBehaviour
{
    //FONTOS!
    /*
     * A Unity <Canvas> rendszere elég bonyolulttá tudja tenni a számításokat, mert nem csak 2D-s (azaz XY síkon elhelyezkedő) kezelőfelületre van felkészítve.
     * Az első "CalculateRect" a megoldás feltételezi hogy a Canvas.RenderMode == Overlay (inspectorban találod). 
     * Ilyenkor a canvas és elemei alapvetően az XY síkon helyezkednek el ami megkönnyíti az életet.
     * Ha átrakod a rendermode-t "Camera" vagy "World" módba, akkor teljesen más megoldás kell a coordináták kinyerésére.
     * Ez a második "CalculateRect" függvény.
     */

    //kiszámítja a recttransform-ból a worldspace Rect-et
    public static Rect CalculateRect(RectTransform rectTransform)
    {
        //először a recttransform 4 sarkát lekérdezzük worldspace-ben
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        //kiszámítjuk a szélességét és magasságát
        Vector2 size = new Vector2(corners[3].x - corners[0].x, corners[1].y - corners[0].y);

        //létrehozunk egy Rect-et az adatokból. Corners[0] a bal alsó, Corners[1] a bal felső, Corners[2] a jobb felső, Corners[3] a jobb alsó sarok
        return new Rect(corners[1], size);
    }

    //kiszámítja a recttransform-ból a worldspace Rect-et a Canvas local coordináta rendszere alapján ami a 3D-s canvasok esetében megoldást jelent
    //nem csak a Canvas rect-jét lehet itt használni, hanem bármelyik recttransformot amelyikhez képest szeretnéd a coordinátákat megtudni.
    public static Rect CalculateRect(RectTransform rectTransform, RectTransform canvasRect)
    {
        //először a recttransform 4 sarkát lekérdezzük worldspace-ben
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        for (int i = 0; i < 4; i++)
        {
            corners[i] = canvasRect.InverseTransformPoint(corners[i]);
        }

        //kiszámítjuk a szélességét és magasságát
        Vector2 size = new Vector2(corners[3].x - corners[0].x, corners[1].y - corners[0].y);

        //létrehozunk egy Rect-et az adatokból. Corners[0] a bal alsó, Corners[1] a bal felső, Corners[2] a jobb felső, Corners[3] a jobb alsó sarok
        return new Rect(corners[1], size);
    }

    //két recttransformot megvizsgál, hogy átfedésben vannak-e
    public static bool IsOverlapping(RectTransform a, RectTransform b)
    {
        Rect rectA = CalculateRect(a);
        Rect rectB = CalculateRect(b);

        //végeredményt a beépített Rect.Overlaps függvény adja
        return rectA.Overlaps(rectB);
    }
}