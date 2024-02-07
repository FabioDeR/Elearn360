using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.VM.Policies
{
    public class Policies
    {
        public const string IsAdmin = "IsAdmin";
        public const string IsVisitor = "IsVisitor";
        public const string IsProfessor = "IsProfessor";
        public const string IsSuperAdmin = "IsSuperAdmin";
        public const string IsStudent = "IsStudent";
        public const string IsTutor = "IsTutor";


        public static AuthorizationPolicy IsAdminPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole("Admin")
                                                   .Build();
        }
        public static AuthorizationPolicy IsVisitorPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole("Visitor")
                                                   .Build();
        }
        public static AuthorizationPolicy IsProfessorPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole("Professor")
                                                   .Build();
        }
        public static AuthorizationPolicy IsStudentPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole("Student")
                                                   .Build();
        }
        public static AuthorizationPolicy IsSuperAdminfPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole("SuperAdmin")
                                                   .Build();
        }
        public static AuthorizationPolicy IsTutorfPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole("Tutor")
                                                   .Build();
        }
    }
}
