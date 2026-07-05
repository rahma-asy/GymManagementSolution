using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Common
{
    public sealed record Result(bool success, string? error = null, Resultkind kind = Resultkind.Ok)
    {

        //insted of let him do obj of Result(false,"",Resultkind.Nitfound) i will make helper methods
        public static Result OK() => new(true);

        //parameters هو هيبعت اسم الخطا مثلا وانا ارجع النتيجه علي حسب ال  
        public static Result Fail(string error, Resultkind kind = Resultkind.Conflict)=> new(false, error, kind); 
                                   //[data not foun] عشان كدا كدا عارف من اسم الميثود ايه سبب الايرور بالظبط
        public static Result NotFound(string error = "Not Found")=> new(false, error, Resultkind.NotFound);

        public static Result Validation(string error)=> new(false, error, Resultkind.ValidationFaild);
    }

    public sealed record Result<T>(bool success, T? value, string? error = null, Resultkind kind = Resultkind.Ok)
    {
        public static Result<T> OK(T value)=> new(true, value);

        public static Result<T> Fail(string error, Resultkind kind = Resultkind.Conflict)
                     //in fail not return value
            => new(false, default, error, kind);

        public static Result<T> NotFound(string error = "Not Found")
            => new(false, default, error, Resultkind.NotFound);

        public static Result<T> Validation(string error)
            => new(false, default, error, Resultkind.ValidationFaild);
    }
}
