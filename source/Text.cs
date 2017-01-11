namespace GenerateExams
{
    public static class Text
    {
        public static string text1(string examName)
        {
            return @"
    \clearpage
    \begin{center}
    \textbf{\LARGE ИЗПИТ ПО " + examName.ToUpper()+ @"}\vspace{3mm}\\
    \textbf{\today}
    \end{center}";
        }

        public static string text2(float coefficientN1, float coefficientN2, float coefficientN3)
        {
            return $"Общата оценка се определя по следната формула: ${coefficientN1} + ({coefficientN2} n_1 - {coefficientN3} n_2)$, където $n_1$ и $n_2$ са съответно броят на верните и грешните отговори. Отбележете, че не е необходимо да решавате всички задачи, за да получите отлична оценка.";
        }
    }
}
