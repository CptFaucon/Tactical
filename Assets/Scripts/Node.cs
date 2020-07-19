using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//les noeuds sont des objets de la classe Node
//elle définit les variables qu'ils doivent contenir, elle permet de créer de d'initialiser celles-ci et elle calcule le coût en F
public class Node
{
    public bool walkable; //navigabilité du noeud

    public Vector3 worldPosition; //position en Vector3 du noeud 

    public int gridX; //position X du noeud sur la grille
    public int gridY; //position y du noeud sur la grille

    public int gCost; //coût en G du noeud
    public int hCost; //coût en H du noeud
    
    public Node parent; //parent du noeud

    //constructeur d'instance qui permet de créer et d'initialiser des variables dans un noeud au moment de sa création
    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    //quand il est appelé, le coût en F du noeud retourne l'addition des coût en G et en H
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
