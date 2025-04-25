# Hangman Game

A classic word-guessing game built with .NET MAUI. Try to guess the hidden word by selecting letters, but be careful—you only get 6 mistakes before losing!

![Hangman Game Screenshot](screenshot.png)

## Features

- Clean, responsive UI that works on Windows and Android
- Interactive letter buttons with visual feedback
- Word selection from a customizable dictionary
- Win/loss detection with appropriate feedback
- Dynamic display of the hidden word with proper spacing
- Error tracking with visual hangman progression

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download) or later
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) with .NET MAUI workload

### Installation

1. Clone the repository:
   ```
   git clone https://github.com/yourusername/HangMan.git
   ```

2. Open the solution in Visual Studio 2022:
   ```
   cd HangMan
   start HangMan.sln
   ```

3. Build the solution:
   ```
   dotnet build
   ```

### Running the Game

#### Using Visual Studio

1. Select your target device from the dropdown menu in the toolbar:
   - Windows Machine (to run on your local PC)
   - Android Emulator (if configured)
   - Connected Android device

2. Click the green "Play" button or press F5

#### Using Command Line

```
cd HangMan
dotnet run
```

## How to Play

1. When the game starts, a random word is selected and displayed as underscores.
2. Click on any letter button to guess that letter:
   - If the letter is in the word, it will be revealed in all positions
   - If the letter is not in the word, you'll lose one of your 6 attempts
3. The hangman image will progressively change with each wrong guess
4. Win by guessing all letters in the word before making 6 mistakes
5. If you lose, the full word will be revealed
6. Click "Reset" to start a new game at any time

## Customizing

### Adding New Words

To add or modify the word list, edit the `_words` array in `MainPage.xaml.cs`:

```csharp
private readonly string[] _words = new string[]
{
    "apple", "banana", "orange", "strawberry", "kiwi",
    "computer", "keyboard", "mouse", "monitor", "printer",
    "programming", "algorithm", "database", "network", "software",
    "hangman", "maui", "dotnet", "csharp", "microsoft",
    // Add your own words here
};
```

### Changing the UI

The UI is defined in `MainPage.xaml`. You can modify colors, sizes, and layout by editing this file.

## Project Structure

- `MainPage.xaml`: Main UI definition
- `MainPage.xaml.cs`: Game logic and UI interactions
- `Resources/Images/`: Contains the hangman state images

## Technical Details

The game implements:
- MVVM-like architecture with data binding
- INotifyPropertyChanged for UI updates
- FlexLayout for dynamic keyboard arrangement
- Property setters with change notification

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgements

- Created as part of CSE4500 Platform Computing course
- Built with .NET MAUI 