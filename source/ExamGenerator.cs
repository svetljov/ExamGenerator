namespace GenerateExams
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    public class ExamGenerator : IExamGenerator
    {
        private const string PageBreakCommand = @"\pagebreak";
        private const string EndDocumentCommand = @"\end{document}";
        
        private string inputFileName, outputFileName, inputPath;
        private int numberOfTasks;
        private float coefficientC1, coefficientC2, coefficientC3;
        private Dictionary<int, KeyValuePair<string, int>> tasks;
        private List<string> inputLines;
        public ExamGenerator(string inputFileName, int numberOfTasks, float coefficientC1, float coefficientC2, float coefficientC3)
        {
            this.inputFileName = inputFileName;
            this.inputPath = GetInputPath();
            this.outputFileName = GetOutputFilename();
            this.inputLines = new List<string>();
            this.numberOfTasks = numberOfTasks;
            this.coefficientC1 = coefficientC1;
            this.coefficientC2 = coefficientC2;
            this.coefficientC3 = coefficientC3;
        }
        public Dictionary<int, KeyValuePair<string, int>> Tasks
        {
            get
            {
                return this.tasks;
            }
            private set
            {
                this.tasks = value;
            }
        }
        public void ExportExams(string examName, int numberOfExams)
        {
            this.inputLines = new List<string>(GetInputLines());
            RemoveCommentedText();
            this.Tasks = GetTasks();

            List<string> exam;

            string preamble = File.ReadAllText(this.inputPath + "Preamble.tex");

            File.WriteAllText(this.outputFileName, preamble, Encoding.Default);

            for (int i = 0; i < numberOfExams; i++)
            {
                File.AppendAllText(this.outputFileName, Text.text1(examName) + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                File.AppendAllText(this.outputFileName, @"\vspace{3mm}" + Environment.NewLine + "Три имена, факултетен номер:" + new String('.', 100) + Environment.NewLine + @"\vspace{3mm}" + Environment.NewLine + Environment.NewLine);
                File.AppendAllText(this.outputFileName, Text.text2(this.coefficientC1, this.coefficientC2, this.coefficientC3) + Environment.NewLine + Environment.NewLine + Environment.NewLine);

                exam = GenerateExam(i);
                File.AppendAllLines(this.outputFileName, exam);
                File.AppendAllText(this.outputFileName, Environment.NewLine + PageBreakCommand + Environment.NewLine);
            }

            File.AppendAllText(this.outputFileName, EndDocumentCommand);
        }

        private IEnumerable<string> GetInputLines()
        {
            return Regex.Split(File.ReadAllText(this.inputFileName), @"^(\d+)\.", RegexOptions.Multiline);
        }

        private string GetInputPath()
        {
            int indexOfLastSlash = this.inputFileName.LastIndexOf('\\');

            return this.inputFileName.Substring(0, indexOfLastSlash + 1);
        }

        private string GetOutputFilename()
        {
            return $"{this.inputPath}\\Exam.tex";
        }

        private List<string> GenerateExam(int examNumber)
        {
            List<string> exam = new List<string>();
            List<int> randomSample = GetRandomSample(examNumber);

            exam.Add(@"\begin{enumerate}");

            for (int i = 0; i < this.numberOfTasks; i++)
            {
                var currentTask = this.tasks[randomSample[i]];
                string taskText = currentTask.Key;
                int numberOfEmptyLines = currentTask.Value;
                exam.Add($"\\item {taskText}");

                //for (int j = 0; j < numberOfEmptyLines; j++)
                //{
                //    exam.Add($"\\\\");
                //}
                exam.Add($"\\\\[{numberOfEmptyLines}\\baselineskip]");
    }

            exam.Add(@"\end{enumerate}");

            return exam;
        }
        private List<int> GetRandomSample(int examNumber)
        {
            Random a = new Random(DateTime.Now.Ticks.GetHashCode() + examNumber);
            List<int> randomList = new List<int>();
            int myNumber;
            int count = 0;

            while (count < this.numberOfTasks)
            {
                myNumber = a.Next(0, this.Tasks.Count);
                if (!randomList.Contains(myNumber))
                {
                    randomList.Add(myNumber);
                    count++;
                }
            }

            return randomList;
        }

        private Dictionary<int, KeyValuePair<string, int>> GetTasks()
        {
            string[] theTasks = this.inputLines.ToArray();
            Dictionary<int, KeyValuePair<string, int>> list = new Dictionary<int, KeyValuePair<string, int>>();

            for (int i = 1; i < (theTasks.Length - 1) / 2; i++)
            {
                string task = theTasks[2 * i];

                int index = task.IndexOf("lines ");
                string taskText = task.Substring(0, index);
                var aa = task.Substring(index + 6, task.Length - index - 8);
                int numberOfLines = int.Parse(aa);
                var kvp = new KeyValuePair<string, int>(taskText.Trim(), numberOfLines);
                list.Add(i - 1, kvp);
            }

            return list;
        }

        private void RemoveCommentedText()
        {
            for (int i = 0; i < this.inputLines.Count; i++)
            {
                string line = this.inputLines[i];

                int indexOfPercent = line.IndexOf("%");

                if (indexOfPercent != -1 && line[indexOfPercent - 1] != '\\')
                {
                    this.inputLines[i] = line.Substring(0, indexOfPercent);
                }
            }
        }
    }
}
