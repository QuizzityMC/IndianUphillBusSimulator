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

For Unity 2021+ (including Unity 2021.3.45f1 used in this project), you need a manually activated license file:

1. **Create a separate workflow to request the activation file:**
   - Add the [game-ci/unity-request-activation-file](https://github.com/game-ci/unity-request-activation-file) action to a workflow
   - Run the workflow to generate a `.alf` file
   - Download the artifact containing the `.alf` file

2. **Activate the license on Unity's website:**
   - Go to [license.unity3d.com/manual](https://license.unity3d.com/manual)
   - Upload the `.alf` file
   - Select your license type (Personal, Plus, Pro)
   - Download the `.ulf` license file

3. **Add the license to GitHub Secrets:**
   - Open the `.ulf` file in a text editor
   - Copy the entire contents
   - Create a new secret named `UNITY_LICENSE` with this content

For detailed instructions, follow the [game-ci activation guide](https://game.ci/docs/github/activation).

#### Troubleshooting License Activation Errors

If you see errors like:
- `[Licensing::Module] Error: Access token is unavailable; failed to update`
- `[Licensing::Module] Error: Failed to activate ULF license`
- `HTTP error code 400` from `core.cloud.unity3d.com`

Try these solutions:

1. **Regenerate your license file** - License files can expire or become invalid. Generate a new `.alf` file and activate it again.

2. **Ensure the license matches the Unity version** - The license must be generated for Unity 2021.3.x. If you previously used a different Unity version, you may need a new license.

3. **Check the secret format** - The `UNITY_LICENSE` secret must contain the raw XML content of the `.ulf` file (starting with `<?xml` or `<root>`), not base64 encoded content or a file path.

4. **Verify your Unity account** - Ensure your Unity account email is verified and the subscription is active.

5. **Retry the build** - Unity's licensing servers occasionally have intermittent issues. Try re-running the workflow.

#### 2. Enable GitHub Pages

1. Go to your repository Settings → Pages
2. Under "Build and deployment", select "GitHub Actions" as the source

### Manual Build

If you prefer to build manually:

1. Open the project in Unity 2021.3.45f1
2. Go to File → Build Settings
3. Select WebGL as the platform
4. Click "Build" and choose an output folder

## Project Structure

- `Assets/` - Game assets including scripts, scenes, and resources
- `Packages/` - Unity package dependencies
- `ProjectSettings/` - Unity project settings

## Unity Version

This project uses Unity 2021.3.45f1 LTS.

## License

Please refer to the license file for usage terms.
