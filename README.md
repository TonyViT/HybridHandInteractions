# Hybrid Hand Interactions
A solution to interact with real physical objects in mixed reality using hand tracking.
![Interaction GIF](https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExajdwaXJmYjRvcnNucWJrZXZ2eTBsa2xtcmozNmhxeGc3OXUyNmFqYiZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/UQfF6TIxjwzEAy1MAO/giphy-downsized.gif)


## Purpose of this project
This project comes from my desire to be able to interact with real objects in mixed reality (e.g. on Meta Quest 3), so that I can, for instance, move a real bottle and activate an augmented-reality animation, or move a real bottle and see it as a fire extinguisher in my hands. Current MR headsets do not support object tracking, so this is not possible out of the box. The idea that I had is that since when you manipulate the physical objects you use your hands, we can maybe track the hands to track the pose of the held object at the same time. With this trick, it is possible to track a moving object in mixed reality.

These hybrid hand interactions require two separate operations to be performed:

- **A set-up moment, which I call "Placement"**, that is when the user specifies where the physical objects are, so they can be tracked by the system. For instance, if the user has a physical bottle on his desk, he has to inform the system of where the bottle is to be able to use it in mixed reality. In this plugin, it is possible to perform this operation using bare hands: the user has just to pinch the physical object to set up its position. After all the objects have been registered, their pose can be saved to be later recovered
- **An interaction moment**, that is where the user actually interacts with the elements. The interaction becomes possible because during the setup the system puts a collider around the physical element, so in the interaction phase, the application can track the relationship between the tracked virtual hand and the collider and try to make it consistent with the interaction between the physical hand and the physical object. The allowed interactions at this moment are:
  - **Grab**: you can take an object in your hand, like when you are holding a bottle
  - **Slide**: you can make an object slide on a line or a surface, like when you are making a paper sheet slide on your desk
  - **Touch**: you can activate an object, like when you are pressing a switch to turn on the light

You can see an example of interaction, with all the involved phases, in the video here below (click the image to open the video):
[![Interaction example](http://img.youtube.com/vi/7egL2W0263Q/0.jpg)](https://www.youtube.com/watch?v=7egL2W0263Q "MR Interaction With Physical Objects")

## Compatibility
The system has been built on top of Unity's XR Interaction Toolkit and AR Foundation. It is so meant to be fully cross-platform among MR headsets. Currently, it has been tested only on Meta Quest 3, though.

## Current Status
The solution is currently in alpha: the code is quite tidy (even if it could be improved even more), but the original assumption of tracking the hands to track the objects proved to fall short because the tracking of the hand is less reliable when the hand is grabbing some objects. There is a need to implement more logic to make the tracking more robust. So your mileage in using this system may vary depending on your use case, but it is already possible to do cool things with it.

## How to use it
I have created a video where I extensively explain everything about this project, from the idea behind it, to how you can customize it to create your own application, not to mention its pitfalls. You can access it by clicking the image here below:
(I know that the video is very long and I'm sorry if at this moment there is not also a quick written guide about this system... I'll see if I can improve this documentation in the next weeks)
[![Deep Technical dive on the project](http://img.youtube.com/vi/MFqpdb8gNDg/0.jpg)](https://www.youtube.com/watch?v=MFqpdb8gNDg "Deep Technical dive on the project")

## Sponsorship
I did this project in my free time to try to gift hybrid object interactions to the XR community and I'm putting it fully open-source with an MIT license so that everyone can use it. In case you want to support me in this project, consider donating to my personal [Patreon](https://www.patreon.com/skarredghost), which helps me keep alive projects I do for the community like my [VR blog](https://skarredghost.com) or these opensource repositories.
If you are a company and you want to become a sponsor of this project, please reach out to me, and let's discuss this opportunity. I would be very happy to be able to dedicate more time to it!

## Authors

* **Antony Vitillo (Skarredghost)** - [Blog](http://skarredghost.com)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Acknowledgments
Almost all the code in this repository has been written by me. I'm also using two Unity samples and those come with a specific Unity licensing, that is added to this project. If someone from Unity believes that this redistribution is not possible, feel free to contact me and we can work together towards the removal of this part.

Have fun :)
