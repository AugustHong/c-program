using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportExcel
{
    //從資料庫取下來時，除了answer外，其他都先填入進去
    public class Question
    {
        public int questionID { get; set; } //考題id
        public int categoryID { get; set; } //考題類型
        public string answer { get; set; } //學生的答案
        public string correntAns { get; set; } //此題正確答案
        public int score { get; set; } //此題的配分
    }


    class Program
    {
        static void Main(string[] args)
        {
            //Creating a new workbook
            var wb2 = new XLWorkbook();

            //Adding a worksheet (新增sheet的名稱)
            var ws2 = wb2.Worksheets.Add("SubjectExamHistories");

            //資料庫的欄位名稱
            string[] subExamColName = { "ExaineeID", "CategoryID", "QuestionID", "Answer", "CorrectAnswer", "Result", "Score", "Createtime", "CreateUser", "ModifyTime", "ModifyUser" };
            string[] c = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K" };

            for (int j = 0; j <= subExamColName.GetUpperBound(0); j++)
            {
                ws2.Cell(c[j] + "1").Value = subExamColName[j];
            }

            string ladder = "88596642";  //梯次

            int id = 1; //考生的id，在一開始填完資料就會有了

            //這邊做測試用所以寫在這，照理來說在隨機出題時就會把資料都寫進去了
            List<Question> questions = new List<Question>();

            Question q = new Question();
            q.answer = "'2"; q.categoryID = 1; q.correntAns = "'2"; q.questionID = 1; q.score = 5;
            questions.Add(q);

            Question q1 = new Question();
            q1.answer = "'4"; q1.categoryID = 1; q1.correntAns = "'2"; q1.questionID = 2; q1.score = 5;
            questions.Add(q1);

            Question q2 = new Question();
            q2.answer = "'1 2"; q2.categoryID = 1; q2.correntAns = "'2 4"; q2.questionID = 3; q2.score = 10;
            questions.Add(q2);

            int i = 2;
            foreach (var ques in questions)
            {
                ws2.Cell("A" + i.ToString()).Value = id;
                ws2.Cell("B" + i.ToString()).Value = ques.categoryID;
                ws2.Cell("C" + i.ToString()).Value = ques.questionID;
                ws2.Cell("D" + i.ToString()).Value = ques.answer;
                ws2.Cell("E" + i.ToString()).Value = ques.correntAns;
                ws2.Cell("F" + i.ToString()).Value = null;
                ws2.Cell("G" + i.ToString()).Value = ques.score;
                ws2.Cell("H" + i.ToString()).Value = DateTime.Now;
                ws2.Cell("I" + i.ToString()).Value = "考試負責人";
                ws2.Cell("J" + i.ToString()).Value = null;
                ws2.Cell("K" + i.ToString()).Value = null;

                i++;
            }

            //--------------------------------------------------------------------以上是考試歷程資料表，以下是成績資料表
            var ws1 = wb2.Worksheets.Add("Scores");

            string[] scoreColName = { "ExaineeID", "ExamType", "Score", "Result", "Createtime", "CreateUser", "ModifyTime", "ModifyUser" };

            for (int j = 0; j <= scoreColName.GetUpperBound(0); j++)
            {
                ws1.Cell(c[j] + "1").Value = scoreColName[j];
            }

            ws1.Cell("A2").Value = id; //考生id
            ws1.Cell("B2").Value = "0"; //學科是0
            ws1.Cell("C2").Value = 90;
            ws1.Cell("D2").Value = null;
            ws1.Cell("E2").Value = DateTime.Now;
            ws1.Cell("F2").Value = "考試負責人";
            ws1.Cell("G2").Value = null;
            ws1.Cell("H2").Value = null;

            //儲存資料，並且設定路徑
            //wb.SaveAs("Showcase.xlsx");
            wb2.SaveAs(@"C:\\Users\\shengsen\\Desktop\\" + ladder + "_testFile_" + id.ToString() + ".xlsx");


        }
    }
}

