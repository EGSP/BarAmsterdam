using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;


public class QuickTool : EditorWindow
{
    [MenuItem("QuickTool/Open")]
    public static void ShowWindow()
    {
        var window = GetWindow<QuickTool>();

        window.titleContent = new GUIContent("QuickTool window");

        window.minSize = new Vector2(250, 50);
    }

    private void OnEnable()
    {
        var root = rootVisualElement;

        root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Testing(Gasanov)/QuickTool_Style.uss"));

        var quickToolVisualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Testing(Gasanov)/QuickTool_Main.uxml");
        quickToolVisualTree.CloneTree(root);

        var toolButtons = root.Query<Button>();
        toolButtons.ForEach(SetupButton);
    }

    private void SetupButton(Button button)
    {
        var buttonIcon = button.Q(className: "quicktool-button-icon");

        var iconPath = "Assets/Sprites/Editor/" + button.parent.name + "Icon.png";

        var iconAsset = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);

        buttonIcon.style.backgroundImage = iconAsset.texture;

        button.tooltip = button.parent.name;
    }
}
