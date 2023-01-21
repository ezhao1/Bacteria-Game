using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public int Value;

    [SerializeField] private SpriteRenderer _visual;
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private BoxCollider2D _boxCollider;
    private AppleSelectionOutline _outline;

    public bool Selected;
    private bool _disabled;

    public void Init(int value, AppleSelectionOutline outline)
    {
        Value = value;
        _text.text = value.ToString();
        _rigidbody.isKinematic = true;
        _outline = outline;
        _disabled = false;
        Selected = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<GameManager>() && !_disabled) {
            Selected = true;
            _outline.gameObject.SetActive(true);
            collision.gameObject.GetComponent<GameManager>().SelectApple(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var gamemanager = collision.gameObject.GetComponent<GameManager>();
        if (gamemanager && !_disabled)
        {
            Selected = false;
            _outline.gameObject.SetActive(false);
            gamemanager.DeselectApple(this);
        }
    }
    public void AnimateDelete()
    {
        _disabled = true;
        this.gameObject.layer = 3;
        _outline.spriteRenderer.sortingOrder = 1;
        _visual.sortingOrder = 2;
        _text.sortingOrder = 3;
        _rigidbody.isKinematic = false;
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody.AddForce(new Vector2(Random.Range(-1100f, 1100f), Random.Range(200f, 2200f)));
        _boxCollider.enabled = false;
    }

    public void TotalDestroy()
    {
        Destroy(gameObject);
        Destroy(_outline.gameObject);
    }

    public void OnBecameInvisible()
    {
        TotalDestroy();
    }

    public void Update()
    {
        _outline.transform.position = new Vector2(transform.position.x, transform.position.y - 0.07f);
    }

}
