using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MazeGen mazeGen;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Player player;

    private void Start()
    {
        player = Instantiate(player);
        player.OnFinish.AddListener(HandleOnFinish);
        mazeGen.Init();

        Restart();
    }

    private void Restart()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Destroyable");
        if (objects.Length != 0)
            foreach (var obj in objects)
                Destroy(obj);

        mazeGen.MakeNewMaze();
        cameraController.SetupMazeCharacteristics(mazeGen.GetMazeHeight(), mazeGen.GetMazeWidth(), player.GetPivot());
        player.InitAndSpawn(mazeGen.GetCellsForPlayer(), mazeGen.GetStartDirection());
    }

    private void HandleOnFinish()
    {
        Restart();
    }
}
