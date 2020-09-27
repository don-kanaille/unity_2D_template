using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        anim = GetComponent<Animator>();
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

        change = Vector3.zero;
    }
}
