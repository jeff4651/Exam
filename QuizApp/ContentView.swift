import SwiftUI

struct Question {
    let text: String
    let answers: [String]
    let correctIndex: Int
}

struct ContentView: View {
    private let questions: [Question] = [
        Question(text: "What is 2 + 2?", answers: ["3", "4", "5", "6"], correctIndex: 1),
        Question(text: "What is the capital of France?", answers: ["Berlin", "London", "Paris", "Rome"], correctIndex: 2)
    ]

    @State private var currentIndex = 0
    @State private var score = 0
    @State private var showResult = false

    var body: some View {
        if showResult {
            VStack {
                Text("Your Score: \(score) / \(questions.count)")
                    .font(.largeTitle)
                    .padding()

                Button("Restart") {
                    currentIndex = 0
                    score = 0
                    showResult = false
                }
                .padding()
            }
        } else {
            let question = questions[currentIndex]
            VStack(alignment: .leading) {
                Text(question.text)
                    .font(.title)
                    .padding()

                ForEach(0..<question.answers.count, id: \.self) { index in
                    Button(action: {
                        if index == question.correctIndex { score += 1 }
                        if currentIndex + 1 == questions.count {
                            showResult = true
                        } else {
                            currentIndex += 1
                        }
                    }) {
                        Text(question.answers[index])
                            .padding()
                            .frame(maxWidth: .infinity)
                            .background(Color.blue.opacity(0.1))
                            .cornerRadius(8)
                    }
                    .padding(.horizontal)
                }
            }
        }
    }
}

struct ContentView_Previews: PreviewProvider {
    static var previews: some View {
        ContentView()
    }
}
