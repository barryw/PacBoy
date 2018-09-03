
        ______         ______
        | ___ \        | ___ \
        | |_/ /_ _  ___| |_/ / ___  _   _
        |  __/ _` |/ __| ___ \/ _ \| | | |
        | | | (_| | (__| |_/ / (_) | |_| |
        \_|  \__,_|\___\____/ \___/ \__, |
                                     __/ |
                                    |___/


#### What is this?

This is an implementation of the 1980 classic Namco Pac-Man written using Unity.

#### There are MILLIONS of Pac-Man remakes. Why another one?

I'm not a game developer. I've been a software engineer for well over 20+ years, but have never coded a game. Recently, I got an Oculus Rift and it opened my eyes to the possibilities of VR.

I did some research on development for the Oculus and saw that newer versions of Unity have built-in support for it, so I played around with it a bit. Once I got the basics of the editor down and figured out how things fit together, I was absolutely hooked!

I started with the 3D space shooter and roll-a-ball tutorials which did a great job of explaining how to manipulate game objects, collisions, animation, etc. I knew that designing a 3D game from scratch while trying to learn Unity would probably lead to frustration, so instead I focused on a 2D game that I already knew well - Pac-Man.

#### How can I run this?

Download Unity 2018.2.6f1 It's free, but you'll need to create an account.

Clone this repository to a location on your machine.

Start Unity and select "Open" in the window that appears. Point it at your cloned PacBoy directory.

Once Unity has started, click the "Play" button at the top of the window. It looks like a right-facing triangle.

__NOTE__: This was completely developed on my Mac, so there may be issues with running it on Windows. If you run into any platform specific problems, please let me know!

#### Can I fork this and extend it?

Be my guest. I'm doing this just to learn Unity, but I'm happy to share. My larger goal is to get very good with Unity and maybe make an original VR game.


#### Why Unity? Why not Unreal?

Unity feels much more approachable to me; probably because I've done a bunch of C# development over the years. I've never actually used C++ and just thought that in addition to learning Unreal, I'd also have to learn C++ and it just felt like too steep of a learning curve.


#### Why the hell was this built using 3D?

Because I'm an idiot.

When I started this, I was already deep into extending the space shooter thinking that I'd start learning unity with 3D. When that failed, I repurposed the project for Pac-Man, but didn't think to change from 3D -> 2D

I've already started to rebuild this from scratch using 2D. I'll steal most of the assets and code from this project, but won't have to deal with a Z axis.

#### Is this a from-scratch implementation?

Yes and no.

I started with these existing implementations:

- https://github.com/vilbeyli/Pacman
- https://noobtuts.com/unity/2d-pacman-game

But I could not get the colliders to behave predictably. I also wanted to do most of the implementation from scratch so that I could learn for myself.

What I did learn from the 2 projects above were how to animate Pac-Man and the ghosts as well as how to move the actors. In the end, instead of using colliders, I opted for the original arcade machine's notion of tile based navigation. Each valid tile is mapped as a Vector2 object and I merely have to check whether the actor is about to move to a valid tile to determine whether it would "collide" with a wall. In my opinion, it's much more like the original.

I'm using very little code from these 2 projects, but I'm grateful for what I learned from them.

#### Is the ghost AI the same as the arcade?

Somewhat.

If you haven't read the Pac-Man dossier, I highly recommend reading it: https://www.gamasutra.com/view/feature/3938/the_pacman_dossier.php?print=1

I'm using this to implement as much of the original arcade version as possible. Is it exact? Not even close. Is it very similar? Definitely. I do support chase, scatter and frightened modes for the ghosts, and their chase behavior should be damn close to the arcade.

I believe there are bugs around decisions ghosts make at intersections with respect to distance to target. I'm using floating point math to do distance calculations, and I'm abiding by direction precedence, but sometimes it feels like the ghosts are making the wrong decisions at intersections.

#### What works?

As of 9/3/2018, I have the following mostly working:

- You can play it and it mostly works. Ghosts chase and turn blue when a power pellet is eaten.
- Levels advance and the proper fruits are displayed in the lower right indicating level.
- Ghost speeds should be mostly accurate based on level, ghost mode (chase, scatter, frightened)
- Pac-Man speed should be mostly accurate based on level, ghost mode, eating dots, etc
- Ghosts leave the ghost house at correct times for the first level (based on number of dots. timer needs to be implemented)
- Death animation when Pac-Man's tile and a ghost's tile is the same
- The ghosts should have correct timing for entering scatter and chase modes based on time and level
- Game will end if you've lost all of your lives.

#### What remains?

As of 9/3/2018, the following needs to be implemented (or fixed)

- Attract mode is non-existent
- Demo mode is non-existent
- Intermissions need to be implemented (I REALLY want to use Timeline for this!)
- There are tons of bugs around the edges having to do with all aspects of game play
- There is no 2 player, but some of the code to implement it is there

This is my first game of any sort, so I'm learning game design as well as Unity. I'm absolutely determined to finish this and may implement one other 80s game before moving on to 3D and VR.

Comments, suggestions, PRs are all welcome. If you're a game designer, I'd especially love feedback on how I've structured this. I'm sure I'm violating at least a dozen Unity best practices, so feel free to point out what I could be doing better!

If you fork this and add your own features, all I ask is that you give me credit. You are free to use the code however you'd like.

#### License

The gameplay, the assets/characters, the sounds, etc are all owned by Namco. I claim NO ownership of any of Namco's intellectual property!

The code itself has an MIT license.

The sprites were borrowed from Google's PacMan doodle implementation.

#### Thanks

Thanks to the authors of these projects for getting me started with this. Your projects and tutorial helped me a ton and I really appreciate your work:

- https://github.com/vilbeyli/Pacman
- https://noobtuts.com/unity/2d-pacman-game
