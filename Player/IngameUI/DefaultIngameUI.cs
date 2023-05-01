using System.Collections;
using System.Collections.Generic;
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

    private VisualElement _minimapUI;

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

        _minimapUI = minimapRoot.Q<VisualElement>("MiniMap");

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
        throw new System.NotImplementedException();
    }

    public void SetResources()
    {
        throw new System.NotImplementedException();
    }
}
