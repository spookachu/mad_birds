# Angry Birds Revisited (Mad Birds)

## Overview
**Mad Birds** is an interactive game with an emphasis on physics-based interactions, inspired by the classic Angry Birds game, but this time with a twist: the birds are now 3D!   
**Mad Birds** *est un jeu interactif qui utilise des interactions basées sur la physique, inspiré du jeu classique Angry Birds, mais cette fois-ci avec une nouveauté: les oiseaux sont en 3D!* 

Players can explore an open-world environment, interact with objects, and participate in minigames. For now, there are 2 available minigames:  
*Les joueurs peuvent explorer une open world, interagir avec des objets et participer à des minigames. Pour l'instant, deux minigames sont disponibles:*

 - **Bullseye** is a target practice game, where you have to launch the bird at pre-set goals. Hit all 3 targets to win!
 - **Bullseye** *est un jeu d'entraînement à la cible, dans lequel vous devez lancer l'oiseau vers des objectifs prédéfinis. Touchez les 3 cibles pour gagner!*
    
 - **Explode** is a minigame where the bird can destroy boxes by blasting in collision with them! Destroy all boxes to win!
 - **Explore** *est un minigames dans lequel l'oiseau peut détruire des boîtes en les percutant! Détruisez toutes les boîtes pour gagner!*

## Gameplay Mechanics
### Minigames
Minigames exist as designated areas within the open world. Players can enter by colliding with the minigame boundary. Upon entry, the camera view resets to focus on the minigame.  
*Les minigames existent en tant que zones désignées dans le open worlds. Les joueurs peuvent y accéder en entrant en collision avec les limites des minigames. Dès l'entrée, la vue de la caméra se réinitialise pour se concentrer sur le minigame.*

Each minigame round grants three lives (throws), displayed at the bottom of the screen. Complete the objective within these attempts to win and earn a power-up.  
*Chaque tour de minigame donne droit à trois vies (jets), affichées en haut de l'écran. Terminez l'objectif au cours de ces tentatives pour gagner et obtenir un bonus.*

- Launching Birds:  
  *Lancer des oiseaux:*  
   - Left-click and hold to aim and visualize the launch trajectory.  
     *Cliquez avec LMB de la mouse et maintenez-le enfoncé pour viser et visualiser la trajectoire de lancement.*  
   - Release LMB to launch the bird.  
     *Relâchez la LMB pour lancer l'oiseau.*  
   - Use power up by clicking Space.  
     *Utilisez le bonus en cliquant sur Space.*  
- Exiting Minigames: Press Q to exit and return to your original position.  
  *Quitter les mini-jeux : Appuyez sur Q pour quitter le jeu et revenir à votre position initiale.*  

### Player Movement
- The player moves in first-person using a CharacterController.  
  *Le joueur se déplace à la première personne à l'aide d'un CharacterController.*  
- Movement: WASD keys for walking, mouse for looking around.  
  *Mouvement : WASD pour marcher, mouse pour regarder autour de soi.*  
- Inside minigames, the camera remains fixed to enhance focus.  
  *Dans les minigames, la caméra reste fixe pour faciliter la mise au point.*  

Main controls are also always displayed on-screen.  
*Les commandes principales sont également toujours affichées à l'écran.*

##  Technical Details
### Player Setup
- **Components:** CharacterController, Collider (Capsule Collider)
- **Scripts:** PlayerMovement: Handles movement, rotation, and camera tracking.

### Lives
- **Scripts:** LifeManager: Sets up and tracks for each minigame the lives across a round.  
  - 3 lives per minigame round  
    *3 vies par tour de mini-jeu*  
  - A throw takes away a life, no matter if you hit the target or fail.  
    *Un lancer enlève une vie, peu importe que vous atteigniez la cible ou que vous échouiez.*  
  - If goal is reached within the allocated lives, the game is won.  
    *Si l'objectif est atteint dans le temps imparti, la partie est gagnée.*  
 
### Power Ups
- **Scripts:** PowerUpManager: Tracks for each minigame if the player has a power up of the corresponding type and allows/disables use.  
  - Each minigame has a specific power up type.  
    *Chaque minigames a un type de pouvoir spécifique.*
      - Bullseye: The bird doubles in size to make targets easier to hit.  
        *L'oiseau double de taille pour faciliter l'atteinte des cibles.*  
      - Explode: If using the power up, a blast sets set to blow up, destroying everything in its surroundings.  
        *Si vous utilisez ce type de pouvoir, une explosion se produit, détruisant tout ce qui se trouve autour d'elle.*
  - Winning a game grants you one power up. Using it uses it up.  
    *Gagner une partie vous permet d'obtenir un pouvoir. En l'utilisant, vous l'augmentez.*
    
### Minigame System
- **Scripts:** MinigameBoundary: tracks if player passes an object that marks a minigame boundary and if so, blocks movement and changes POV.  
  *MinigameBoundary : vérifie si le joueur passe devant un objet qui marque une limite du minigame et si c'est le cas, bloque le mouvement et change le POV.*
- Triggered by colliding with a minigame boundary.  
  *Déclenché par une collision avec une limite du minigame.*   
- The camera moves to an anchor point and player controls are disabled.  
  *La caméra se déplace vers un point d'ancrage et les commandes du joueur sont désactivées.*  
- Upon exit, the player is reset to their original position.  
  *En quittant le jeu, le joueur retrouve sa position initiale.*

### Trajectory
- **Components:** LineRenderer  
- **Scripts:**: Projectile: parent class for all minigame projectile types. Handles general interactions.  
  *parent classe pour tous les types de projectiles du minigame. Gère les interactions générales.*  
  - When clicking LMB to start launching a bird, a visual aid is created to draw the parabole of the potential throw. This way, the player can see the direction of the projectile.  
    *Lorsque l'on clique sur LMB pour lancer un oiseau, une aide visuelle est créée pour dessiner la parabole du jet potentiel. De cette façon, le joueur peut voir la direction du projectile.*  
  - Furthest points are lower in intensity color, to show effect of gravity and drag over time.  
    *Les points les plus éloignés ont une couleur moins intense, pour montrer l'effet de la gravité et de la traînée au fil du temps.*  

### Objects Physics Setup
- **Components:** Rigidbodies, Colliders, Physics Material (Bouncy)
- **Properties:**
  - Objects should be able to be interacted with. The player can collide with them for environment objects, can pick up projectiles.  
    *Les objets doivent pouvoir être manipulés. Le joueur peut entrer en collision avec eux pour les objets de l'environnement, il peut ramasser des projectiles.*
  - isKinematic: an important property that determines whether objects are affected by collisions.  
    *une propriété importante qui détermine si les objets sont affectés par les collisions.*
  - isTrigger: describes whether objects interact physically or through a script.  
    *décrit si les objets interagissent physiquement ou par l'intermédiaire d'un script.*
  - Mass: Adjusted for realistic movement.  
    *Ajustée pour un mouvement réaliste.*
  - Collision Detection: Continuous for accurate interactions.  
    *Continue pour des interactions précises.*

## Controls
| Action       | Key |
|-------------|----|
| Move        | WASD / Arrow Keys |
| Look Around | Mouse |
| Run         | Left Shift |
| Interact    | Left Click |
| Exit Minigame | Q |

## Future Improvements
- Add more minigame variations with different mechanics (more powerups, different goals,...).  
  *Ajouter des variantes de minigames avec des mécanismes différents (plus de bonus, des objectifs différents,...).*  
- Add levels with increasing difficulty.  
  *Ajouter des niveaux de difficulté croissante.*
- Add different way of interacting with projectiles (body sensors, VR)  
  *Ajouter différentes façons d'interagir avec les projectiles (capteurs corporels, RV, etc.).*

---

Developed by Lelia Erscoi using **Unity**.
