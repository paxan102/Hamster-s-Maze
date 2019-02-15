using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MazeGen mazeGen;
    [SerializeField] private CameraController cameraController;

    void Start()
    {
        mazeGen.Init();
        mazeGen.MakeMaze();
        cameraController.SetupMazeCharacteristics(mazeGen.GetMazeHeight(), mazeGen.GetMazeWidth());
    }

    private void Update()
    {
        
    }
}
