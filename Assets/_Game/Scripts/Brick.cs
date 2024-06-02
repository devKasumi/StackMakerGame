using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private bool isBridgeBrick;

    private MeshRenderer brickMeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        brickMeshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (isBridgeBrick)
            {
                if (!brickMeshRenderer.enabled)
                {
                    brickMeshRenderer.enabled = true;
                    player.RemoveBrick();
                    LevelManager.GetInstance.AddBrickForBridge(this.gameObject.GetComponent<Brick>());
                    if (LevelManager.GetInstance.NotEnoughBrickBridge() && player.GetListBrickCount() == 0)
                    {
                        player.StopMoving(this.gameObject.GetComponent<Brick>());
                        player.FinishLevel();
                        UIManager.GetInstance.ShowLoseUI();
                    }
                }
            }
            else
            {
                player.AddBrick(this.gameObject.GetComponent<Brick>());
            }      
        }
    }
}
