using System;
using System.Collections;


#region Answer 
public class Answer
{
    public int AnswerId { get; set; }
    public string AnswerText { get; set; }

    public Answer(int id, string text)
    {
        AnswerId = id;
        AnswerText = text;
    }

    public override string ToString()
    {
        return $"Answer {AnswerId}: {AnswerText}";
    }
}

#endregion 
//done


#region Question
public class Question
{
    public string Header { get; set; }
    public string Body { get; set; }
    public int Mark { get; set; }
    public Answer[] Answers { get; set; }
    public Answer RightAnswer { get; set; }

    public Question(string header, string body, int mark, Answer[] answers, Answer rightAnswer)
    {
        Header = header;
        Body = body;
        Mark = mark;
        Answers = answers;
        RightAnswer = rightAnswer;
    }

    public override string ToString()
    {
        return $"Question: {Header}\n{Body}\nMark: {Mark}";
    }
}

public class TrueFalseQuestion : Question
{
    public TrueFalseQuestion(string header, string body, int mark, Answer[] answers, Answer rightAnswer)
        : base(header, body, mark, answers, rightAnswer)
    {
    }
}


public class MCQQuestion : Question
{
    public MCQQuestion(string header, string body, int mark, Answer[] answers, Answer rightAnswer)
        : base(header, body, mark, answers, rightAnswer)
    {
    }
}
#endregion
//done


#region Exam
public abstract class Exam
{
    public DateTime TimeOfExam { get; set; }
    public int NumberOfQuestions { get; set; }
    public ArrayList Questions { get; set; }
    public Subject Subject { get; set; }

    public Exam(DateTime time, int numQuestions, Subject subject)
    {
        TimeOfExam = time;
        NumberOfQuestions = numQuestions;
        Questions = new ArrayList();
        Subject = subject;
    }

    public abstract void ShowExam();

    public override string ToString()
    {
        return $"Exam on {TimeOfExam.ToString("yyyy-MM-dd HH:mm")}, {NumberOfQuestions} questions.";
    }
}


public class FinalExam : Exam
{
    public FinalExam(DateTime time, int numQuestions, Subject subject)
        : base(time, numQuestions, subject)
    {
    }

    public override void ShowExam()
    {
        DateTime startTime = DateTime.Now;
        ArrayList userAnswers = new ArrayList();
        bool Flag;

        foreach (Question q in Questions)
        {
            Console.WriteLine(q.Body);
            if (q is MCQQuestion)
            {
                MCQQuestion mcq = (MCQQuestion)q;
                for (int i = 0; i < mcq.Answers.Length; i++)
                {
                    Console.WriteLine($"{i + 1}-{mcq.Answers[i].AnswerText}");
                }

                int userAnswerId;
                do
                {
                    Console.WriteLine("Please Enter The Answer Id");
                    Flag = int.TryParse(Console.ReadLine(), out userAnswerId);

                } while (!Flag || userAnswerId <= 0);


                userAnswers.Add(userAnswerId);
            }
            else if (q is TrueFalseQuestion)
            {
                TrueFalseQuestion tf = (TrueFalseQuestion)q;
                Console.WriteLine("1-True\n2-False");

                int userAnswerId;
                do
                {
                    Console.WriteLine("Please Enter The Answer Id (1 for True | 2 For False)");
                    Flag = int.TryParse(Console.ReadLine(), out userAnswerId);

                } while (!Flag || (userAnswerId != 1 && userAnswerId != 2));

                userAnswers.Add(userAnswerId);
            }
        }

        DateTime endTime = DateTime.Now;
        TimeSpan timeTaken = endTime - startTime;
        int grade = 0;

        for (int i = 0; i < Questions.Count; i++)
        {
            Question q = (Question)Questions[i];
            int userAnswerId = (int)userAnswers[i];
            Answer userAnswer = null;
            if (q is MCQQuestion)
            {
                MCQQuestion mcq = (MCQQuestion)q;
                userAnswer = mcq.Answers[userAnswerId - 1];
            }
            else if (q is TrueFalseQuestion)
            {
                TrueFalseQuestion tf = (TrueFalseQuestion)q;
                userAnswer = tf.Answers[userAnswerId - 1];
            }
            if (userAnswer != null && userAnswer.AnswerId == q.RightAnswer.AnswerId)
            {
                grade += q.Mark;
            }
        }

        Console.Clear();
        Console.WriteLine("\nExam Results:");
        for (int i = 0; i < Questions.Count; i++)
        {
            Question q = (Question)Questions[i];
            int userAnswerId = (int)userAnswers[i];
            Answer userAnswer = null;
            if (q is MCQQuestion)
            {
                MCQQuestion mcq = (MCQQuestion)q;
                userAnswer = mcq.Answers[userAnswerId - 1];
                Console.WriteLine($"Question {i + 1}: {q.Body}\nYour Answer => {userAnswer.AnswerText}\nRight Answer => {q.RightAnswer.AnswerText}\n");
            }
            else if (q is TrueFalseQuestion)
            {
                TrueFalseQuestion tf = (TrueFalseQuestion)q;
                userAnswer = tf.Answers[userAnswerId - 1];
                Console.WriteLine($"Question {i + 1}: {q.Body}\nYour Answer => {userAnswer.AnswerText}\nRight Answer => {q.RightAnswer.AnswerText}\n");
            }
        }
        Console.WriteLine($"Your Grade is {grade} from {Questions.Count * ((Question)Questions[0]).Mark}\nTime {timeTaken}\nThank You");
    }
}

public class PracticalExam : Exam
{
    public PracticalExam(DateTime time, int numQuestions, Subject subject)
        : base(time, numQuestions, subject)
    {
    }

    public override void ShowExam()
    {
        DateTime startTime = DateTime.Now;
        DateTime endTime = DateTime.Now;
        TimeSpan timeTaken = endTime - startTime;

        Console.Clear();
        Console.WriteLine("Practical Exam Results:");
        foreach (Question q in Questions)
        {
            Console.WriteLine($"Question: {q.Body}\nRight Answer => {q.RightAnswer.AnswerText}\n");
        }
        Console.WriteLine($"Time {timeTaken}\nThank You");
    }
}
#endregion
//done


#region Subject
public class Subject
{
    public int SubjectId { get; set; }
    public string SubjectName { get; set; }
    public Exam Exam { get; set; }

    public Subject(int id, string name)
    {
        SubjectId = id;
        SubjectName = name;
    }

    public void CreateExam(Exam examType)
    {
        Exam = examType;
        Exam.Subject = this;
    }

    public override string ToString()
    {
        return $"Subject ID: {SubjectId}, Name: {SubjectName}";
    }
}
#endregion
//done

#region Main
class Program
{
    static void Main(string[] args)
    {
        Subject subject = new Subject(1, "OOP");

        bool Flag;
        int examType;
        do
        {
            Console.WriteLine("Please Enter The Type Of Exam (1 For Practical | 2 For final)");
            Flag = int.TryParse(Console.ReadLine(), out examType);

        } while (!Flag || (examType != 1 && examType != 2));

        int examTime;
        do
        {
            Console.WriteLine("Please Enter the time For Exam From (30 min to 180 min)");
            Flag = int.TryParse(Console.ReadLine(), out examTime);

        } while (!Flag || examTime < 30 || examTime > 180);

        int numQuestions;
        do
        {
            Console.WriteLine("Please Enter the Number Of questions");
            Flag = int.TryParse(Console.ReadLine(), out numQuestions);

        } while (!Flag || numQuestions <= 0);

        Console.Clear();

        Exam exam;
        if (examType == 2)
        {
            FinalExam finalExam = new FinalExam(DateTime.Now, numQuestions, subject);
            for (int i = 1; i <= numQuestions; i++)
            {

                int questionType;
                do
                {
                    Console.WriteLine($"Question {i}\nPlease Enter Type Of Question (1 for MCQ | 2 For True False)");
                    Flag = int.TryParse(Console.ReadLine(), out questionType);

                } while (!Flag || (questionType != 1 && questionType != 2));

                Console.Clear();

                if (questionType == 1)
                {
                    Console.WriteLine("Please Enter Question Body");
                    string questionBody = Console.ReadLine();

                    int mark;

                    do
                    {
                        Console.WriteLine("Please Enter Question Mark");
                        Flag = int.TryParse(Console.ReadLine(), out mark);

                    } while (!Flag || mark <= 0);

                    int numChoices;
                    do
                    {
                        Console.WriteLine("Please Enter Number Of Choices");
                        Flag = int.TryParse(Console.ReadLine(), out numChoices);

                    } while (!Flag || numChoices <= 0);

                    Answer[] answers = new Answer[numChoices];
                    for (int j = 1; j <= numChoices; j++)
                    {
                        Console.WriteLine($"Please Enter Choice Number {j}");
                        string choiceText = Console.ReadLine();
                        answers[j - 1] = new Answer(j, choiceText);
                    }

                    int rightAnswerId;
                    do
                    {
                        Console.WriteLine("Please Enter the right Answer id");
                        Flag = int.TryParse(Console.ReadLine(), out rightAnswerId);

                    } while (!Flag || rightAnswerId <= 0);

                    Answer rightAnswer = answers[rightAnswerId - 1];

                    MCQQuestion mcq = new MCQQuestion(questionBody, questionBody, mark, answers, rightAnswer);
                    finalExam.Questions.Add(mcq);
                    Console.Clear();
                }
                else if (questionType == 2)
                {
                    Console.WriteLine("Please Enter Question Body");
                    string questionBody = Console.ReadLine();

                    int mark;
                    do
                    {
                        Console.WriteLine("Please Enter Question Mark");
                        Flag = int.TryParse(Console.ReadLine(), out mark);

                    } while (!Flag || mark <= 0);

                    int rightAnswerId;
                    do
                    {
                        Console.WriteLine("Please Enter the right Answer id (1 for true | 2 For False)");
                        Flag = int.TryParse(Console.ReadLine(), out rightAnswerId);

                    } while (!Flag || rightAnswerId > 3 || rightAnswerId < 0);

                    Console.Clear();

                    Answer[] answers = new Answer[2]
                    {
                        new Answer(1, "True"),
                        new Answer(2, "False")
                    };
                    Answer rightAnswer = answers[rightAnswerId - 1];

                    TrueFalseQuestion tf = new TrueFalseQuestion(questionBody, questionBody, mark, answers, rightAnswer);
                    finalExam.Questions.Add(tf);
                }
            }
            exam = finalExam;
        }
        else if (examType == 1)
        {
            PracticalExam practicalExam = new PracticalExam(DateTime.Now, numQuestions, subject);
            for (int i = 1; i <= numQuestions; i++)
            {
                Console.WriteLine($"MCQ Question {i}\nPlease Enter Question Body");
                string questionBody = Console.ReadLine();

                int mark;

                do
                {
                    Console.WriteLine("Please Enter Question Mark");
                    Flag = int.TryParse(Console.ReadLine(), out mark);

                } while (!Flag || mark <= 0);


                int numChoices;
                do
                {
                    Console.WriteLine("Please Enter Number Of Choices");
                    Flag = int.TryParse(Console.ReadLine(), out numChoices);

                } while (!Flag);

                Answer[] answers = new Answer[numChoices];
                for (int j = 1; j <= numChoices; j++)
                {
                    Console.WriteLine($"Please Enter Choice Number {j}");
                    string choiceText = Console.ReadLine();
                    answers[j - 1] = new Answer(j, choiceText);
                }

                int rightAnswerId;
                do
                {
                    Console.WriteLine("Please Enter the right Answer id");
                    Flag = int.TryParse(Console.ReadLine(), out rightAnswerId);

                } while (!Flag || rightAnswerId <= 0);

                Answer rightAnswer = answers[rightAnswerId - 1];

                MCQQuestion mcq = new MCQQuestion(questionBody, questionBody, mark, answers, rightAnswer);
                practicalExam.Questions.Add(mcq);
            }
            exam = practicalExam;
        }
        else
        {
            Console.WriteLine("Invalid exam type.");
            return;
        }

        subject.CreateExam(exam);

        Console.WriteLine("Do You Want To Start Exam (Y/N)");
        string startExamInput = Console.ReadLine().ToLower();
        if (startExamInput == "y")
        {
            Console.Clear();
            exam.ShowExam();
        }
        else if (startExamInput == "n")
        {
            Console.WriteLine("Exam not started. Thank You.");
        }
        else
        {
            Console.WriteLine("Invalid input.");
        }
    }
}

#endregion
//done
