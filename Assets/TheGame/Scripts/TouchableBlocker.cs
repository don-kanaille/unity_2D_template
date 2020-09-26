using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basis-Klasse für alle Blockaden, die bei Berührung
/// benachrichtigt werden sollen.
/// </summary>
public class TouchableBlocker : MonoBehaviour
{
    /// <summary>
    /// Wird aufgerufen, wenn das Objekt berührt wird.
    /// Unerklassen sollten diese Methode überschreiben,
    /// um auf den Kontakt zu reagieren, ähnlich wie bei OnTrigger-Ereignissen.
    /// </summary>
    public virtual void OnTouch() {
        // Code
    }
}
