using Microsoft.TeamFoundation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Net.Sockets;
using NetFwTypeLib;

/*
    參考網址： https://www.cnblogs.com/pilgrim/p/11173111.html 、 https://www.cnblogs.com/pilgrim/p/11167461.html
    去 NuGet 裝上 Microsoft.VisualStudio.Services.Client
    參考/右鍵/加入參考/COM/NetFwTypeLib
 */

namespace 防火牆設定
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FirewallRuleHelper f = new FirewallRuleHelper();

            // 去  搜尋/打 防火牆 /  找到  具有進階安全性的 Windows Defender 防火牆
            f.AllowPort("TestAllowPort", 8055, Microsoft.TeamFoundation.Common.NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP, Microsoft.TeamFoundation.Common.NET_FW_SCOPE_.NET_FW_SCOPE_ALL);
            List<Microsoft.TeamFoundation.Common.INetFwOpenPort> allOpenPortList = f.GetAllGloballyOpenPorts();
            List<int> allOpenPort = f.GetAllGloballyOpenPortsList();
            f.RemovePort(8055, Microsoft.TeamFoundation.Common.NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP);

            // 去  搜尋/打 防火牆 /  找到  允許應用程式通過WIndow防火牆
            f.AllowApp("C:\\Users\\user\\Desktop\\T\\T\\bin\\Debug\\T.exe", "TestAppAllow");
            List<Microsoft.TeamFoundation.Common.INetFwAuthorizedApplication> allAuthorizedApplicationsList = f.GetAllAuthorizedApplications();
            List<string> allAuthorizedApplicationsNameList = f.GetAllAuthorizedApplicationsName();
            f.RemoveApp("C:\\Users\\user\\Desktop\\T\\T\\bin\\Debug\\T.exe");


            //去 搜尋/ 打 防火牆 / 找到  具有進階安全性的 Windows Defender 防火牆
            f.CreateRule(Microsoft.TeamFoundation.Common.NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP, NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN, NET_FW_ACTION_.NET_FW_ACTION_ALLOW, "TestRule1", "測試規則1", "C:\\Users\\user\\Desktop\\T\\T\\bin\\Debug\\T.exe");
            List<string> allRuleNameList = f.GetAllRuleName();
            f.RemoveRule("TestRule1");

            Console.ReadLine();
        }
    }

    public class FirewallRuleHelper
    {
        // 實體
        Microsoft.TeamFoundation.Common.INetFwMgr netFwMgr;
        INetFwPolicy2 policy2;

        public FirewallRuleHelper()
        {
            netFwMgr = (Microsoft.TeamFoundation.Common.INetFwMgr)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr"));
            policy2 = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
        }

        /// <summary>
        /// 取得所有通過的 應用程式規則
        /// </summary>
        /// <returns></returns>
        public List<Microsoft.TeamFoundation.Common.INetFwAuthorizedApplication> GetAllAuthorizedApplications()
        {
            List<Microsoft.TeamFoundation.Common.INetFwAuthorizedApplication> result = new List<Microsoft.TeamFoundation.Common.INetFwAuthorizedApplication>();

            // 看起來很多，但查出來卻只有幾筆。但至少我們新增的有在上面
            foreach (Microsoft.TeamFoundation.Common.INetFwAuthorizedApplication item in netFwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications)
            {
                result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// 取得所有通過的 應用程式規則 名稱清單
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllAuthorizedApplicationsName()
        {
            return GetAllAuthorizedApplications().Select(x => x.Name).ToList();
        }

        /// <summary>
        /// 允許應用程式通過
        /// </summary>
        /// <param name="appPath">實體應用程式路徑(絕對路徑)</param>
        /// <param name="appName">在防火牆例外顯示的名稱(預設 空白給 應用程式的名稱)</param>
        /// <returns></returns>
        public string AllowApp(string appPath, string appName = "")
        {
            string result = string.Empty;

            try
            {
                if (!File.Exists(appPath))
                {
                    throw new Exception("查無此絕對路徑下的應用程式");
                }

                // 要顯示在防火牆規則下的名稱
                string name = string.IsNullOrEmpty(appName) ? Path.GetFileNameWithoutExtension(appPath) : appName;

                // 查看是否目前規則己存在
                List<string> authorizedApplicationsNameList = GetAllAuthorizedApplicationsName();
                if (authorizedApplicationsNameList.Where(x => x == name).Count() > 0)
                {
                    throw new Exception($"此規則名稱 {name}  己存在");
                }

                // 建立規則
                Microsoft.TeamFoundation.Common.INetFwAuthorizedApplication app = (Microsoft.TeamFoundation.Common.INetFwAuthorizedApplication)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwAuthorizedApplication"));
         
                app.Name = name; //在防火牆例外規則下顯示的名稱
                app.ProcessImageFileName = appPath;  //程式實體路徑
                //objPort.Scope = NET_FW_SCOPE_.NET_FW_SCOPE_ALL;  //針對哪個類型或哪個IP開啟
                //objPort.IpVersion = NET_FW_IP_VERSION_.NET_FW_IP_VERSION_V4;  //指定IP類型(IPV4 或 IPV6)
                app.Enabled = true;  //是否啟用規則

                //加入到防火牆的管理策略
                netFwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(app);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 移除 應用程式通過 規則
        /// </summary>
        /// <param name="appPath">防火牆規則中，實體應用程式路徑(絕對路徑)</param>
        /// <returns></returns>
        public string RemoveApp(string appPath)
        {
            string result = string.Empty;

            try
            {
                netFwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Remove(appPath);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }
    
        /// <summary>
        /// 取得 開放的 Port 清單
        /// </summary>
        /// <returns></returns>
        public List<Microsoft.TeamFoundation.Common.INetFwOpenPort> GetAllGloballyOpenPorts()
        {
            List<Microsoft.TeamFoundation.Common.INetFwOpenPort> result = new List<Microsoft.TeamFoundation.Common.INetFwOpenPort>();

            foreach (Microsoft.TeamFoundation.Common.INetFwOpenPort mPort in netFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts)
            {
                result.Add(mPort);
            }

            return result;
        }

        /// <summary>
        /// 取得 開放的 Port 清單 的 Port
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllGloballyOpenPortsList()
        {
            return GetAllGloballyOpenPorts().Select(x => x.Port).Distinct().ToList();
        }

        /// <summary>
        /// 允許port通過
        /// </summary>
        /// <param name="name">防火牆規則名稱</param>
        /// <param name="port">port</param>
        /// <param name="protocolType">TCP、UDP</param>
        /// <param name="scope">定義範圍(哪些可以用…)</param>
        /// <param name="remoteAddresses">自定義IP範圍</param>
        /// <returns></returns>
        public string AllowPort(string name, int port, Microsoft.TeamFoundation.Common.NET_FW_IP_PROTOCOL_ protocolType, Microsoft.TeamFoundation.Common.NET_FW_SCOPE_ scope, string remoteAddresses = "")
        {
            string result = string.Empty;

            try
            {
                //查是否有相同的規則己存在
                List<Microsoft.TeamFoundation.Common.INetFwOpenPort> globallyOpenPortsList = GetAllGloballyOpenPorts();
                if (globallyOpenPortsList.Where(x => x.Protocol == protocolType && x.Port == port).Count() > 0)
                {
                    throw new Exception($"己有相同的規則己存在  Port = {port}");
                }

                // 建立
                Microsoft.TeamFoundation.Common.INetFwOpenPort objPort = (Microsoft.TeamFoundation.Common.INetFwOpenPort)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwOpenPort"));
                
                objPort.Name = name;  //名稱
                objPort.Port = port;  //port
                objPort.Protocol = protocolType;  //連接類型(TCP/UDP)

                /*
                 *      範圍：
                 *      NET_FW_SCOPE_ALL：所有 
                 *      NET_FW_SCOPE_CUSTOM：自定 
                 *      NET_FW_SCOPE_LOCAL_SUBNET：本地子網
                 *      NET_FW_SCOPE_MAX：僅用於測試
                 */
                objPort.Scope = scope;
                //objPort.IpVersion = NET_FW_IP_VERSION_.NET_FW_IP_VERSION_V4; //指定IP類型(IPV4 或 IPV6)

                //自定義範圍
                if (objPort.Scope == Microsoft.TeamFoundation.Common.NET_FW_SCOPE_.NET_FW_SCOPE_CUSTOM)
                {
                    //空白會有問題要移除
                    objPort.RemoteAddresses = remoteAddresses.Replace(" ", ""); ;//"192.168.1.10,192.168.1.12.......";
                }
                objPort.Enabled = true;  //是否啟用規則

                //加入防火牆規則
                netFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts.Add(objPort);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 移除 Port 規則
        /// </summary>
        /// <param name="port">port</param>
        /// <param name="protocolType">TCP、UDP</param>
        /// <returns></returns>
        public string RemovePort(int port, Microsoft.TeamFoundation.Common.NET_FW_IP_PROTOCOL_ protocolType)
        {
            string result = string.Empty;

            try
            {
                netFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts.Remove(port, protocolType);
            }
            catch(Exception ex) 
            {
                result = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 取得所有規則
        /// </summary>
        /// <returns></returns>
        public List<INetFwRule> GetAllRule()
        {
            List<INetFwRule> result = new List<INetFwRule>();

            // 查出所有規則
            foreach (INetFwRule item in policy2.Rules)
            {
                result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// 取得所有規則名稱
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllRuleName()
        {
            return GetAllRule().Select(x => x.Name).ToList();
        }

        /// <summary>
        /// 邁立規則 
        /// </summary>
        /// <param name="protocolType">TCP、UDP</param>
        /// <param name="direction">輸入規則 or 輸出規則</param>
        /// <param name="action">設置規則是 允許 還是 阻擋(就是指 要讓這程式通過 ， 還是 讓這程式 被阻擋)</param>
        /// <param name="ruleName">規則名稱</param>
        /// <param name="description">描述</param>
        /// <param name="appPath">應用程式(實體路徑)</param>
        /// <param name="localAddresses">加入本地IP</param>
        /// <param name="localPorts">開啟的port</param>
        /// <param name="remoteAddresses">遠端IP位址</param>
        /// <param name="remotePorts">遠端Port</param>
        /// <param name="grouping">分阻名稱</param>
        /// <returns></returns>
        public string CreateRule(Microsoft.TeamFoundation.Common.NET_FW_IP_PROTOCOL_ protocolType, NET_FW_RULE_DIRECTION_ direction, NET_FW_ACTION_ action, string ruleName, string description, string appPath, string localAddresses = "", string localPorts = "", string remoteAddresses = "", string remotePorts = "", string grouping = "")
        {
            string result = string.Empty;

            try
            {
                // 先查規則名是否存在
                List<string> ruleNameList = GetAllRuleName();
                if (ruleNameList.Where(x => x == ruleName).Count() > 0)
                {
                    throw new Exception($"規則名稱  {ruleName}  己存在");
                }

                INetFwRule rule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwRule"));
                rule.Name = ruleName;  //規則名稱
                rule.Description = description; //規則描述
                rule.Direction = direction;  //輸入規則 or 輸出規則
                rule.Protocol = (int)protocolType;  //TCP、UDP
                rule.ApplicationName = appPath; //程式位置 (絕對位置)

                //本地IP   
                if (!string.IsNullOrEmpty(localAddresses))
                {
                    rule.LocalAddresses = localAddresses;
                }

                //本地Port
                if (!string.IsNullOrEmpty(localPorts))
                {
                    //不能有空白
                    rule.LocalPorts = localPorts.Replace(" ", "");// "1-29999, 30003-33332, 33334-55554, 55556-60004, 60008-65535";
                }

                //遠端IP
                if (!string.IsNullOrEmpty(remoteAddresses))
                {
                    rule.RemoteAddresses = remoteAddresses;
                }
                //遠端Port
                if (!string.IsNullOrEmpty(remotePorts))
                {
                    // 不能有空白
                    rule.RemotePorts = remotePorts.Replace(" ", "");
                }

                rule.Action = action;  //規則是允許還阻擋
                rule.Grouping = grouping; //分組名
                rule.InterfaceTypes = "All";
                rule.Enabled = true; //是否啟用

                // 加入規則
                policy2.Rules.Add(rule);
            }
            catch (Exception ex)
            { 
                result = ex.Message;
            }

            return result;
        }
    
        /// <summary>
        /// 移除規則
        /// </summary>
        /// <param name="ruleName">規則名稱</param>
        /// <returns></returns>
        public string RemoveRule(string ruleName)
        {
            string result = string.Empty;

            try
            {
                // 先查規則名是否存在
                List<string> ruleNameList = GetAllRuleName();
                if (ruleNameList.Where(x => x == ruleName).Count() <= 0)
                {
                    throw new Exception($"規則名稱  {ruleName}  不存在");
                }

                policy2.Rules.Remove(ruleName);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }
    }
}
