using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // VARIABLES DE BASE
    public GameObject blockPrefab;
    public int gridWidth = 6;
    public int gridHeight = 6;
    public float blockSpacing = 1.2f;
    public List<Sprite> blockSprites;

    private GameObject[,] grid;
    private GameObject selectedBlock; // Variable pour stocker le bloc sélectionné
    private Vector3 mouseStartPos;
    private bool isDragging = false;
    public Vector3 gridStartPosition = new Vector3(-3.0f, -5.0f, 0.0f); // Ajuster selon tes préférences


    void Start()
    {
        GenerateGrid(); 
        Debug.Log("Grid generated successfully.");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collider = Physics2D.OverlapPoint(mouseStartPos);
            Debug.Log("Mouse Clicked at: " + mouseStartPos);

            if (collider != null)
            {
                Debug.Log("Collider Detected on: " + collider.gameObject.name);
                selectedBlock = collider.gameObject;
                Debug.Log("Selected Block at: " + selectedBlock.transform.position);
                isDragging = true;
            }
            else
            {
                Debug.Log("No Collider Detected");
            }
        }

        // Utilise les touches directionnelles pour indiquer la direction
        if (selectedBlock != null && Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Right Arrow Key Pressed");
            StartCoroutine(SlideBlock(selectedBlock, Vector3.right));
        }
        if (selectedBlock != null && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Left Arrow Key Pressed");
            StartCoroutine(SlideBlock(selectedBlock, Vector3.left));
        }
        if (selectedBlock != null && Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("Up Arrow Key Pressed");
            StartCoroutine(SlideBlock(selectedBlock, Vector3.up));
        }
        if (selectedBlock != null && Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("Down Arrow Key Pressed");
            StartCoroutine(SlideBlock(selectedBlock, Vector3.down));
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            selectedBlock = null;
        }
    }

    void GenerateGrid()
    {
        grid = new GameObject[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 position = gridStartPosition + new Vector3(x * blockSpacing, y * blockSpacing, 0);
                GameObject block = Instantiate(blockPrefab, position, Quaternion.identity);
                grid[x, y] = block;

                if (blockSprites.Count > 0)
                {
                    Sprite randomSprite = blockSprites[Random.Range(0, blockSprites.Count)];
                    block.GetComponent<Block>().SetSprite(randomSprite);
                    block.GetComponent<Block>().SetTargetPosition(position);
                    Debug.Log($"Bloc créé à : {position} avec le sprite : {randomSprite.name}");
                }
                else
                {
                    Debug.LogError("La liste de sprites est vide !");
                }
            }
        }
    }




    private IEnumerator SlideBlock(GameObject block, Vector3 direction)
    {
        Vector3 startPos = block.transform.position;
        Vector3 endPos = startPos + direction * blockSpacing;

        int startX = Mathf.RoundToInt((startPos.x - gridStartPosition.x) / blockSpacing);
        int startY = Mathf.RoundToInt((startPos.y - gridStartPosition.y) / blockSpacing);
        int endX = Mathf.RoundToInt((endPos.x - gridStartPosition.x) / blockSpacing);
        int endY = Mathf.RoundToInt((endPos.y - gridStartPosition.y) / blockSpacing);

        if (endX >= 0 && endX < gridWidth && endY >= 0 && endY < gridHeight)
        {
            GameObject adjacentBlock = grid[endX, endY];

            grid[startX, startY] = adjacentBlock;
            grid[endX, endY] = block;

            Vector3 blockPos = block.transform.position;
            Vector3 adjacentBlockPos = adjacentBlock.transform.position;

            block.GetComponent<Block>().SetTargetPosition(adjacentBlockPos);
            adjacentBlock.GetComponent<Block>().SetTargetPosition(blockPos);

            yield return new WaitForSeconds(0.5f); // Attendre pour l'animation

            if (block != null && adjacentBlock != null)
            {
                CheckForMatches();
            }

            CenterBlocks(); // Recentre les blocs
        }
        else
        {
            Debug.LogWarning($"Invalid move - out of grid bounds: ({endX}, {endY})");
        }
    }





    private void CheckForMatches()
    {
        List<GameObject> matchedBlocks = new List<GameObject>();

        // Vérifie les alignements horizontaux
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth - 2; x++)
            {
                GameObject block1 = grid[x, y];
                GameObject block2 = grid[x + 1, y];
                GameObject block3 = grid[x + 2, y];

                if (block1 != null && block2 != null && block3 != null &&
                    block1.GetComponent<SpriteRenderer>().sprite == block2.GetComponent<SpriteRenderer>().sprite &&
                    block2.GetComponent<SpriteRenderer>().sprite == block3.GetComponent<SpriteRenderer>().sprite)
                {
                    matchedBlocks.Add(block1);
                    matchedBlocks.Add(block2);
                    matchedBlocks.Add(block3);
                }
            }
        }

        // Vérifie les alignements verticaux
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight - 2; y++)
            {
                GameObject block1 = grid[x, y];
                GameObject block2 = grid[x, y + 1];
                GameObject block3 = grid[x, y + 2];

                if (block1 != null && block2 != null && block3 != null &&
                    block1.GetComponent<SpriteRenderer>().sprite == block2.GetComponent<SpriteRenderer>().sprite &&
                    block2.GetComponent<SpriteRenderer>().sprite == block3.GetComponent<SpriteRenderer>().sprite)
                {
                    matchedBlocks.Add(block1);
                    matchedBlocks.Add(block2);
                    matchedBlocks.Add(block3);
                }
            }
        }

        // Supprime les blocs correspondants
        foreach (GameObject block in matchedBlocks)
        {
            if (block != null)
            {
                int xIndex = Mathf.RoundToInt((block.transform.position.x - gridStartPosition.x) / blockSpacing);
                int yIndex = Mathf.RoundToInt((block.transform.position.y - gridStartPosition.y) / blockSpacing);

                if (xIndex >= 0 && xIndex < gridWidth && yIndex >= 0 && yIndex < gridHeight)
                {
                    grid[xIndex, yIndex] = null;
                }

                Destroy(block);
            }
        }

        Debug.Log("Match found and blocks destroyed: " + matchedBlocks.Count);

        // Remplir les espaces vides avec de nouveaux blocs
        StartCoroutine(FillEmptySpaces());
    }


    private IEnumerator FillEmptySpaces()
    {
        bool filledAnySpace;
        do
        {
            filledAnySpace = false;
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    if (grid[x, y] == null)
                    {
                        // Faire tomber les blocs au-dessus pour remplir l'espace vide
                        for (int yAbove = y + 1; yAbove < gridHeight; yAbove++)
                        {
                            if (grid[x, yAbove] != null)
                            {
                                grid[x, y] = grid[x, yAbove];
                                grid[x, yAbove] = null;
                                grid[x, y].GetComponent<Block>().SetTargetPosition(new Vector3(x * blockSpacing + gridStartPosition.x, y * blockSpacing + gridStartPosition.y, 0));
                                filledAnySpace = true;
                                break;
                            }
                        }

                        // Créer un nouveau bloc en haut de la grille si l'espace est vide
                        if (grid[x, y] == null)
                        {
                            for (int topY = gridHeight - 1; topY >= 0; topY--)
                            {
                                if (grid[x, topY] == null)
                                {
                                    Vector3 position = new Vector3(x * blockSpacing + gridStartPosition.x, topY * blockSpacing + gridStartPosition.y, 0);
                                    GameObject newBlock = Instantiate(blockPrefab, position, Quaternion.identity);
                                    Sprite randomSprite = blockSprites[Random.Range(0, blockSprites.Count)];
                                    newBlock.GetComponent<Block>().SetSprite(randomSprite);
                                    grid[x, topY] = newBlock;
                                    filledAnySpace = true;
                                    Debug.Log($"New block created at ({x}, {topY})");
                                }
                            }
                        }
                    }
                }
            }

            yield return new WaitForEndOfFrame();
        } while (filledAnySpace);

        CenterBlocks(); // Recentre les blocs
        Debug.Log("Finished filling empty spaces");
    }





    private bool IsPositionOccupied(Vector3 position)
    {
        foreach (GameObject block in grid)
        {
            if (block != null && block.transform.position == position)
            {
                return true;
            }
        }
        return false;
    }




    private void CenterBlocks()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] != null)
                {
                    Vector3 correctPosition = gridStartPosition + new Vector3(x * blockSpacing, y * blockSpacing, 0);
                    grid[x, y].GetComponent<Block>().SetTargetPosition(correctPosition);
                }
            }
        }
    }







}
