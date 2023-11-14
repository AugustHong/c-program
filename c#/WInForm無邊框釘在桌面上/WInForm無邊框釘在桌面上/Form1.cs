using Microsoft.VisualBasic;
using System.Text;

namespace WInForm無邊框釘在桌面上
{
    public class Todo
    {
        public int ok { get; set; }  // 0 (未完成) ， 1(完成)
        public string itemName { get; set; }  //事項名稱
    }

    public partial class Form1 : Form
    {
        public List<Todo> todoList = new List<Todo>();
        public string InitStr = "請輸入以下指定進行動作：\r\n" +
                                "add [待辦事項名稱]  ：(新增事項)\r\n" +
                                "upd [待辦事項編號] [0(未完成)/1(完成)]  ：(修改事項)\r\n" +
                                "del [待辦事項編號]  ：(刪除事項)\r\n" +
                                "exit  ： (離開程式)\r\n";

        public Form1()
        {
            InitializeComponent();
            FixDesktop.SetToDeskTop(this);  //固定在桌面上

            // 計算與父層的邊框長度，與擺放的位置
            int x = System.Windows.Forms.SystemInformation.WorkingArea.Width - this.Size.Width - 50;
            //int y = System.Windows.Forms.SystemInformation.WorkingArea.Height - this.Size.Height - 300;
            int y = 50;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = (Point)new Size(x, y);
            this.FormBorderStyle = FormBorderStyle.None;  //無邊框

            // 附加 事件
            Activated += Form1_Activated;
            Paint += Form1_Paint;

            ShowText();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            FixDesktop.ActiveOrPaint(this);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            FixDesktop.ActiveOrPaint(this);
        }

        /// <summary>
        /// 呈現 當前 文字
        /// </summary>
        public void ShowText()
        {
            // 轉全形編碼會錯，所以要加這行
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string result = InitStr;
            result += "己完成　編號　事項\r\n";
            result += "=====  ===  ====================================\r\n";

            int i = 0;
            foreach (Todo item in todoList) 
            {
                string t = string.Empty;

                // 是否完成
                t += (item.ok == 1) ? "　✔　" : "　　　";
                t += "   ";
                // 編號(先補空白2格)，再轉全形
                string nS = String.Format("{0,2}", (i).ToString());
                t += Strings.StrConv(nS, VbStrConv.Wide, 0);
                t += "    ";

                // 事項
                t += item.itemName;
                t += "\r\n";

                result += t;

                i++;
            }

            textShow.Text = result;
            textInput.Text = "";
        }

        private void textInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string input = textInput.Text;
                List<string> tmpSList = input.Split(' ').ToList();

                try
                {
                    switch (tmpSList[0])
                    {
                        case "add":
                            todoList.Add(new Todo { ok = 0, itemName = tmpSList[1] });
                            break;
                        case "upd":
                            int n = Convert.ToInt32(tmpSList[1]);
                            Todo item = todoList[n];
                            item.ok = tmpSList[2] == "1" ? 1 : 0;
                            break;
                        case "del":
                            int num = Convert.ToInt32(tmpSList[1]);
                            Todo todo = todoList[num];
                            todoList.Remove(todo);
                            break;
                        case "exit":
                            this.Close();
                            break;
                    }

                    // 更新文字
                    ShowText();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "執行發生錯誤，請依照指定語法執行");
                }
            }
        }
    }
}