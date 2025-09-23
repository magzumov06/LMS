using Domain.Dtos.QuestionDto;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface IQuestionService
{
    public Task<Response<string>> CreateQuestion(CreateQuestionDto dto);
    public Task<Response<string>> UpdateQuestion(UpdateQuestionDto dto);
    public Task<Response<string>> DeleteQuestion(int id);
    public Task<Response<GetQuestionDto>> GetQuestionById(int id);
    public Task<Response<List<GetQuestionDto>>> GetQuestions();
}