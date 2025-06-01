using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerItemFader : MonoBehaviour
{
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        ItemFader[] itemFaders = other.GetComponentsInChildren<ItemFader>();

        if (itemFaders.Length > 0)
        {
            foreach (ItemFader itemFader in itemFaders)
            {
                itemFader.FadeOut();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ItemFader[] itemFaders = other.GetComponentsInChildren<ItemFader>();

        if (itemFaders.Length > 0)
        {
            foreach (ItemFader itemFader in itemFaders)
            {
                itemFader.FadeIn();
            }
        }
    }
}
