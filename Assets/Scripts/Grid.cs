using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask; //à lier à un layer et à faire collisionner pour définir des zones intraversables
    public Vector2 gridWorldSize; //la taille de la grille
    public float nodeRadius; //le rayon des noeuds
    public List<Node> path; //le tableau contenant le chemin final

    Node[,] grid; //un tableau qui contient les informations de tous les noeuds
    int gridSizeX, gridSizeY; //le nombre de colonnes de la grille
    float nodeDiameter; //le diamètre des noeuds

    //lancement du code
    private void Start()
    {
        //on calcule le diamètre des noeuds
        nodeDiameter = nodeRadius * 2;
        //on calcule le nombre de colonnes qui composeront la grille
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
        //on appelle la fonction CreateGrid
        CreateGrid();
    }

    //Création de la grille et des noeuds qui la composent
    void CreateGrid()
    {
        //on attribue à notre tableau le nombre de colonnes qui composeront la grille
        grid = new Node[gridSizeX,gridSizeY];

        //on obtient la position du coin inférieur gauche de la zone définie
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        //on crée une boucle qui a autant d'itérations qu'il existera de noeuds dans la grille
        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                //à chaque itération on définit une nouvelle position pour un noeud
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                //si sur le layer unwalkableMask aucune collision n'est détectée à cette position, le noeud est navigable
                bool navigable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                //on stocke alors les coordonnées du nouveau noeud et on le définit comme navigable ou pas, puis la boucle recommence
                grid[x, y] = new Node(navigable, worldPoint, x, y);
            }
        }
        //à la fin de cette boucle la grille a été créée
    }
    
    //recherche des voisins d'un noeud
    public List<Node> GetNeighbours(Node node)
    {
        //on crée une nouvelle liste de voisins et pas un tableau pour pouvoir rajouter et supprimer des éléments
        List<Node> neighbours = new List<Node>();
        
        //on crée une boucle qui va itérer neuf fois (-1x,-1y à 1x,1x)
        //la cinquième itération (0x,0y) va reconnaitre le noeud actuel et l'ignorer
        //les huit autres vont identifier un noeud voisin
        //si un voisin est situé hors de la grille on l'ignore
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                
                //pour déterminer la position d'un voisin...
                int checkX = node.gridX + x; //...on ajoute la valeur x de la boucle à la position X du noeud actuel
                int checkY = node.gridY + y; //...on ajoute la valeur y de la boucle à la position Y du noeud actuel

                //si les valeurs obtenues indiquent que le voisin est situé hors de la grille, on passe à un autre voisin
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    //sinon on cherche dans le tableau contenant tous nos noeuds celui situé aux coordonnées indiqués par nos valeurs et on l'ajoute à notre liste de voisins
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        //notre liste contient à présent tous les noeuds voisins et on peut la retourner
        return neighbours;
    }

    //passage d'une coordonnée Vector3 à une position sur la grille
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        //on récupère les coordonnées X et Y d'un Vector3 placé sur la grille et on les convertit en deux pourcentages compris entre 0 et 1
        float percentX = (worldPosition.x / gridWorldSize.x) + 0.5f;
        float percentY = (worldPosition.z / gridWorldSize.y) + 0.5f;
        //on empêche aux valeurs de dépasser 0 et 1
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        //on multiple ces pourcentages par le nombre de noeuds moins un dans une colonne et on convertit le résultat en nombres entiers
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        //on attribue ces nombres entiers à une position sur la grille
        return grid[x, y];
    }

    //Visualisation de la grille
    private void OnDrawGizmos()
    {
        //on crée un aperçu des limites de la grille
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        //si une grille est présente...
        if (grid != null)
        {
            //...on crée un gizmo pour chacun de ses noeuds
            foreach (Node n in grid)
            {
                //on attribue des couleurs différentes aux noeuds navigables et non-navigables
                Gizmos.color = (n.walkable) ? Color.white : Color.red;

                //si on a un chemin...
                if (path != null) {
                    //et qu'il contient des noeuds
                    if (path.Contains(n))
                    {
                        //on attribue la couleur noire à ces noeuds
                        Gizmos.color = Color.black;
                    }
                }

                //on fait s'afficher un cube pour chaque noeud
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}
