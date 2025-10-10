using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MinigameManager : MonoBehaviour
{
    public List<RepairSlot> slots = new List<RepairSlot>();
    public List<RepairPiece> pieces = new List<RepairPiece>();
    public Text statusText;

    private int placedCount = 0;

    void Start()
    {
        // auto-collect if not assigned
        if (slots.Count == 0) slots.AddRange(FindObjectsOfType<RepairSlot>());
        if (pieces.Count == 0) pieces.AddRange(FindObjectsOfType<RepairPiece>());
        UpdateStatus();
    }

    public void OnPiecePlaced(RepairSlot slot, RepairPiece piece)
    {
        placedCount++;
        UpdateStatus();
        // optional: check win
        if (placedCount >= slots.Count)
        {
            OnWin();
        }
    }

    private void UpdateStatus()
    {
        if (statusText != null)
            statusText.text = $"Piezas colocadas: {placedCount}/{slots.Count}";
    }

    private void OnWin()
    {
        if (statusText != null)
            statusText.text = "¡Museo reparado!";
        Debug.Log("Minijuego completado: todas las piezas colocadas.");
    }
}
