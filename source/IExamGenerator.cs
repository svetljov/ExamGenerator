namespace GenerateExams
{
    public interface IExamGenerator
    {
        void ExportExams(string examName, int numberOfExams);
    }
}