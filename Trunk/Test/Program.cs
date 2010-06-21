using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Test
{
    class BaseProgram
    {
        protected void GetData2(string a, string b)
        {
        }  
    }
	class Program : BaseProgram
	{
		static void Main(string[] args)
		{


		    var p = new Program();
		    p.Run();

		    Console.WriteLine("Press any key to exit...");
		    Console.ReadLine();
		}

        private void Run()
        {
            string x = "foo";
            string y = "bar";

            //Expression<Func<int>> expr = () => GetData(x,y);
            Expression<Action> expr = () => GetData2(x, y);

            string methodName = ((MethodCallExpression)expr.Body).Method.Name;
            var parameterNames = (from memberExpr in ((MethodCallExpression)expr.Body).Arguments.Cast<MemberExpression>()
                                  select memberExpr.Member.Name).ToArray();

            Console.WriteLine("Func: {0}({1});", methodName, String.Join(",", parameterNames));
        }

        private int GetData(string a, string b)
        {
            return 42;
        }
	}
}
