using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service
{
    public interface IMemberService
    {
        int doAdd(int a, int b);

        //應該要寫CRUD的，但測試而已
    }
}
