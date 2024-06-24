using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("[REFERENCES] Scriptable Objects")]
    [SerializeField] public PlayerInputActions playerInput;
    [SerializeField] public ResourceStats resourceStats;
    [SerializeField] public PowerNodeStats powerNodeStats;
    [SerializeField] public UIStats UIStats;
    [SerializeField] public GameState gameState;

    [Header("[REFERENCES] build phase UIs")]
    [SerializeField] public GameObject[] buildPhaseUIs;
    [SerializeField] public GameObject _waveStartConfirmationUI;
    

    [Header("[REFERENCES] Resource/Upkeep UI")]
    [SerializeField] public TMP_Text _resourcesTxt;
    [SerializeField] public TMP_Text _maxUpkeepTxt;
    [SerializeField] public TMP_Text _upkeepTxt;

    [Header("[REFERENCES] UI Buttons")]
    [SerializeField] public Button _startWaveButton;

    [Header("[DEBUG] private variables")]
    [SerializeField] private int _intResources;
    [SerializeField] private int _intUpkeep;
    [SerializeField] private int _intMaxUpkeep;

    private InputAction pause;

    // Start is called before the first frame update

    private void Awake()
    {
        _startWaveButton.interactable = true;
        _waveStartConfirmationUI.SetActive(false);
        playerInput = new PlayerInputActions();
        TryGetComponent<Canvas>(out UIStats.canvas);
    }

    private void OnEnable()
    {
        pause = playerInput.Gameplay.Pause;
        pause.Enable();
        pause.performed += PauseGame;
    }

    private void OnDisable()
    {
        pause.performed -= PauseGame;
        pause.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayInfo();

        if (gameState.BuildPhase)
        {
            _startWaveButton.interactable = true;
            ActivateBuildPhaseUI();
        }
        else
        {
            DeactivateBuildPhaseUI();
        }

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

    void PauseGame(InputAction.CallbackContext context)
    {
        gameState.SetPaused(!gameState.IsPaused());
    }

    public void StartWavePhase()
    {
        _waveStartConfirmationUI.SetActive(false);
        _startWaveButton.interactable = false;
        gameState.BuildPhase = false;
    }

    public void ReturnToBuildPhase()
    {
        _waveStartConfirmationUI.SetActive(false);
        _startWaveButton.interactable = true;
    }

    public void StartWaveButton()
    {
        _startWaveButton.interactable = false;
        _waveStartConfirmationUI.SetActive(true);
    }

    public void DeactivateBuildPhaseUI()
    {
        foreach (GameObject uiObject in buildPhaseUIs)
        {
            DisableIconScript(uiObject);
        }
    }    
    
    public void ActivateBuildPhaseUI()
    {
        foreach (GameObject uiObject in buildPhaseUIs)
        {
            EnableIconScript(uiObject);
        }
    }

    private void DisableIconScript(GameObject structureUI)
    {
        structureUI.GetComponent<IconScript>().enabled = false;
    }

    private void EnableIconScript(GameObject structureUI)
    {
        structureUI.GetComponent<IconScript>().enabled = true;
    }


}
