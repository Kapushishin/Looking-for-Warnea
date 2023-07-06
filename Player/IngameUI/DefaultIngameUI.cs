using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

public class DefaultIngameUI : MonoBehaviour, IIngameUI
{
    [SerializeField] UIDocument resourcesPanel;
    [SerializeField] UIDocument statsPanel;
    [SerializeField] UIDocument minimapPanel;
    [SerializeField] UIDocument actionPanel;
    [SerializeField] UIDocument buildingsPanel;

    private VisualElement resourcesRoot;
    private VisualElement statsRoot;
    private VisualElement minimapRoot;
    private VisualElement actionsRoot;
    private VisualElement buildingsRoot;


    private Label _nameUI;
    private Label _healthUI;
    private Label _damageUI;
    private Label _armorUI;
    private Label _levelUI;

    private Button _minimapUI;

    [SerializeField] private CinemachineVirtualCamera _playerCamera;
    [SerializeField] private GameObject _minimapFrame;
    [SerializeField] private GameObject _targetCamera;
    [SerializeField] private GameObject _terrain;

    private Button _action1;
    private Button _action2;
    private Button _action3;
    private Button _action4;

    private Label _woodUI;
    private Label _ironUI;
    private Label _foodUI;
    private Label _villagersUI;
    private Label _goldUI;

    private void Start()
    {
        resourcesRoot = resourcesPanel.rootVisualElement;
        statsRoot = statsPanel.rootVisualElement;
        minimapRoot = minimapPanel.rootVisualElement;
        actionsRoot = actionPanel.rootVisualElement;
        buildingsRoot = buildingsPanel.rootVisualElement;

        _woodUI = resourcesRoot.Q<Label>("WoodValue");
        _ironUI = resourcesRoot.Q<Label>("IronValue");
        _foodUI = resourcesRoot.Q<Label>("FoodValue");
        _villagersUI = resourcesRoot.Q<Label>("PeopleValue");
        _goldUI = resourcesRoot.Q<Label>("GoldValue");

        _nameUI = statsRoot.Q<Label>("Name");
        _healthUI = statsRoot.Q<Label>("Health");
        _damageUI = statsRoot.Q<Label>("Damage");
        _armorUI = statsRoot.Q<Label>("Armor");
        _levelUI = statsRoot.Q<Label>("Level");

        _minimapUI = minimapRoot.Q<Button>("MiniMap");
        _minimapUI.clicked += SetMiniMap;

        _action1 = actionsRoot.Q<Button>("Action1");
        _action2 = actionsRoot.Q<Button>("Action2");
        _action3 = actionsRoot.Q<Button>("Action3");
        _action4 = actionsRoot.Q<Button>("Action4");
    }

    public void SetStatsPanel(string name, float currentHealth, float maxHealth, float damage, float armor)
    {
        _nameUI.text = name;
        _healthUI.text = currentHealth + "/" + maxHealth;
        _damageUI.text = damage.ToString();
        _armorUI.text = armor.ToString();
        _levelUI.text = ((maxHealth + damage + armor) / 13).ToString();
    }

    public void SetActionsPanel()
    {
        throw new System.NotImplementedException();
    }

    public void SetBuildingActions()
    {
        throw new System.NotImplementedException();
    }

    public void SetMiniMap()
    {
        _playerCamera.Follow = _targetCamera.transform;
        _playerCamera.LookAt = _targetCamera.transform;

        Vector2 mouse = Input.mousePosition;
        //Vector2 minimapSize = new Vector2 (Screen.width * 0.15f, Screen.height * 0.2f);

        Vector2 mapsize = new Vector2(500f, 500f);

        float minimapPosX = Screen.width * 0.85f;
        float minimapSizeX = Screen.width - minimapPosX;
        float minimapSizeY = Screen.height * 0.2f;
        Debug.Log(minimapSizeY);

        float X = minimapPosX - minimapSizeX / 2;
        float Y = minimapSizeY / 2;



        //float kScaleX = mapsize / minimapSize.x;
        //float kScaleY = mapsize.y / minimapSize.y;

        Vector2 minimapCenter = new Vector2(Screen.width * 0.15f / 2, Screen.height * 0.2f / 2);

        Vector3 curPos = new Vector3(mouse.x - X, mouse.y - Y, 0);

        X = minimapSizeX;
        Y = 0;

        Vector3 pos = new Vector3((curPos.x - X), 0, (curPos.y - Y));
        _targetCamera.transform.position = pos;

        //Debug.Log(Screen.width - minimapPosX);

        /*_targetCamera.transform.position = 
            new Vector3(-(minimapCenter.x - mouse.x/2 * 0.15f) * kScaleX,
            _targetCamera.transform.position.y,
            (minimapCenter.y - (Screen.height - mouse.y)/2 * 0.2f) * kScaleY);*/

        /*_targetCamera.transform.position = new Vector3(- (Screen.width - mouse.x - minimapCenter.x),
            _targetCamera.transform.position.y, 
            (mouse.y - minimapCenter.y));*/

        //Debug.Log("mouse x = " + mouse.x + "Screen.width = " + Screen.width);
        //Debug.Log("mouse y = " + mouse.y + "Screen.height = " + Screen.height);
        //Debug.Log("minimapCenter.x = " + minimapCenter.x + "minimapCenter.y" + minimapCenter.y);


        /*_minimapFrame.transform.position =
            new Vector3(-(minimapCenter.x - mouse.x / 2 * 0.15f) * kScaleX,
            _minimapFrame.transform.position.y,
           (minimapCenter.y - (Screen.height - mouse.y) / 2 * 0.2f) * kScaleY);*/

    }

    public void SetResources()
    {
        throw new System.NotImplementedException();
    }
}
