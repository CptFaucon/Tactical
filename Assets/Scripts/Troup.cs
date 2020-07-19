using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
//comportement des troupes
public class Troup : MonoBehaviour
{
    public bool isEnemy; //est-ce un allié ou un ennemi

    Pathfinding pathfinding; //référence à Pathfinding
    TroupData troupData; //référence à troupData

    Transform troupPosition; //position de l'unité
    SpriteRenderer troupSprite; //texture de l'unité

    private void Start()
    {
        //on récupère toutes les références
        pathfinding = GetComponentInParent<Pathfinding>();
        troupData = GetComponentInParent<TroupData>();
        troupSprite = GetComponent<SpriteRenderer>();

        //si l'unité est un ennemi
        if (isEnemy == true)
        {
            //on lui assigne la position d'arrivée
            troupPosition = pathfinding.target;
            //on lui assigne la texture d'un ennemi
            troupSprite.sprite = troupData.EnemyTroup;
        }

        //si l'unité est alliée
        else
        {
            //on lui assigne la position de départ
            troupPosition = pathfinding.seeker;
            //on lui assigne la texture d'un allié
            troupSprite.sprite = troupData.AllyTroup;
        }
        //on réassigne la position de l'unité
        transform.position = troupPosition.position;
    }
}
