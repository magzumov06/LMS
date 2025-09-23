using Domain.Dtos.ProgresDto;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface IProgresService
{
    public Task<Response<string>> CreateProgres (CreateProgresDto dto);
    public Task<Response<string>> UpdateProgres(UpdateProgresDto dto);
    public Task<Response<string>> DeleteProgres(int id);
    public Task<Response<GetProgresDto>> GetProgresById(int id);
    public Task<Response<List<GetProgresDto>>> GetAllProgress();
}