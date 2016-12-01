using UnityEngine;

public abstract class Page {
    protected GameObject prefab,
                       instance;

    private static string prefabsPath = "Prefabs/Pages/";

    protected string prefabName;

    public bool IsActive { get { return instance.activeSelf; } }

    public void Load() {
        prefab = Resources.Load<GameObject>(prefabsPath + prefabName);
        instance = Object.Instantiate(prefab);
        instance.transform.SetParent(UIManager.Canvas.transform, false);
    }

    public abstract void Init();

    public void Show() {
        instance.SetActive(true);
    }
    public void Hide() {
        instance.SetActive(false);
    }
}
