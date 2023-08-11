﻿using System.Windows.Controls;

using MahApps.Metro.Controls;

using SDRDBE.Behaviors;

namespace SDRDBE.Contracts.Views;

public interface IShellWindow
{
    Frame GetNavigationFrame();

    void ShowWindow();

    void CloseWindow();

    Frame GetRightPaneFrame();

    SplitView GetSplitView();

    RibbonTabsBehavior GetRibbonTabsBehavior();
}
