# Indian Uphill Bus Simulator 3D

A Unity-based bus simulation game.

## Play Online

Once deployed, the game will be available at your GitHub Pages URL:
`https://<your-username>.github.io/IndianUphillBusSimulator/`

## Building for WebGL

This repository includes a GitHub Actions workflow that automatically builds and deploys the game to GitHub Pages.

### Setup Requirements

To enable the automated build and deployment, you need to configure the following:

#### 1. Unity License Secrets

Add the following secrets to your repository (Settings → Secrets and variables → Actions):

- `UNITY_LICENSE` - Your Unity license file content (for personal/professional licenses)
- `UNITY_EMAIL` - Your Unity account email
- `UNITY_PASSWORD` - Your Unity account password

**How to obtain your Unity License:**

For Unity Personal or Professional licenses, follow the [game-ci activation guide](https://game.ci/docs/github/activation).

#### 2. Enable GitHub Pages

1. Go to your repository Settings → Pages
2. Under "Build and deployment", select "GitHub Actions" as the source

### Manual Build

If you prefer to build manually:

1. Open the project in Unity 2018.4.13f1
2. Go to File → Build Settings
3. Select WebGL as the platform
4. Click "Build" and choose an output folder

## Project Structure

- `Assets/` - Game assets including scripts, scenes, and resources
- `Packages/` - Unity package dependencies
- `ProjectSettings/` - Unity project settings

## Unity Version

This project uses Unity 2018.4.13f1.

## License

Please refer to the license file for usage terms.
