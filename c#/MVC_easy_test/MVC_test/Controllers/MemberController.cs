using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Blog.Service;   //先使用

using AutoMapper;

namespace MVC_test.Controllers
{
    public class MemberController : Controller
    {
        #region 邏輯抽換（但不改程式）
        // GET: Member
        //先記得要用上面的Service，要先去加入參考才能用喔
        public string Index()
        {
            IMemberService obj = GetMemberService();   
            //寫成GetMemberService()是要抽換，以後就算邏輯改變了，仍然不用改這裡的程式碼，只要把邏輯的程式碼放上去即可
            int result = obj.doAdd(2, 5);

            return $"you input number add is {result.ToString("D3")}";
        }


        //做到抽換其邏輯而不改上面的function之做法
        private IMemberService GetMemberService()
        {
            //先在WebConfig中的<appSettings>加入其參數資料

            //取得Config的資料（會取到 'Blog.Service.MemberService, Blog.Service' 這個)
            string setting = System.Configuration.ConfigurationManager.AppSettings["memberService"].ToString();

            //得到各自的參數
            string[] attr = setting.Split(',');
            string className = attr[0];
            string dllName = attr[1];

            //把字串變成產生物件(用 Reflation 反映)
            var obj = System.Reflection.Assembly.Load(dllName).CreateInstance(className);

            return (IMemberService)obj;
        }

        #endregion


        #region AutoMap 和 自已寫的Map

        public string Test()
        {
            string result = string.Empty;

            Type t = typeof(Member);
            var propertyInfos = t.GetProperties(); //得到此class的所有屬性

            //得到所以屬性的名稱
            foreach (var item in propertyInfos)
            {
                result += item.Name;
            }


            //用自己寫的map函式來用
            Member m = new Member { Id = 1, Name = "jack" };
            VIP v = new VIP();
            map(m, v);
           


            //用AutoMap來玩（先去NuGet裝AutoMapper，再去using）
            var source = Mapper.Map<VIP>(m);   //這段是跟上方一樣的作用
            var source2 = Mapper.Map<Member, VIP>(m);  //跟上一行一樣，但寫清楚而已


            //--------------------------------------------------------------------------
            //有在Global.asax有定義的，不過跟上面差不多  =>  沒啥影響
            var config = Mapper.Configuration;  //取得在Global.asax的設定資料
            IMapper mapper = new Mapper(config);

            var dest = mapper.Map<Member, VIP>(m);


            return result;
        }


        //自已寫map的函式
        private void map(Member member, VIP vip)
        {
            Type t = vip.GetType();
            Type tMember = member.GetType();

            foreach(var pInfo in t.GetProperties())
            {
                string propertyName = pInfo.Name;
                dynamic memberValue = tMember.GetProperty(propertyName).GetValue(member);

                pInfo.SetValue(vip, memberValue);
            }
        }


        //要用到的class
        public class Member
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class VIP
        {
            public  int  ID { get; set; }
            public string Name { get; set; }
        }

        #endregion

    }
}