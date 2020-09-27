using UnityEngine;
using System.Collections;

/// <summary>
/// Oberste Klasse für Spielobjekte meines PixelArt-Games.
/// Enthält allgemeine Funktionen, die für die meisten Szenenobjekte
/// potentiell nützlich sind.
/// </summary>
public class TheGameObject : MonoBehaviour
{
    /// <summary>
    /// Größe eines PixelArt-Pixels in Unity-Einheiten.
    /// </summary>
    private static float pixelFrac = 1f / 16f; //16 = Pixels per Unit

    /// <summary>
    /// Runde auf PixelArt-Pixel.
    /// </summary>
    protected float roundToPixelGrid(float f)
    {
        return Mathf.Ceil(f / pixelFrac) * pixelFrac;
    }

    /// <summary>
    /// Zeiger auf den Box-Collider für die Kollisionserkennung
    /// mittels isColliding, um die Suchfunktion (getComponent) einzusparen.
    /// </summary>
    protected BoxCollider2D boxCollider;

    /// <summary>
    /// Ergebnis-Zwischenspeicher für Kollisionserkennung
    /// mittels isColliding.
    /// </summary>
    protected Collider2D[] colliders;

    /// <summary>
    /// Der Filter, der Kollisionsobjekte im Sinne von Hindernissen findet.
    /// (Trigger werden ignoriert!)
    /// </summary>
    protected ContactFilter2D obstacleFilter;

    /// <summary>
    /// Zeiger auf Animator-Komponente, die die Sprite-Animation realisiert.
    /// Die Laufbewegung wird mit diesem Animator synchronisiert.
    /// </summary>
    protected Animator anim;

    /// <summary>
    /// Anzahl der bei der letzten Kollisionsprüfung gefundenen
    /// Kollisionspartner (=Anzahl der Suchergebnisse in colliders)
    /// </summary>
    protected int numFound = 0;

	protected virtual void Awake()
	{
        boxCollider = GetComponent<BoxCollider2D>();
        colliders = new Collider2D[10];
        anim = GetComponent<Animator>();
        obstacleFilter = new ContactFilter2D();
	}

	/// <summary>
	/// Prüft, ob eine Kollision zwischen dem BoxCollider2D dieses Spielobjekts
	/// und anderen 2D-Kollidern stattfindet.
	/// </summary>
	protected bool isColliding()
    {
        numFound = boxCollider.OverlapCollider(obstacleFilter, colliders);
        return numFound > 0;
    }


    /// <summary>
    /// Bewegung, die die Figur in diesem Frame vollziehen soll.
    /// 1 = nach rechts/oben, -1 = nach links/unten.
    /// </summary>
    public Vector3 change = new Vector3();

    private void LateUpdate()
    {
        anim.SetFloat("change_x", change.x);
        //anim.SetFloat("change_y", change.y);
        /*
        if (change.y <= -1f) 
        {
            anim.SetFloat("lookAt", 0f);
        } else if (change.x <= -1f)
        {
            anim.SetFloat("lookAt", 1f);
        } else if (change.y >= 1f)
        {
            anim.SetFloat("lookAt", 2f);
        } else if (change.x >= 1f) 
        {
            anim.SetFloat("lookAt", 3f);
        }*/

        float step = roundToPixelGrid(1f * Time.deltaTime);
        Vector3 oldPos = transform.position;
        transform.position += change * step;

        if (isColliding())
        {
            transform.position = oldPos;
            for (int i = 0; i < numFound; i++)
            {
                TouchableBlocker tb = colliders[i].GetComponent<TouchableBlocker>();

                if (tb != null)
                {
                    tb.OnTouch();
                }
            }
        }
        change = Vector3.zero;
    }

    /// <summary>
    /// Berechnet den genauen Mittelpunkt der Kachel in der sich die Figur
    /// befindet. Kann verwendet werden um die Figur in eine Kachel einzurasten,
    /// </summary>
    public Vector3 getFullTilePosition()
    {
        Vector3 p = transform.position;
        p.x = Mathf.FloorToInt(p.x);
        p.y = Mathf.CeilToInt(p.y);

        p.x += 0.5f;
        p.y -= 0.5f;

        return p;
    }

    /// <summary>
    /// Schiebt die Figur auf die um deltaX/Y Kacheln entfertne Nachbarkachel 
    /// weiter.
    /// </summary>
    public void pushByTiles (float deltaX, float deltaY)
    {
        Vector3 tilePos = getFullTilePosition();
        tilePos.x += deltaX;
        tilePos.y += deltaY;

        Vector3 oldPosition = getFullTilePosition();
        transform.position = tilePos;

        if (isColliding())
        {
            transform.position = oldPosition;
        } else
        {
            StartCoroutine(animateMoveTowards(oldPosition, tilePos));
        }
    }

    /// <summary>
    /// Schiebt die Figur mit einer Animation von einer Position zu einer 
    /// anderen.
    /// </summary>
    private IEnumerator animateMoveTowards (Vector3 fromPos, Vector3 targetPos) {
        float duration = 0.2f;
        for (float t = 0; t <= 1f; t += Time.deltaTime / duration)
        {
            transform.position = Vector3.Lerp(fromPos, targetPos, t);
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Drückt die Figur vom angegeben Objekt wegwärts
    /// </summary>
    public void pushAwayFrom(MonoBehaviour deflector, bool topLeftAnker)
    {
        Vector3 diff;
    
        if (topLeftAnker) { // Ausrichtung links oben
            diff = transform.position - (deflector.transform.position + new Vector3(0.5f, -0.5f, 0f)) ;

        } else { // Ausrichtung am Mittelpunkt
            diff = transform.position - deflector.transform.position;
        }
        pushByTiles(diff.x, diff.y);
    }

    /// <summary>
    /// Lässt die Figur blinken.
    /// </summary>
    public virtual void flicker(int times) // Virtual: in Unterklassen überschreibbar
    {
        StartCoroutine(animateFlicker(times));
    }

    /// <summary>
    /// Animiert das Blinken, mittels Flicker.
    /// </summary>
    private IEnumerator animateFlicker(int times)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        for (int i = 0; i < times; i++)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
