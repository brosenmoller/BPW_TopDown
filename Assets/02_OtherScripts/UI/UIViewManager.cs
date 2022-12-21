// Adapted from Code by Ralf Zeilstra (Game Developer HKU Year 1 in 2022)

using System.Collections.Generic;
using UnityEngine;
using System;

public class UIViewManager : Singleton<UIViewManager>
{
    [Header("UI View Settings")]
    [SerializeField] private UIView startingView;
    [SerializeField] private UIView[] views;

    private readonly Dictionary<Type, UIView> viewsDictionary = new();

    private UIView currentView;
    private readonly Stack<UIView> history = new();

    private void Start()
    {
        foreach (UIView view in views)
        {
            view.Initialize();
            view.Hide();
            viewsDictionary.Add(view.GetType(), view);
        }

        if (startingView != null)
        {
            Show(startingView.GetType(), true);
        }
    }

    public UIView GetView(Type viewType)
    {
        if (!viewsDictionary.ContainsKey(viewType)) { return null; }
        
        return viewsDictionary[viewType];
    }

    public void Show(Type viewType, bool remember = true)
    {
        if (!viewsDictionary.ContainsKey(viewType)) { return; }

        if (currentView != null)
        {
            if (remember) { history.Push(currentView); }

            currentView.Hide();
        }

        currentView = viewsDictionary[viewType];
        currentView.Show();
    }

    public void ShowLast()
    {
        if (history.Count != 0)
        {
            Show(history.Pop().GetType(), false);
        }
    }
}