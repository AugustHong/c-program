using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service
{
    public class MemberService : IMemberService
    {
        public int doAdd(int a, int b)
        {
            return a + b;
        }
    }
}
