using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]

public class GameManager : MonoBehaviour
{
    
    [SerializeField] private Apple _applePrefab;
    [SerializeField] private Apple _changingApplePrefab;
    [SerializeField] private Apple _halvingApplePrefab;
    [SerializeField] private Apple _negativeApplePrefab;

    [SerializeField] private AppleSelectionOutline _appleSelectionOutlinePrefab;
    [SerializeField] private SelectionBox _selectionBoxPrefab;
    [SerializeField] private SpriteRenderer _background;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Camera _camera;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private AudioClip _selection;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _levelProgressText;

    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] Vector2 initialMousePosition, currentMousePosition;
    private BoxCollider2D _boxColl;
    private SelectionBox _theSelectionBox;
    private List<Apple> _selectedApples = new List<Apple>();
    private List<List<Apple>> _apples;
    private int _totalApples = 0;


    private int applesClearedInLevel = 0;

    public GameState _state;
    public float targetTime = 60.0f;

    public List<Level> levels;
    private Level currentLevelSettings;
    [SerializeField] private int _level = 0;

    public static event Action OnMoveCompleted;

    void Awake()
    {
        currentLevelSettings = levels[_level];
    }

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
                EndGame();
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

    private void UpdateScoreText()
    {
        _levelProgressText.text = applesClearedInLevel.ToString() + "/" + currentLevelSettings.requiredToClear.ToString();
        _scoreText.text = _totalApples.ToString();
    }

    private void HandleUnclick()
    {
        if (IsAMatch())
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = _selection;
            audio.Play();
            
            _totalApples += _selectedApples.Count;
            applesClearedInLevel += _selectedApples.Count;


            UpdateScoreText();
            
            OnMoveCompleted?.Invoke();
            

            for (int i = 0; i < _selectedApples.Count; i++)
            {
                var apple = _selectedApples[i];
                apple.OnClear();
            }
            _selectedApples.Clear();
            
            if (applesClearedInLevel >= currentLevelSettings.requiredToClear)
            {
                UpdateLevel();
            }

        }
        _lineRenderer.positionCount = 0;
        Destroy(_boxColl);
        transform.position = Vector3.zero;
        Destroy(_theSelectionBox.gameObject);
        _theSelectionBox = null;
    }

    private void InitializeLevel()
    {
        _levelText.text = (_level+1).ToString();
        applesClearedInLevel = 0;
        SpawnApples(currentLevelSettings.width, currentLevelSettings.height);
        UpdateScoreText();
        ChangeState(GameState.WaitingInput);
    }

    private void UpdateLevel()
    {
        DestroyApples();
        _level++;

        if (_level < levels.Count)
        {
            currentLevelSettings = levels[_level];
        }
        targetTime += currentLevelSettings.additionalTime;
        ChangeState(GameState.GenerateLevel);

    }

    private Apple SelectRandomApplePrefab()
    {
        float roll = UnityEngine.Random.Range(0f, 1f);
        float chanceCounter = currentLevelSettings.changingAppleChance;
        if (chanceCounter > roll)
        {
            return _changingApplePrefab;
        }

        chanceCounter += currentLevelSettings.halvingAppleChance;
        if (chanceCounter > roll)
        {
            return _halvingApplePrefab;
        }

        chanceCounter += currentLevelSettings.negativeAppleChance;
        if (chanceCounter > roll)
        {
            return _negativeApplePrefab;
        }

        return _applePrefab;
    }

    private void SpawnApples(int width, int height)
    {
        _apples = new List<List<Apple>>();
        for (int x = 0; x < width; x++)
        {
            var temp = new List<Apple>();
            for (int y = 0; y < height; y++)
            {
                AppleSelectionOutline outline = Instantiate(_appleSelectionOutlinePrefab, new Vector2(x, y - 0.07f), Quaternion.identity);
                outline.gameObject.SetActive(false);

                Apple applePrefabToSpawn = SelectRandomApplePrefab();
                var node = Instantiate(applePrefabToSpawn, new Vector2(x, y), Quaternion.identity);
                node.Init(UnityEngine.Random.Range(1, 10), outline);
                temp.Add(node);
            }
            _apples.Add(temp);
        }

        var center = new Vector2((float)width / 2 - 0.5f, (float)height / 2 - 0.5f);

        Camera.main.transform.position = new Vector3(center.x, center.y + 0.55f, -10);
        _background.transform.position = new Vector2(center.x, center.y);
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
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

public enum GameState
{
    GenerateLevel,
    WaitingInput,
    Selecting,
    End
}
