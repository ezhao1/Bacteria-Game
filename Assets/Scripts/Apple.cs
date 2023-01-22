using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Apple : MonoBehaviour
{
    private int value;
    public int Value   // property
    {
        get { return this.value; }   // get method
        set { this.value = value; _text.text = this.value.ToString(); }  // set method
    }
    [SerializeField] private SpriteRenderer _visual;
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private BoxCollider2D _boxCollider;
    private AppleSelectionOutline _outline;

    public bool Selected;
    private bool _disabled;


    private Vector3 initialScale;
    private Vector3 initialPosition;
    private bool mousedOver;

    //private Vector3 scaleVelocity = Vector3.zero;
    //public float scaleChangeTime = 0.05f;

    public void Awake()
    {
        _disabled = false;
        Selected = false;

        initialScale = transform.localScale;
        initialPosition = transform.position;
        GameManager.Instance.OnMoveCompleted += this.OnMove;

        // cool juice things
        transform.localScale = Vector3.zero;

        

        // change to random animation frame 
        Animator animator = _visual.GetComponent<Animator>();
        if (animator != null)
        {
            animator.speed = Random.Range(0.9f, 1.1f);
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);//could replace 0 by any other animation layer index
            animator.Play(state.fullPathHash, -1, Random.Range(0f, 1f));

            //https://forums.tigsource.com/index.php?topic=49645.0
        }

    }
    public void Start()
    {
        Vector3 centerPos = new Vector3(GameManager.Instance.CurrentLevelSettings.width / 2f - 0.5f, GameManager.Instance.CurrentLevelSettings.height / 2f - 0.5f);
        Debug.Log(centerPos);

        transform.position += (initialPosition - centerPos) * 10;
    }

    public void Update()
    {
        _outline.transform.position = new Vector2(transform.position.x, transform.position.y - 0.07f);

        if (mousedOver || Selected)
        {
            //transform.localScale = Vector3.SmoothDamp(transform.localScale, initialScale * 1.2f, ref scaleVelocity, scaleChangeTime);
            transform.localScale = Vector3.Slerp(transform.localScale, initialScale * 1.2f, Time.deltaTime * 5f);
        }
        else
        {
            //transform.localScale = Vector3.SmoothDamp(transform.localScale, initialScale, ref scaleVelocity, scaleChangeTime);
            transform.localScale = Vector3.Slerp(transform.localScale, initialScale, Time.deltaTime * 5f);
        }
        transform.position = Vector3.Lerp(transform.position, initialPosition, Time.deltaTime * 2.5f);
    }

    public virtual void OnMove() // should be overridden by special apples
    {

    }

    public virtual void OnClear() // should be overridden by special apples
    {
        AnimateDelete();
    }

    public void OnMouseEnter()
    {
        mousedOver = true;
    }

    public void OnMouseExit()
    {
        mousedOver = false;
    }

    public virtual void Init(int value, AppleSelectionOutline outline)
    {
        this.Value = value;
        _outline = outline;
        _rigidbody.isKinematic = true;
        _outline.transform.position = transform.position;
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

        GameManager.Instance.OnMoveCompleted -= this.OnMove;
        _outline.spriteRenderer.enabled = false;

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
        GameManager.Instance.OnMoveCompleted -= this.OnMove;
        Destroy(gameObject);
        Destroy(_outline.gameObject);
    }

    public void OnBecameInvisible()
    {
        TotalDestroy();
    }
}
