***ATENTION*: This is windows only for now**
# 🎮 PongCs
>Objective: Learning the tool used by the masters over at Extremely OK Games to make Celeste. I'm talking about monogame

i should probably make something besides pong clones, geez. Anyway's, i'll add stuff like a main menu and all that like in my raylib version in the next commits

## Game Instructions
The ballMinSpeed variable at line 55 dictates the minimum speed for the ball and ballMaxSpeed at 56 dictates its maximum speed

the Increment variable at 57 dictates by how much the ball's speed will be incremented each time it hits the paddles

MaxSeekForce at 58 dictates how preciselly the opponent will follow the ball. making it anything bigger than 9 is not a good idea

making minimum greater than maximum or making any of these 4 negative is dumb, don't do that