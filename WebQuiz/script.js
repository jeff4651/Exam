const questions = [
    { id: 1, chapter: 1, text: "What is 2 + 2?", answers: ["3", "4", "5"], correct: 1 },
    { id: 2, chapter: 1, text: "What is the capital of France?", answers: ["Berlin", "Paris", "London"], correct: 1 },
    { id: 3, chapter: 2, text: "Which type is the base of all types in C#?", answers: ["object", "int", "string"], correct: 0 },
];

let stats = { incorrect: {}, total: 0 };
let employeeId = "";
let quizQuestions = [];
let current = 0;
let correctCount = 0;

function loadStats() {
    const data = localStorage.getItem('quizStats');
    if (data) stats = JSON.parse(data);
}

function saveStats() {
    localStorage.setItem('quizStats', JSON.stringify(stats));
}

function setScreen(html) {
    document.getElementById('screen').innerHTML = html;
}

function start() {
    loadStats();
    setScreen(`
        <label>Employee ID <input id="emp" /></label><br>
        <button onclick="chooseMode()">Start</button>
    `);
}

function chooseMode() {
    employeeId = document.getElementById('emp').value.trim();
    if (!employeeId) return;
    setScreen(`
        <button onclick="chapterMode()">Quiz by Chapter</button><br>
        <button onclick="mistakeMode()">Frequent Mistakes</button><br>
        <button onclick="fullMode()">Full Quiz</button>
    `);
}

function chapterMode() {
    setScreen(`
        <label>Chapter <input id="chapter" type="number" /></label><br>
        <button onclick="beginChapter()">Start</button>
    `);
}

function beginChapter() {
    const ch = parseInt(document.getElementById('chapter').value, 10);
    quizQuestions = questions.filter(q => q.chapter === ch);
    if (quizQuestions.length === 0) {
        setScreen('No questions in this chapter.<br><button onclick="chooseMode()">Back</button>');
    } else {
        startQuiz();
    }
}

function mistakeMode() {
    const sorted = questions.filter(q => stats.incorrect[q.id])
                            .sort((a,b) => stats.incorrect[b.id]-stats.incorrect[a.id])
                            .slice(0,5);
    if (sorted.length === 0) {
        setScreen('No mistakes recorded.<br><button onclick="chooseMode()">Back</button>');
    } else {
        quizQuestions = sorted;
        startQuiz();
    }
}

function fullMode() {
    quizQuestions = [...questions];
    startQuiz();
}

function startQuiz() {
    current = 0;
    correctCount = 0;
    showQuestion();
}

function showQuestion() {
    if (current >= quizQuestions.length) {
        stats.total++;
        saveStats();
        setScreen(`Score: ${correctCount} / ${quizQuestions.length}<br>Total quizzes taken: ${stats.total}<br><button onclick="chooseMode()">Menu</button>`);
        return;
    }
    const q = quizQuestions[current];
    let html = `<div>${q.text}</div>`;
    q.answers.forEach((ans, idx) => {
        html += `<label><input type="radio" name="ans" value="${idx}"> ${ans}</label><br>`;
    });
    html += `<button onclick="submit()">Submit</button>`;
    setScreen(html);
}

function submit() {
    const q = quizQuestions[current];
    const choice = document.querySelector('input[name="ans"]:checked');
    if (!choice) return;
    const ans = parseInt(choice.value, 10);
    if (ans === q.correct) {
        correctCount++;
        alert('Correct');
    } else {
        alert('Wrong. Correct answer: ' + q.answers[q.correct]);
        stats.incorrect[q.id] = (stats.incorrect[q.id] || 0) + 1;
    }
    current++;
    showQuestion();
}

start();
