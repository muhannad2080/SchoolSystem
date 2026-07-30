using System.Data;
using SchoolSystem.DataAccess;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public class MarkService
    {
        private readonly MarkRepository repository = new MarkRepository();

        public DataTable GetAllMarks() => repository.GetAllMarks();
        public bool MarkExists(int studentId, int subjectId, string examType, int excludeId = 0)
            => repository.MarkExists(studentId, subjectId, examType, excludeId);
        public void AddMark(Mark mark) => repository.AddMark(mark);
        public bool UpdateMark(Mark mark) => repository.UpdateMark(mark);
        public bool DeleteMark(int markId) => repository.DeleteMark(markId);
    }
}