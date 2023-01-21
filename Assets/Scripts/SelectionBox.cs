using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBox : MonoBehaviour
{
    public Vector2 Pos => transform.position;
    [SerializeField] private Color standardColor;
    [SerializeField] private Color matchFoundColor;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer.color = standardColor;
    }

    public void UpdateSelectionColor(bool isAMatch)
    {
        if (isAMatch)
        {
            _spriteRenderer.color = matchFoundColor;
        }
        else
        {
            _spriteRenderer.color = standardColor;
        }
    }
}
