# Haunted Mansion VR/AR Experience

## Overview
A spine-chilling VR/AR horror experience set in a haunted mansion. Explore eerie environments, encounter supernatural entities, and survive the night in this immersive horror game built with Unity.

## Project Structure

### Key Assets
- **3D Models**:
  - Haunted Mansion (DAE format with detailed textures)
  - Granny Character (Static model with optional animation support)
  - Environmental Assets:
    - Decorative items
    - Interactive objects
  - **Note on Furniture**:
    - The project currently doesn't include furniture assets
    - You can add your own furniture models to enhance the environment
    - Place them in a new folder (e.g., `Assets/Furniture/`) for organization
  - **Note on Animations**:
    - The main Granny model is currently static
    - An animated version is available in `Assets/Meshy_Merged_Animations.glb`
    - To implement animations:
      1. Import the animated GLB model
      2. Set up the Animator Controller
      3. Configure animation states and transitions

- **Scenes**:
  - **Main Game Scene**: `Assets/Scenes/TRY only.unity` (Primary gameplay scene - Use this to play the game)
  - **Development Backups**:
    - `TRY only 1.unity` - Development backup 1
    - `TRY only 2.unity` - Development backup 2
    - `Granny animated drop.unity` - Character animation tests (uses the animated Granny model)
    - `house scene.unity` - Environment testing (includes furniture placement examples)
    - `SampleScene.unity` - Initial test scene
  
  > **Adding Furniture**:
  > - Create a new folder (e.g., `Assets/Furniture/`) for your furniture assets
  > - Import your 3D models (FBX, OBJ, or other supported formats)
  > - To add furniture to your scene:
  >   1. Drag and drop models from the Project window into the Scene view
  >   2. Adjust position, rotation, and scale as needed
  >   3. Add colliders (Box, Mesh, or other collider components)
  >   4. Mark as static for navigation if needed
  > - Recommended furniture types:
  >   - Chairs, tables, and other room furnishings
  >   - Decorative items (vases, paintings, etc.)
  >   - Interactive objects (doors, drawers, etc.)

  > **Note**: Always use `TRY only.unity` as your main scene when building or testing the complete game. Other scenes are development backups and may not contain all features.

- **Scripts**:
  - Custom C# scripts for game mechanics, interactions, and AI behaviors

- **Audio**:
  - Horror ambiance soundtracks
  - Sound effects (screams, environmental sounds)
  - Coin pickup sounds

- **Materials & Textures**:
  - Custom materials for realistic horror aesthetics
  - Texture maps for 3D models
  - Stylized wood and brick textures

## Technical Details

### Requirements
- Unity 2022.3 or later
- Universal Render Pipeline (URP) installed
- VR/AR compatible hardware (for VR/AR functionality)

### Features
- Immersive 3D environment
- Interactive objects and collectibles
- Animated horror characters
- Dynamic lighting and shadows
- Spatial audio for enhanced immersion
- VR/AR support

## Setup Instructions

1. **Clone the repository**
   ```bash
   git clone [repository-url]
   ```

2. **Open the project in Unity**
   - Launch Unity Hub
   - Click 'Add Project' and select the project folder
   - Open the project with the recommended Unity version

3. **Import required packages**
   - The project uses URP, ensure it's installed via Package Manager
   - Import TextMesh Pro if prompted

4. **Build and Run**
   - Open the desired scene from the Scenes folder
   - Configure build settings for your target platform (PC, Android, iOS, etc.)
   - Build and run the project

## Controls

### PC Controls
- **WASD** - Move character
- **Mouse** - Look around
- **Left Click** - Interact with objects
- **Escape** - Pause menu

### VR Controls
- **Left Thumbstick** - Move
- **Right Thumbstick** - Turn/Snap turn
- **Trigger** - Grab/Interact
- **Grip** - Grab objects

## Development Process

### 1. Asset Creation & Import
- **Character Model**:
  - Generated the Granny model using Meshy AI
  - Rigged and animated the model for realistic movements
  - Optimized polygon count for better performance

- **Environment**:
  - Imported Haunted Mansion assets from [RenderHub](https://www.renderhub.com/shredder/haunted-mansion)
  - Manually constructed walls and floors using Unity's ProBuilder
  - Applied high-quality PBR materials and textures to all models
  - Set up proper UV mapping for all imported assets

### 2. Environment Setup
- **Lighting & Atmosphere**:
  - Disabled default directional light for darker ambiance
  - Set ambient light intensity to 0.5 for a spooky atmosphere
  - Added point lights and spotlights for dynamic lighting effects
  - Implemented fog and particle effects for enhanced horror atmosphere

- **Navigation Mesh**:
  - Created NavMesh for AI pathfinding using Unity's Navigation system
  - Set up navigation areas and obstacles for dynamic pathfinding
  - Optimized NavMesh for performance in large environments

### 3. Player Mechanics
- **First-Person Controller**:
  - Implemented smooth camera movement with mouse look
  - Added walking and running mechanics with head bobbing
  - Integrated footstep sounds based on surface type
  - Added crouching and leaning mechanics

- **Interaction System**:
  - Created raycast-based interaction system
  - Implemented item pickup and inventory system
  - Added context-sensitive interaction prompts

### 4. Enemy AI (Granny)
- **AI Behavior**:
  - Implemented finite state machine for AI states (Patrol, Chase, Attack)
  - Added hearing and vision detection systems
  - Created dynamic pathfinding with obstacle avoidance
  - Implemented AI memory system for player last known position

- **Attack Mechanics**:
  - Proximity-based kill system
  - Scream sound effect on player detection
  - Visual and audio feedback for attacks

### 5. Gameplay Systems
- **Key & Door System**:
  - Scripted key collection mechanics
  - Interactive door system with lock/unlock functionality
  - Visual feedback for locked/unlocked states

- **Torch System**:
  - Dynamic torch lighting with flicker effect
  - Battery life system
  - Interaction with environment (lighting candles, etc.)

### 6. Audio Implementation
- **Sound Design**:
  - Background music with dynamic intensity based on game state
  - 3D spatial audio for environmental sounds
  - Footstep sounds based on surface type
  - Voice lines and monster sounds

- **Audio Mixing**:
  - Custom audio mixer with separate channels (SFX, Music, Voice)
  - Dynamic volume adjustment based on game events
  - Low-pass filter for tension building

### 7. UI/UX
- **Heads-Up Display (HUD)**:
  - Health and sanity indicators
  - Item collection prompts
  - Objective markers

- **Menus**:
  - Main menu with settings
  - Pause menu with game options
  - Game over and win screens
  - Settings menu with graphics and audio options

### 8. Optimization
- **Performance**:
  - Implemented object pooling for frequently spawned objects
  - Added LOD (Level of Detail) systems for complex models
  - Optimized lighting with mixed lighting and light probes
  - Used occlusion culling for better rendering performance

- **Build Settings**:
  - Configured build settings for PC
  - Set up quality presets
  - Optimized build size and loading times

## Contributing

1. Fork the repository
2. Create a new branch for your feature
3. Commit your changes
4. Push to the branch
5. Create a new Pull Request


## Installation Instructions

### For Players (Using Pre-built Executable)
1. Locate the `Build` folder in the project directory
2. The folder contains a ready-to-play version of the game
3. Run `HauntedMansion.exe` to start the game
4. For optimal performance:
   - Run the game as administrator
   - Update your graphics drivers
   - Ensure your system meets the minimum requirements

> **Note**: The game might be flagged by some antivirus software as it's an unsigned executable. This is normal for indie games. You may need to add an exception in your antivirus settings if you encounter any issues.

### For Developers (Building from Source)

#### Prerequisites
- Unity Hub installed
- Unity 2022.3.62f1 (exact version recommended)
- Visual Studio 2019/2022 with Unity workload (or VS Code with C# extension)
- Git (for version control)

#### Setup Steps
1. Clone the repository or download the source code
   ```bash
   git clone https://github.com/yourusername/haunted-mansion.git
   ```
   or download the ZIP and extract it

2. Open Unity Hub and click "Add Project"
3. Navigate to the cloned/extracted repository folder and select it
4. Wait for Unity to import all assets (this may take several minutes on first import)
5. Open the main scene at `Assets/Scenes/TRY only.unity`
6. Click the Play button in Unity Editor to test the game

#### Building the Game
1. Go to `File > Build Settings...`
2. Select your target platform (Windows, Mac, Linux)
3. Click "Add Open Scenes" to include the current scene
4. Click "Build" and select an output folder
5. Wait for the build to complete
6. The built game will be in the specified output folder

> **Note for Builds**:
> - The first build might take longer as Unity processes all assets
> - Ensure all required scenes are added to the build settings
> - Test the built executable before distribution

#### Building the Project
1. Go to `File > Build Settings...`
2. Select your target platform (Windows, Mac, Linux)
3. Click "Add Open Scenes" to include the current scene
4. Click "Build" and select an output folder
5. Wait for the build to complete

## Game Controls

### PC Controls
- **WASD** - Move character
- **Mouse** - Look around
- **Left Click** - Interact with objects
- **Escape** - Pause menu

## Troubleshooting

### Common Issues
1. **Game crashes on launch**
   - Update your graphics drivers
   - Verify that your system meets the minimum requirements
   - Try running the game as administrator

2. **Low FPS/Performance issues**
   - Lower the graphics quality in the settings
   - Update your graphics drivers
   - Close other resource-intensive applications

3. **Audio not working**
   - Check your system's audio settings
   - Ensure the correct audio output device is selected
   - Verify that the game's volume is not muted in the audio mixer

4. **Controls not responding**
   - Make sure no other applications are interfering with input
   - Check if your gamepad is properly connected and recognized by Windows
   - Try resetting controls to default in the settings

## Common Setup Issues & Solutions

### 1. Pink/Purple Materials (Missing Shaders)
**Problem**: Imported 3D models appear pink/purple in the scene.
**Solution**:
1. Select the problematic material in the Project window
2. In the Inspector, change the Shader to "Universal Render Pipeline/Lit"
3. Reassign the texture maps (Albedo, Normal, etc.) to their respective slots
4. If using custom shaders, ensure they're compatible with URP (Universal Render Pipeline)

### 2. Materials Not Applying Correctly
**Problem**: Textures don't appear on models or look incorrect.
**Solution**:
1. Verify that all texture files are in the project's Assets folder
2. Check that texture import settings are correct:
   - Set Texture Type to "Default" for color textures
   - Enable "sRGB" for color textures
   - Set Normal Map for normal maps
   - Adjust Max Size based on quality needs
3. For FBX models with embedded textures:
   - Extract Textures from the model
   - Reassign them to the material

### 3. Lighting Issues
**Problem**: Scene is too dark or lighting doesn't look right.
**Solution**:
1. Ensure you have a Global Volume with Post-Processing enabled
2. Check that your scene has appropriate lighting (Directional Light, Point Lights, etc.)
3. Verify that light settings match your project's render pipeline
4. Rebuild lighting (Window > Rendering > Lighting > Generate Lighting)

### 4. Missing Script References
**Problem**: Script references are missing after importing the project.
**Solution**:
1. Reimport all scripts (Right-click Scripts folder > Reimport)
2. If using Visual Studio, regenerate project files (Assets > Open C# Project)
3. Check for compilation errors in the Console window

### 5. Navigation Mesh Not Working
**Problem**: AI characters can't navigate the scene.
**Solution**:
1. Open Window > AI > Navigation
2. Select all static objects that should be walkable
3. Check their Navigation Static flag
4. Bake the navigation mesh (Bake tab in Navigation window)

### 6. Audio Not Playing
**Problem**: No sound effects or music in the game.
**Solution**:
1. Check that Audio Listener is attached to the main camera
2. Verify that Audio Sources are properly configured
3. Ensure audio files are imported with correct settings (Load Type: Compressed in Memory)
4. Check the Audio Mixer setup and volume levels

## Support
For additional help or to report issues, please:
1. Check the [Issues](https://github.com/yourusername/haunted-mansion/issues) page
2. Create a new issue if your problem isn't listed
3. Include your system specifications and steps to reproduce the issue

## License
This project is licensed under the [MIT License](LICENSE).

## Credits
- **3D Models**:
  - Haunted Mansion by Shredder (RenderHub)
  - Character models by Meshy AI

- **Sound Effects**:
  - Horror Ambience by [Creator]
  - Sound effects from [Source]

- **Music**:
  - [Track Name] by [Composer]
  - [Track Name] by [Composer]

- **Development**:
  - [Your Name] - Lead Developer
  - [Team Member] - Level Design
  - [Team Member] - Sound Design

## Special Thanks
- Unity Technologies for the amazing game engine
- The open-source community for various tools and assets
- All playtesters for their valuable feedback

---
*Last Updated: November 5, 2025*
