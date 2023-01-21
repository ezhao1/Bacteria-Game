using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _level = 1;
    [SerializeField] private Apple _applePrefab;
    [SerializeField] private AppleSelectionOutline _appleSelectionOutlinePrefab;
    [SerializeField] private SelectionBox _selectionBoxPrefab;
    [SerializeField] private SpriteRenderer _background;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Camera _camera;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private AudioClip _selection;
    [SerializeField] private TextMeshProUGUI _levelText;

    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] Vector2 initialMousePosition, currentMousePosition;
    private BoxCollider2D _boxColl;
    private SelectionBox _theSelectionBox;
    private List<Apple> _selectedApples = new List<Apple>();
    private List<List<Apple>> _apples;
    private int _totalApples = 0;
    private int _finalLevel = 3;

    public GameState _state;
    public float targetTime = 10.0f;

    public static event Action OnMoveCompleted;

    // Start is called before the first frame update
    void Start()
    {
        ScaleAccordingToScreen();
        ChangeState(GameState.GenerateLevel);
    }

    private void ChangeState(GameState newState)
    {
        _state = newState;

        switch(newState)
        {
            case GameState.GenerateLevel:
                InitializeLevel();
                break;
            case GameState.WaitingInput:
                break;
            case GameState.Selecting:
                break;
            case GameState.End:
                UpdateLevel();
                break; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        Select();
        if (_state == GameState.WaitingInput || _state == GameState.Selecting)
        {
            HandleTimer();
        }
    }

    private void HandleTimer()
    {
        if (targetTime > 0.0f)
        {
            targetTime -= Time.deltaTime;
            _timerText.text = targetTime.ToString("0.0");
        }
        else
        {
            ChangeState(GameState.End);
        }
    }

    void ScaleAccordingToScreen()
    {

        float width = Screen.safeArea.width;
        float height = Screen.safeArea.height;
        float ratio = height / width;
        if (ratio < 1.4f)
        {
            _camera.orthographicSize = 7.5f;
        }
        else if (ratio < 1.8f)
        {
            _camera.orthographicSize = 7f;
        }
        else
        {
            _camera.orthographicSize = 8.2f;
        }
        
    }

    private void Select()
    {
        if (Input.GetMouseButtonDown(0) && _state == GameState.WaitingInput)
        {
            ChangeState(GameState.Selecting);
            _lineRenderer.positionCount = 4;
            initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _lineRenderer.SetPosition(0, new Vector2(initialMousePosition.x, initialMousePosition.y));
            _lineRenderer.SetPosition(1, new Vector2(initialMousePosition.x, initialMousePosition.y));
            _lineRenderer.SetPosition(2, new Vector2(initialMousePosition.x, initialMousePosition.y));
            _lineRenderer.SetPosition(3, new Vector2(initialMousePosition.x, initialMousePosition.y));

            Destroy(_theSelectionBox);
            _theSelectionBox = Instantiate(_selectionBoxPrefab, new Vector2(0, 0), Quaternion.identity, _selectionBoxPrefab.transform.parent);

            _boxColl = gameObject.AddComponent<BoxCollider2D>();
            _boxColl.isTrigger = true;
            _boxColl.offset = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }

        //when being held down
        if (Input.GetMouseButton(0) && _state == GameState.Selecting)
        {
            currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _lineRenderer.SetPosition(0, new Vector2(initialMousePosition.x, initialMousePosition.y));
            _lineRenderer.SetPosition(1, new Vector2(initialMousePosition.x, currentMousePosition.y));
            _lineRenderer.SetPosition(2, new Vector2(currentMousePosition.x, currentMousePosition.y));
            _lineRenderer.SetPosition(3, new Vector2(currentMousePosition.x, initialMousePosition.y));

            transform.position = (currentMousePosition + initialMousePosition) / 2;

            Vector2 boxSize = new Vector2(Math.Abs(initialMousePosition.x - currentMousePosition.x) * 10, Math.Abs(initialMousePosition.y - currentMousePosition.y) * 10);
            Vector2 boxPosition = new Vector2((initialMousePosition.x + currentMousePosition.x) / 2f, (initialMousePosition.y + currentMousePosition.y) / 2f);

            if (_theSelectionBox == null)
            {
                _theSelectionBox = Instantiate(_selectionBoxPrefab, boxSize, Quaternion.identity);
            }
            _theSelectionBox.gameObject.transform.SetPositionAndRotation(boxPosition, Quaternion.identity);
            _theSelectionBox.gameObject.transform.localScale = boxSize;
            _boxColl.size = new Vector2(
            Mathf.Abs(initialMousePosition.x - currentMousePosition.x),
            Mathf.Abs(initialMousePosition.y - currentMousePosition.y));
        }

        if (Input.GetMouseButtonUp(0) && _state == GameState.Selecting)
        {
            HandleUnclick();
            ChangeState(GameState.WaitingInput);
        }
    }

    private void HandleUnclick()
    {
        if (IsAMatch())
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = _selection;
            audio.Play();
            _totalApples += _selectedApples.Count;
            _scoreText.text = _totalApples.ToString();
            
            OnMoveCompleted?.Invoke();
            
            for (int i = 0; i < _selectedApples.Count; i++)
            {
                var apple = _selectedApples[i];
                apple.OnClear();
            }
            _selectedApples.Clear();
        }
        _lineRenderer.positionCount = 0;
        Destroy(_boxColl);
        transform.position = Vector3.zero;
        Destroy(_theSelectionBox.gameObject);
        _theSelectionBox = null;
    }

    private void InitializeLevel()
    {
        _levelText.text = _level.ToString();
        switch(_level)
        {
            case 1:
                targetTime = 10f;
                SpawnApples(6, 10);
                break;
            case 2:
                targetTime = 60f;
                SpawnApples(7, 12);
                break;
        }
        

        ChangeState(GameState.WaitingInput);
    }

    private void SpawnApples(int width, int height)
    {
        _apples = new List<List<Apple>>();
        for (int x = 0; x < width; x++)
        {
            var temp = new List<Apple>();
            for (int y = 0; y < height; y++)
            {
                //AppleSelectionOutline outline = Instantiate(_appleSelectionOutlinePrefab, new Vector2(x, y - 0.07f), Quaternion.identity);
                //outline.gameObject.SetActive(false);
                var node = Instantiate(_applePrefab, new Vector2(x, y), Quaternion.identity);
                node.Init(UnityEngine.Random.Range(1, 10));
                temp.Add(node);
            }
            _apples.Add(temp);
        }

        var center = new Vector2((float)width / 2 - 0.5f, (float)height / 2 - 0.5f);

        Camera.main.transform.position = new Vector3(center.x, center.y + 0.55f, -10);
        _background.transform.position = new Vector2(center.x, center.y);
    }

    private void UpdateLevel()
    {
        DestroyApples();
        _level++;
        if (_level == _finalLevel)
        {
            EndGame();
        }
        else
        {
            ChangeState(GameState.GenerateLevel);
        }
    }

    private void EndGame()
    {

    }

    public void SelectApple(Apple apple)
    {
        if (!_selectedApples.Contains(apple))
        {
            _selectedApples.Add(apple);
        }
        ToggleSelectionColor();
    }

    public void DeselectApple(Apple apple)
    {
        if (_selectedApples.Contains(apple))
        {
            _selectedApples.Remove(apple);
        }
        ToggleSelectionColor();
    }

    private void ToggleSelectionColor()
    {
        bool isAMatch = IsAMatch();
        if (_theSelectionBox != null)
        {
            _theSelectionBox.UpdateSelectionColor(isAMatch);
        }
    }

    private bool IsAMatch()
    {
        int valueCount = 0;
        foreach (Apple apple in _selectedApples)
        {
            valueCount += apple.Value;
        }
        if (valueCount == 10)
        {
            return true;
        }
        return false;
    }

    private void DestroyApples()
    {
        foreach(List<Apple> row in _apples)
        {
            foreach(Apple apple in row)
            {
                //apple.TotalDestroy();
                apple.AnimateDelete();
            }
        }
        _apples.Clear();
    }
}

public enum GameState
{
    GenerateLevel,
    WaitingInput,
    Selecting,
    End
}
