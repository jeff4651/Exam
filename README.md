# Exam

This repository now provides two ways to take quizzes.

## .NET Console Application

1. Install the .NET 6 SDK.
2. From the `DotNetQuiz` directory run:
   ```
   dotnet run
   ```
   The console app asks for an employee ID, lets you choose quiz modes by chapter or by frequently missed questions, records wrong answers, and tracks how many quizzes have been taken.

## Responsive Web Quiz

Open `WebQuiz/index.html` in any modern browser. The page is responsive for mobile and desktop screens. It stores quiz stats in your browser's local storage and offers the same quiz modes as the console app.
