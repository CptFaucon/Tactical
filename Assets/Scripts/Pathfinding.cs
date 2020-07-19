using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target; //les transform de la position de départ et d'arrivée

    Grid grid; //un objet de la classe Grid
    
    //appel à toutes les frames de la fonction findPath avec les coordonnées de départ et d'arrivée
    private void Update()
    {
        FindPath(seeker.position, target.position);
    }

    //assignation de l'objet de la classe Grid
    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    //recherche du noeud d'arrivée depuis le noeud de départ
    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //on convertit les coordonnées de départ et d'arrivée en positions sur la grille avec la fonction NodeFromWorldPoint
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        //création de la liste OPEN contenant les noeuds identifiés
        List<Node> openSet = new List<Node>();
        //création de la liste CLOSED contenant les noeuds déjà passés en revue
        HashSet<Node> closedSet = new HashSet<Node>();
        //on ajoute le noeud de départ comme premier élément de la liste OPEN
        openSet.Add(startNode);

        //on crée une boucle qui tourne tant qu'il y a des noeuds dans la liste OPEN
        while(openSet.Count > 0)
        {
            //on crée le noeud currentNode qui va servir à récolter les informations sur le trajet
            //on lui attribue les valeurs du premier élément de la liste OPEN
            Node currentNode = openSet[0];

            //on crée une seconde boucle qui parcourt toute la liste OPEN
            //on la fait partir du seconde noeud car on ne veut pas qu'elle itère pour notre noeud de départ
            for(int i = 1; i < openSet.Count; i++)
            {
                //on sélectionne le noeud qui a le coût en F le plus bas ou, en cas d'égalité, le coût en H le plus bas
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.fCost)
                {
                    //currentNode récupère alors les valeurs de ce noeud
                    currentNode = openSet[i];
                }
            }

            //on retire de la liste OPEN le noeud attribué à currentNode
            openSet.Remove(currentNode);
            //on ajoute celui-ci à CLOSED
            closedSet.Add(currentNode);

            //si currentNode est arrivé jusqu'au noeud cible...
            if(currentNode == targetNode)
            {
                //...on appelle la fonction RetracePath avec les positions du noeud de départ et d'arrivée
                RetracePath(startNode, targetNode);
                //...et on arrête la fonction ici
                return;
            }

            //sinon on appelle la fonction GetNeighbours avec les informations de currentNode
            //on récupère ainsi par neighbour les noeuds de tous ses voisins
            //cette boucle itère autant de fois qu'il y a de voisins, 8 étant le nombre standard
            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                //si le voisins actuel est intraversable ou qu'il est dans la liste CLOSED on l'ignore et on passe à un autre voisin
                if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                    continue;
                }

                //on calcule le coût en G du voisin en additionnant le coût en G du noeud actuel et le coût de la distance qui les séparent
                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                
                //si le coût en G du voisin est inférieur à son coût précédent, ou que le voisin n'est pas contenu dans la liste OPEN...
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    //...on assigne le nouveau coût en G
                    neighbour.gCost = newMovementCostToNeighbour;
                    //...on calcule son coût en H avec la fonction GetDistance
                    neighbour.hCost = GetDistance(currentNode, targetNode);
                    //...on définit currentNode comme son parent
                    neighbour.parent = currentNode;

                    //et si le voisin n'est pas contenu dans la liste OPEN
                    if (!openSet.Contains(neighbour))
                    {
                        //...on l'ajoute à la liste OPEN
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }
    
    //création du chemin final
    void RetracePath(Node startNode, Node endNode)
    {
        //on crée la liste qui contiendra les noeuds du chemin final
        List<Node> path = new List<Node>();
        //on crée un nouveau noeud auquel on assigne le noeud d'arrivée
        Node currentNode = endNode;

        //on crée une boucle qui tourne tant que le noeud n'est pas égal au noeud de départ
        //on va remonter la liste des noeuds parents dont le coût en F est le plus faible
        //on obtien ainsi le trajet le moins coûteux du point d'arrivée au point de départ
        while(currentNode != startNode)
        {
            //on ajoute le noeud à la liste
            path.Add(currentNode);
            //on lui assigne les valeurs de son parent
            currentNode = currentNode.parent;
        }
        //une fois la boucle terminée tous les parents ont été récupérés
        //on inverse l'ordre des éléments de la liste pour les positionner du départ à l'arrivée
        path.Reverse();
        //le chemin final est récupéré par un autre tableau de la classe Grid
        grid.path = path;
    }
    
    //calcul de la distance entre deux noeuds
    int GetDistance (Node nodeA, Node nodeB)
    {
        //on calcule la distance absolue en X et Y des deux noeuds référencés
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        //dans le calcul, un déplacement en diagonale a un poids de 14 et un déplacement horizontal ou vertical un poids de 10
        //Si la distance absolue en X est plus grande que celle en Y...
        if (dstX > dstY)
        {
            //...on multiplie Y par 14, c'est le nombre de diagonales à parcourir
            //...on multiplie (X - Y) par 10, c'est la distance horizontale ou verticale à parcourir
            //...on additionne les valeurs et on retourne le résultat
            return 14 * dstY + 10 * (dstX - dstY);
        }
        //on calcule l'inverse si la distance en Y est supérieure à celle en X
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
