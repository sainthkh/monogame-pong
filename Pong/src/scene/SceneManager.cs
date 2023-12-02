namespace mg_pong;

public static class SceneManager {
    private static Scene currentScene;

    public static Scene CurrentScene {
        get { return currentScene; }
    }

    public static void Load(SceneTypes sceneType) {
        switch (sceneType) {
            case SceneTypes.Lobby:
                currentScene = null;
                break;
            default:
                currentScene = null;
                break;
        }
    }

    public static void LoadLevel(int level) {
        switch(level) {
            case 1:
                currentScene = new Level1();
                break;
            case 2:
                currentScene = new Level2();
                break;
            case 3:
                currentScene = new Level3();
                break;
            case 4:
                currentScene = new Level4();
                break;
        }

        currentScene.Load();
    }
}