string str = "Hello"; // x009

str = str.ToUpper(); // x010

Console.WriteLine(str); // xo10


ExamResult er = new ExamResult("Carlos", 90);

ExamResult er2 = new ExamResult("Ana", 49);

er2 = er2.WithScore(80);




Console.WriteLine("Enter new exam score: ");
string score = Console.ReadLine();

double newScore = double.Parse(score);

Console.WriteLine(er.Result);

class ExamResult
{
    public string StudentName { get; }
    public double Result { get;  }

    public ExamResult(string name, double result)
    {
        StudentName = name;
        Result = result;
    }
    
    public ExamResult WithScore(double newScore)
    {
        return new ExamResult(StudentName, newScore);
    }
}