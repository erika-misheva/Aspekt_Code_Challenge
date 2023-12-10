namespace Domain.Common;

public static class ResponseDetail
{
    public static object NotFound(string entity, int id)
    {
        return new
        {
            Message = $"{entity} with id {id} couldn't be found."
        };
    }

    public static object NotFound(string entity1, string entity2)
    {
        return new
        {
            Message = $"{entity1} or {entity2} couldn't be found."
        };
    }

    public static object Created(int id)
    {
        return new
        {
            CreatedEntityId = id
        };
    }

}
