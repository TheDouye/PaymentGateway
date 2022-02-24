using System;
using System.Diagnostics.CodeAnalysis;
using Application.Services;

namespace Infrastructure.Services
{
    [ExcludeFromCodeCoverage(Justification = "Out of the scope. Mock to provide date. In Infrastructure... just to be consistent" +
                                             "Depends on the implementation")]
    public class DateServiceMock : IDateService
    {
        public DateTime Now => DateTime.Now;
    }
}