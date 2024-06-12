﻿namespace FIAP.TechChalenge.EpicCollections.Api.ApiModels.Response;

public class ApiResponse<TData>
{
    public TData Data { get; private set; }

    public ApiResponse(TData data)
    {
        Data = data;
    }

}
