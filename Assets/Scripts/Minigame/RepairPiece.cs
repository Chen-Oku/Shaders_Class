using UnityEngine;

public class RepairPiece : MonoBehaviour
{
    [Tooltip("Optional ID to match with a slot")]
    public string pieceId;

    public bool isPlaced = false;

    // Called after the player releases the piece
    public void OnReleased()
    {
        // check nearby slots
        var slots = FindObjectsOfType<RepairSlot>();
        foreach (var s in slots)
        {
            if (s.IsMatching(this) && s.IsNearby(transform.position))
            {
                s.PlacePiece(this);
                return;
            }
        }
    }
}
