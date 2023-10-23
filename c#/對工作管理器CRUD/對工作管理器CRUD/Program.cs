using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskScheduler;

/*
    參考網址： https://topic.alibabacloud.com/tc/a/c--view-create-and-delete-system-task-scheduler_1_31_31922840.html
    (1) 先去將 C:\Windows\System32\taskschd.dll 複製到 資料夾下並引用 (像我這邊是多建1個 lib資料夾來放他)
    (2) 參考/TashScheduler/右鍵/屬性/內嵌 InterOp 類型 改為 False

 */

namespace 對工作管理器CRUD
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string taskName = "TaskName1";

            // 建立 工作管理器項目
            TaskSchedulerClass scheduler = new TaskSchedulerClass();
            scheduler.Connect(null, null, null, null);  //連接
            ITaskFolder folder = scheduler.GetFolder("\\");  // 宣告Folder (就拿全部)
            bool taskExist = false;  //要建立的排程名稱是否存在

            foreach (IRegisteredTask t in folder.GetTasks(1))
            {
                if (t.Name == taskName)
                {
                    taskExist = true;
                    break;
                }
            }

            // 不存在才建立
            if (taskExist == false)
            {
                // 基本屬性設定
                ITaskDefinition task = scheduler.NewTask(0);
                task.RegistrationInfo.Author = "ABC";//建立者
                task.RegistrationInfo.Description = "這是測試描述";//描述

                // 設定觸發時機
                /*
                 觸發器有很多種，這個在手動建立計劃任務的時候就會發現，如
                    當然最常用的就是按時間觸發，每隔幾天或每個幾個月的觸發器用IDailyTrigger，網上的很多都是以這個為例。
                    我在這裡就拿ITimerTrigger做個例子，能實現幾分鐘就觸發一次的效果。
                 */
                ITimeTrigger tt = (ITimeTrigger)task.Triggers.Create(_TASK_TRIGGER_TYPE2.TASK_TRIGGER_TIME);
                //迴圈時間 (就是設定的迴圈時間，但並不是我們熟悉的毫秒數。它的值需要滿足“PT1H1M1S”的格式，就是幾小時幾分鐘幾秒執行一次)
                // 像這邊就是只 5分鐘執行一次
                tt.Repetition.Interval = "PT5M";
                // 開始執行時間
                tt.StartBoundary = DateTime.Now.ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToString("HH:mm:ss");//開始執行時間 (格式像： 2013-01-21T14:27:25)

                // 設定動作 (這邊就是 單純執行EXE)
                IExecAction action = (IExecAction)task.Actions.Create(_TASK_ACTION_TYPE.TASK_ACTION_EXEC);
                action.Path = "C:\\Windows\\System32\\calc.exe";

                // 其他設定
                // 對應  工作管理器/新建工作/設定/如果工作執行時間大於以下值即停止 (我猜的)
                task.Settings.ExecutionTimeLimit = "PT0S";  //指 0 秒
                                                            // 對應  工作管理器/新建工作/條件/只有在電腦是使用AC電源時才啟動這個工作
                task.Settings.DisallowStartIfOnBatteries = false;
                // 對應  工作管理器/新建工作/條件/只有電腦閒置下列時間後才啟動工作
                task.Settings.RunOnlyIfIdle = false;

                // 註冊 (taskName 變數 就是 填入排程名稱)
                /*
                 _TASK_LOGON_TYPE.TASK_LOGON_INTERACTIVE_TOKEN 的意思，
                貌似是用當前登入的帳戶去註冊任務（所以登入名稱和密碼都是null），
                而且只有在目前使用者登入的情況下才起作用。

                官方說明參見：http://technet.microsoft.com/zh-cn/library/aa381365
                 */
                try
                {
                    IRegisteredTask regTask = folder.RegisterTaskDefinition(taskName, task, (int)_TASK_CREATION.TASK_CREATE, null, null, _TASK_LOGON_TYPE.TASK_LOGON_INTERACTIVE_TOKEN, "");
                    IRunningTask runTask = regTask.Run(null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"註冊工作管理器發生錯誤： {ex.Message}");
                    Console.WriteLine("----------------------------------------------------------");
                }
            }

            //------------------------------------------------------------------------------------------------

            //查出所有工作管理器上的排程
            TaskSchedulerClass ts = new TaskSchedulerClass(); 
            ts.Connect(null, null, null, null); 
            ITaskFolder f = ts.GetFolder("\\"); 
            foreach (IRegisteredTask t in f.GetTasks(1))
            {
                Console.WriteLine($"排程名稱：{t.Name}");
                Console.WriteLine($"排程描述：{t.Definition}");
                Console.WriteLine($"最後執行時間：{t.LastRunTime}");
                Console.WriteLine($"是否啟用：{t.Enabled}");
                Console.WriteLine($"上次執行結果：{t.LastTaskResult}");
                Console.WriteLine($"執行路徑：{t.Path}");
                Console.WriteLine("------------------------------------------------------------------");
            }

            //---------------------------------------------------------------------------------------------------

            // 刪除工作管理器上的排程
            TaskSchedulerClass ts1 = new TaskSchedulerClass();
            ts1.Connect(null, null, null, null);
            ITaskFolder ts1Folder = ts.GetFolder("\\");
            ts1Folder.DeleteTask(taskName, 0);


            Console.Read();
        }
    }
}
