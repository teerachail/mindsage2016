using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    public interface ILessonTestResultRepository
    {
        Task UpsertTestedResult(LessonTestResult data);
        LessonTestResult GetTestedResult(string classRoomId, string lessonId, string username);
    }
}
