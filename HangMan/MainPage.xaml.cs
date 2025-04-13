using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HangMan;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
	private readonly string[] _words = new string[]
	{
		"apple", "banana", "orange", "strawberry", "kiwi",
		"computer", "keyboard", "mouse", "monitor", "printer",
		"programming", "algorithm", "database", "network", "software",
		"hangman", "maui", "dotnet", "csharp", "microsoft"
	};

	private readonly Random _random = new Random();
	private string _answer = string.Empty;
	private string _spotlight = string.Empty;
	private string _message = string.Empty;
	private string _currentImage = "hangman1.png";
	private int _mistakes = 0;
	private const int MaxWrong = 6;
	private bool _hasMessage = false;
	private readonly List<char> _guessed = new List<char>();
	private bool _gameOver = false;

	public string Answer
	{
		get => _answer;
		set
		{
			if (_answer != value)
			{
				_answer = value;
				OnPropertyChanged();
			}
		}
	}

	public string Spotlight
	{
		get => _spotlight;
		set
		{
			if (_spotlight != value)
			{
				_spotlight = value;
				OnPropertyChanged();
			}
		}
	}

	public string Message
	{
		get => _message;
		set
		{
			if (_message != value)
			{
				_message = value;
				HasMessage = !string.IsNullOrEmpty(value);
				OnPropertyChanged();
			}
		}
	}

	public bool HasMessage
	{
		get => _hasMessage;
		set
		{
			if (_hasMessage != value)
			{
				_hasMessage = value;
				OnPropertyChanged();
			}
		}
	}

	public string CurrentImage
	{
		get => _currentImage;
		set
		{
			if (_currentImage != value)
			{
				_currentImage = value;
				OnPropertyChanged();
			}
		}
	}

	public int Mistakes
	{
		get => _mistakes;
		set
		{
			if (_mistakes != value)
			{
				_mistakes = value;
				MistakesDisplay = $"Error's {_mistakes} out of {MaxWrong}";
				OnPropertyChanged();
			}
		}
	}

	public string MistakesDisplay { get; set; } = "Error's 0 out of 6";

	public List<char> Guessed
	{
		get => _guessed;
	}

	public bool IsLetterEnabled => !_gameOver;

	public MainPage()
	{
		InitializeComponent();
		BindingContext = this;
		
		// Set initial defaults
		CurrentImage = "hangman1.png";
		MistakesDisplay = "Error's 0 out of 6";
		Message = string.Empty;
		
		// Initialize the game
		ResetGame();
	}

	private void PickWord()
	{
		Answer = _words[_random.Next(_words.Length)];
		Spotlight = CalculateWord(Answer, Guessed);
		
		// Force UI update for the Spotlight
		OnPropertyChanged(nameof(Spotlight));
	}

	private string CalculateWord(string answer, List<char> guessed)
	{
		// For each letter, show the letter if guessed, otherwise show underscore
		// Format exactly as "_ _ _ _ _" with spaces between each character
		char[] result = new char[answer.Length * 2 - 1];
		for (int i = 0; i < answer.Length; i++)
		{
			// Set the actual character or underscore
			result[i * 2] = guessed.Contains(answer[i]) ? char.ToUpper(answer[i]) : '_';
			
			// Add space between characters (except after the last one)
			if (i < answer.Length - 1)
				result[i * 2 + 1] = ' ';
		}
		
		return new string(result);
	}

	private void HandleGuess(char letter)
	{
		if (Guessed.Contains(letter) || _gameOver)
			return;

		Guessed.Add(letter);

		if (Answer.Contains(letter))
		{
			Spotlight = CalculateWord(Answer, Guessed);
			CheckIfGameWon();
		}
		else
		{
			Mistakes++;
			CurrentImage = $"hangman{Mistakes + 1}.png";
			CheckIfGameLost();
		}
	}

	private void CheckIfGameWon()
	{
		// Check if all letters have been guessed - no underscores left
		if (!Spotlight.Contains("_"))
		{
			Message = "You Win!";
			_gameOver = true;
			OnPropertyChanged(nameof(IsLetterEnabled));
			UpdateStatus();
		}
	}

	private void UpdateStatus()
	{
		// This triggers property changed for relevant UI elements
		OnPropertyChanged(nameof(Message));
		OnPropertyChanged(nameof(MistakesDisplay));
		OnPropertyChanged(nameof(CurrentImage));
		OnPropertyChanged(nameof(Spotlight));
	}

	private void CheckIfGameLost()
	{
		if (Mistakes >= MaxWrong)
		{
			// Show the full word in uppercase when the player loses
			Spotlight = string.Join(" ", Answer.ToUpper().ToCharArray());
			
			// Set the loss message - shorter to avoid layout issues
			Message = $"You Lost! Word: {Answer.ToUpper()}";
			_gameOver = true;
			OnPropertyChanged(nameof(IsLetterEnabled));
			UpdateStatus();
		}
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		if (sender is Button button && char.TryParse(button.Text, out char letter))
		{
			// Gray out the button
			button.IsEnabled = false;
			button.BackgroundColor = Colors.Gray;
			
			// Process the guess
			HandleGuess(letter);
		}
	}

	private void ResetGame(object? sender = null, EventArgs? e = null)
	{
		Guessed.Clear();
		Mistakes = 0;
		MistakesDisplay = "Error's 0 out of 6";
		CurrentImage = "hangman1.png";
		Message = string.Empty;
		_gameOver = false;
		
		// Reset all alphabet buttons
		foreach (var button in GetAllAlphabetButtons())
		{
			button.IsEnabled = true;
			button.BackgroundColor = Colors.Indigo;
		}
		
		// Pick a new word and update the Spotlight
		PickWord();
		
		// Make sure UI is updated
		OnPropertyChanged(nameof(IsLetterEnabled));
		OnPropertyChanged(nameof(MistakesDisplay));
		OnPropertyChanged(nameof(Spotlight));
	}
	
	private IEnumerable<Button> GetAllAlphabetButtons()
	{
		// Find all letter buttons in the FlexLayout
		List<Button> buttons = new List<Button>();
		
		// Find all child buttons with a single letter as Text
		foreach (var element in this.GetVisualTreeDescendants())
		{
			if (element is Button button && 
				button.Text?.Length == 1 && 
				char.IsLetter(button.Text[0]))
			{
				buttons.Add(button);
			}
		}
		
		return buttons;
	}

	public new event PropertyChangedEventHandler? PropertyChanged;

	protected new virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName ?? string.Empty));
	}
}

