using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface IIngameUI
{
    public void SetStatsPanel(string _name, float _currentHealth, float _maxHealth, float _damage, float _armor);
    public void SetActionsPanel();
    public void SetMiniMap();
    public void SetResources();
    public void SetBuildingActions();
}
