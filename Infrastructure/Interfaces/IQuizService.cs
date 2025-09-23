using Domain.Dtos.QuestionDto;
using Domain.Dtos.QuizDto;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface IQuizService
{
    public Task<Response<string>> CreateQuiz(CreateQuizDto dto);
    public Task<Response<string>> UpdateQuiz(UpdateQuizDto dto);
    public Task<Response<string>> DeleteQuiz(int id);
    public Task<Response<GetQuizDto>> GetQuizById(int id);
    public Task<Response<List<GetQuizDto>>> GetQuizs();
}