public class Theory
{
    /* 
    coût en G = distance du noeud de départ
    coût en H = distance du noeud d'arrivée
    coût en F = coût en G + coût en H

    - Quand plusieurs coûts en F sont similaires, on prend le coût en H le plus bas pour continuer.
    - On choisit au hasard quand les coûts en G et en H sont similaires.
    - Le coût en G d'un noeud est revu à la baisse si un chemin plus court vers celui-ci est découvert.
    - Chaque noeud possède en mémoire son noeud d'origine.
    - L'origine d'un noeud est redéfinie si un chemin plus court vers celui-ci est découvert.


    Pseudo-code :

    On crée deux listes :
    - Une liste OPEN qui contient les noeuds dont on a calculé le coût en F.
    - Une liste CLOSED qui contient les noeuds qui ont été évalués.

    Dans le Start on ajoute le noeud de départ dans la liste OPEN. C'est alors le seul noeud présent dans cette liste. La liste CLOSED, elle, ne contient rien pour l'instant.
     
    On crée une boucle. Dans celle-ci on crée une variable temporaire appelée "current node". Elle correspond au noeud de la liste dont le coût en F est le plus faible. Ensuite, on retire cette variable de la liste OPEN et on la met dans la liste CLOSED. Si le "current node" est le noeud cible alors le chemin a été trouvé. Enfin, on sort de la boucle.

    Si le "current node" n'est pas le noeud cible on cherche tous les nodes voisins du "current node". Si un voisin n'est pas traversable ou qu'il est dans la liste CLOSED on passe au voisin suivant. Si ce n'est pas le cas et que le chemin vers le voisin est plus court que son ancien chemin, ou que ce voisin n'est pas dans la liste OPEN, on calcule le coût en F de celui-ci et on dit que le parent de ce noeud est le "current node". Si le voisin n'était pas dans la liste OPEN, on l'ajoute alors à celle-ci.

    On fait boucler ces actions jusqu'à ce que le "current node" soit égal au noeud cible. On sort alors de la boucle et on lance une méthode qui retrace la liste des parents depuis le noeud cible jusqu'au noeud de départ. On obtient alors notre chemin.


    Pseudo-code (résumé) :

    OPEN //the set of nodes to be evaluated
    CLOSED //the set of nodes already evaluated
    add the start node to OPEN

    loop
        current = node in OPEN with the lowest f_cost
        remove current from OPEN
        add current to CLOSED

        if current is the target node //path has been found
            return

        foreach neighbour of the current node
            if neighbour is not traversable or neighbour is CLOSED
                skip to the next neighbour

            if new path to neighbour is shorter OR neighbour is not in OPEN
                set f_cost of neighbour
                set parent of neighbour to current
                if neighbour is not in OPEN
                    add neighbour to OPEN
     */
}
