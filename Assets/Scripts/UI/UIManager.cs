using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [Header("[REFERENCES]")]
    [SerializeField] public PlayerInputActions playerInput;
    [SerializeField] public ResourceStats resourceStats;
    [SerializeField] public PowerNodeStats powerNodeStats;
    [SerializeField] public UIStats UIStats;
    [SerializeField] public GameState gameState;
    [SerializeField] public TMP_Text _resourcesTxt;
    [SerializeField] public TMP_Text _maxUpkeepTxt;
    [SerializeField] public TMP_Text _upkeepTxt;

    [Header("[DEBUG] private variables")]
    [SerializeField] private int _intResources;
    [SerializeField] private int _intUpkeep;
    [SerializeField] private int _intMaxUpkeep;

    private InputAction pause;

    // Start is called before the first frame update

    private void Awake()
    {
        playerInput = new PlayerInputActions();
        TryGetComponent<Canvas>(out UIStats.canvas);
    }

    private void OnEnable()
    {
        pause = playerInput.Gameplay.Pause;
        pause.Enable();
        pause.performed += PauseCommand;
    }

    private void OnDisable()
    {
        pause.performed -= PauseCommand;
        pause.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayInfo();
    }

    void DisplayInfo()
    {
        _intUpkeep = Mathf.RoundToInt(powerNodeStats.GetUpkeep());
        _intMaxUpkeep = Mathf.RoundToInt(powerNodeStats.GetMaxUpkeep());
        _intResources = Mathf.RoundToInt(resourceStats.GetTotalResources());

        _upkeepTxt.text = _intUpkeep.ToString();
        _maxUpkeepTxt.text = _intMaxUpkeep.ToString();
        _resourcesTxt.text = _intResources.ToString();

        if (_intUpkeep >= 999)
        {
            _upkeepTxt.text = "999";
        }        
        if (_intMaxUpkeep >= 999)
        {
            _maxUpkeepTxt.text = "999";
        }
        if (_intResources >= 9999999)
        {
            _resourcesTxt.text = "9999999";
        }
    }

    void PauseCommand(InputAction.CallbackContext context)
    {
        gameState.SetPaused(!gameState.IsPaused());
    }


}
