using UnityEngine;

public class RepairSlot : MonoBehaviour
{
    [Tooltip("ID expected from piece")]
    public string expectedPieceId;
    public float acceptDistance = 0.5f;
    public Transform snapTarget; // where piece should move to when placed

    private RepairPiece placedPiece;

    public bool IsMatching(RepairPiece piece)
    {
        if (string.IsNullOrEmpty(expectedPieceId)) return true;
        return piece.pieceId == expectedPieceId;
    }

    public bool IsNearby(Vector3 pos)
    {
        return Vector3.Distance(pos, transform.position) <= acceptDistance;
    }

    public void PlacePiece(RepairPiece piece)
    {
        if (!IsMatching(piece)) return;
        placedPiece = piece;
        piece.isPlaced = true;
        // snap into place
        if (snapTarget != null)
        {
            piece.transform.position = snapTarget.position;
            piece.transform.rotation = snapTarget.rotation;
        }
        else
        {
            piece.transform.position = transform.position;
            piece.transform.rotation = transform.rotation;
        }
        // disable dragging
        var drag = piece.GetComponent<Collider>();
        if (drag != null) drag.enabled = false;
        var rb = piece.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // notify manager
        var mgr = FindObjectOfType<MinigameManager>();
        if (mgr != null) mgr.OnPiecePlaced(this, piece);
    }
}
