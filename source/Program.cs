namespace GenerateExams
{
    using System;
    using System.IO;
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0 && File.Exists(args[0]))
            {
                string inputFileName = args[0];

                try
                {
                    Console.Write("Name of exam = ");
                    string examName = Console.ReadLine();

                    Console.Write("Number of exams = ");
                    int numberOfExams = int.Parse(Console.ReadLine());

                    Console.Write("Number of questions (e.g. 15) = ");
                    int numberOfTasks = int.Parse(Console.ReadLine());

                    Console.WriteLine();
                    Console.WriteLine("The mark is calculated by the formula: c1 + c2 n1 - c3 n2, where n1(n2) is the number of correct(incorrect) answers.");
                    Console.WriteLine("Example: c1, c2, c3 = 2, 0.35, 0.1.");
                    Console.Write("c1 = ");
                    float coefficientC1 = float.Parse(Console.ReadLine());
                    Console.Write("c2 = ");
                    float coefficientC2 = float.Parse(Console.ReadLine());
                    Console.Write("c3 = ");
                    float coefficientC3 = float.Parse(Console.ReadLine());

                    IExamGenerator examGenerator = new ExamGenerator(inputFileName, numberOfTasks, coefficientC1, coefficientC2, coefficientC3);

                    examGenerator.ExportExams(examName, numberOfExams);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }
            }
            else
            {
                throw new ArgumentException("Drag and drop a valid text file.");
            }
        }
    }
}
