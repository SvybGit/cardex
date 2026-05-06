using Cardex.DAL.Enums;

namespace Cardex.Model.Dtos;

public class CardResultDto
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public int CardId { get; set; }
    public CardRating Rating { get; set; }
}
