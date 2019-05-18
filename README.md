## NEED
 - dotnet
 - docker
## Config

in Ogame/Ogame/appsettings.json
- comment or uncomment the line if you want to use localdb on windows without docker


## Launch server

docker-compose up --build

on windows without docker and with local db :
open with visual studio
in nugget console run
- Update-Database
then run with visual studio

## Play to Ogame

### On creation
The first thing to do when playing to this Ogame is to create an account.
When you first access your account your planet will be generated with a defense, 3 mine, a solarpannel and a spaceship level 0.

### Production
Mines and Solar pannels will create ressources each cycle.
However your planet can't stock more than a certain quantity of ressources.
You can upgrade those to increase the rate of production and the maximum capacity of your planet.
The rate of production of your mine of deuterium and your solarpannel are dependant of the distance between your planet and his star.

### Spaceship and attack
You can also uprade your spaceship. When a spaceship is level 1 or more you can use it to attack other planet. A random reachable planet will be select and you can choose to search another planet or attack this planet.
If you attack an unnocupied planet you will take control of it.
If you attack an occupied planet a battle will occur.

During a battle your spaceship will attack and will be attacked by the defense of the attacked planet.
Both your spaceship and the defenses will lose energy.
If your spaceship survive you will take half of the ressource of the attacked planet (except energy)
You can attack your own planet to transfer ressources between planet

You can add other vessel to your planet if you have enough deuterium.

### Energy
Energy is used during each upgrade like other ressources, however energy is also used by spaceship and defenses during battle and travel.
When a defense or a spaceship is idle on a planet they will refilled themself using the energy of the planet.
