<br />
<div align="center">
  <a href="https://github.com/Joooch/TUF-Keyboard-Lights">
    <img src="logo.ico" alt="Logo" width="128" height="128">
  </a>

<h3 align="center">TUF-Keyboard-Lights</h3>

  <p align="center">
    Extended light modes for TUF laptop keyboard
    <br>
    (Tested on ASUS TUF Gaming F15)
  </p>
</div>

## Recommendations
- I do not recomment u to use this program. This program was created by a beginner for fun and may cause some problems.

## Features
+ Music visualization
+ KeyPress Sensor


## NuGet Packages:
* [NAudio](https://github.com/naudio/NAudio/)
* [KeystrokeAPI](https://github.com/fabriciorissetto/KeystrokeAPI/)



## Requirements
* Armoury Crate (tested on 5.0.10.0)

## Usage
1. Clone the repo
2. Compile
    - Visual Studio:
        - Open `.sln` file using Visual Studio
        - TopBar -> Build Solution
    - DotNet:
        ```sh
        dotnet build
        ```
2. Import `ACPIWMI.dll` (aka keyboard library)
    - Go to `C:\Program Files\ASUS\ARMOURY CRATE Service\AuraPlugIn\`
    - Copy `ACPIWMI.dll` to the folder with the compiled program. ( `project/bin/Release/` by default )
3. Run Program
4. Right click on keyboard icon in taskbar and select mode.
