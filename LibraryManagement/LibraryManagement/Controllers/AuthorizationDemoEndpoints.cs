using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.Controllers
{
    public static class AuthorizationDemoEndpoints
    {
        public static IEndpointRouteBuilder MapAuthorizationDemoEndpoints(this IEndpointRouteBuilder app)
        {
            //Chỉ dành cho admin quản lý hệ thống
            app.MapGet("/AdminOnly", AdminOnly);

            //Admin & Librarian có thể quản lý sách
            app.MapGet("/AdminOrLibrarian",
            [Authorize(Roles = "Admin, Librarian")] () =>
            {
                return "Admin Or Librarian";
            });

            //Truy cập tài khoản cá nhân, lịch sử mượn sách	
            app.MapGet("/MemberAccess",
            [Authorize(Roles = "Admin, Librarian, Member")] () =>
            {
                return "Member";
            });
            app.MapGet("/MemberAccess2",
            [Authorize(Roles = "Member")] () =>
            {
                return "Member2";
            });
            //Guest (truy cập thông tin chung)
            app.MapGet("/GuestAccess", () => "Guest Access: Public Library Information")
               .AllowAnonymous();
            return app;
        }
        [Authorize(Roles = "Admin")]
        private static string AdminOnly()
        {
            return "Admin Only";
        }
    }
}
